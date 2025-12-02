using SMC.Financeiro.ServiceContract.BLT;
using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Security.Util;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class PermissaoInscricaoForaPrazoDomainService : InscricaoContextDomain<PermissaoInscricaoForaPrazo>
    {
        #region Services

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return Create<InscricaoDomainService>(); }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return Create<OfertaDomainService>(); }
        }

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService
        {
            get { return Create<InscricaoBoletoTituloDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private PermissaoInscricaoForaPrazoInscritoDomainService PermissaoInscricaoForaPrazoInscritoDomainService
        {
            get { return Create<PermissaoInscricaoForaPrazoInscritoDomainService>(); }
        }

        #endregion Services

        public void SalvarPermissoes(PermissaoInscricaoForaPrazoVO permissaoVO)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                PermissaoInscricaoForaPrazo permissaoOld = null;

                try
                {
                    // Verifica se existe outra inscrição fora prazo com período coincidente
                    ValidaInscricaoPeriodoCoincidente(permissaoVO);

                    var processo = ProcessoDomainService.SearchByKey(new SMCSeqSpecification<Processo>(permissaoVO.SeqProcesso),
                                                                                    IncludesProcesso.HierarquiasOferta |
                                                                                    IncludesProcesso.Taxas);

                    var seqOfertas = processo.HierarquiasOferta.Where(f => f.EOferta).Select(s => s.Seq).ToArray();
                    var ofertas = OfertaDomainService.SearchBySpecification(new SMCContainsSpecification<Oferta, long>(x => x.Seq, seqOfertas),
                                                                                IncludesOferta.Taxas_Taxa).ToList();

                    // Ajuste para não permitir permissões após o término de ofertas, se não possuir a permissão necessaria.
                    if (ofertas.Any(f => permissaoVO.DataInicio > f.DataFim) && !SMCSecurityHelper.Authorize(UC_INS_001_11_02.CRIAR_PERMISSAO_INSCRICAO_APOS_PRAZO))
                    {
                        throw new PermissaoInscricaoAposOfertasException();
                    }
                    // Fim do ajuste

                    if (permissaoVO.Seq != 0)
                    {
                        // Busca a permissão do banco para verificações de alteração
                        permissaoOld = this.SearchByKey(new SMCSeqSpecification<PermissaoInscricaoForaPrazo>(permissaoVO.Seq));
                    }

                    var permissao = permissaoVO.Transform<PermissaoInscricaoForaPrazo>();

                    ValidaAlteracoes(permissaoVO, permissaoOld, processo, ofertas);

                    // Executa modificações das taxas
                    if (processo.SeqEvento.HasValue && permissaoVO.DataVencimento.HasValue)
                    {
                        // Verifica se é uma nova permissão ou se ela já existia
                        if (permissaoOld != null)
                        {
                            EditaPermissaoExistente(permissaoVO, permissaoOld, processo);
                        }
                        else
                        {
                            permissao.OfertaPeriodoTaxas = new List<OfertaPeriodoTaxa>();
                            var parametroCrei = RecuperaCrei(processo.SeqEvento.Value, permissaoVO.DataVencimento.Value, SMCContext.User.Identity.Name);
                            var ofertasComTaxa = ofertas.Where(f => f.Taxas.Count > 0);
                            foreach (Oferta oferta in ofertasComTaxa)
                            {
                                foreach (var taxa in oferta.Taxas.GroupBy(g => g.Taxa.SeqTipoTaxa))
                                {
                                    var novaOfertaPeriodoTaxa = CriaNovaOfertaPeriodoTaxa(permissaoVO, processo, taxa.OrderByDescending(o => o.DataFim).First());
                                    novaOfertaPeriodoTaxa.PermissaoInscricaoForaPrazo = permissao;
                                    permissao.OfertaPeriodoTaxas.Add(novaOfertaPeriodoTaxa);
                                }
                            }
                        }
                    }

                    // FIX: O lookup de inscritos não consegue guardar a associação com a permissão. O codigo abaixo busca novamente os sequenciais.
                    // Quando o lookup resolver o problema sozinho, remover o código abaixo.
                    foreach (var inscrito in permissao.Inscritos)
                    {
                        if (inscrito.Seq == 0)
                        {
                            var seqPermissaoInscrito = PermissaoInscricaoForaPrazoInscritoDomainService
                                                        .SearchProjectionBySpecification(new PermissaoInscricaoForaPrazoInscritoSpecification()
                                                        {
                                                            SeqInscrito = inscrito.SeqInscrito,
                                                            SeqPermissao = permissao.Seq
                                                        },
                                                        x => x.Seq).FirstOrDefault();
                            inscrito.Seq = seqPermissaoInscrito;
                        }
                    }

                    this.SaveEntity(permissao);
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        public void DeletarPermissao(long seq)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var permissao = SearchByKey(new SMCSeqSpecification<PermissaoInscricaoForaPrazo>(seq));

                    if (InscricaoDomainService.Count(new InscricaoNoPrazoSpecification()
                    {
                        SeqProcesso = permissao.SeqProcesso,
                        DataInicio = permissao.DataInicio,
                        DataFim = permissao.DataFim
                    }) > 0)
                    {
                        throw new ExcluirPermissaoInscricaoForaPrazoComInscricaoException();
                    }

                    var ofertasTaxas = OfertaPeriodoTaxaDomainService.SearchBySpecification(new OfertaPeriodoTaxaForaPrazoSpecification(seq));
                    foreach (var ofertaTaxa in ofertasTaxas)
                    {
                        OfertaPeriodoTaxaDomainService.DeleteEntity(ofertaTaxa);
                    }

                    this.DeleteEntity(seq);
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        #region Validações

        private void ValidaAlteracoes(PermissaoInscricaoForaPrazoVO permissaoVO, PermissaoInscricaoForaPrazo permissaoOld, Processo processo, List<Oferta> ofertas)
        {
            if (ofertas != null && ofertas.Any())
            {
                // Encontra a menor data das ofertas
                var dataInicioOferta = ofertas.OrderBy(o => o.DataInicio).Select(s => s.DataInicio).First();
                // Encontra a maior data das ofertas
                var dataFimOferta = ofertas.OrderByDescending(o => o.DataFim).Select(s => s.DataFim).First();

                // Verifica se o período da permissão coincide com o período de alguma oferta do processo.
                if (dataInicioOferta <= permissaoVO.DataFim && dataFimOferta >= permissaoVO.DataInicio)
                {
                    throw new PermissaoInscricaoForaPrazoCoincideOfertaException();
                }

                // Verifica se o período da permissão coincide com algum período de taxa das ofertas.
                if (ofertas.Any(o => o.Taxas.Any(t => permissaoVO.DataInicio.Date <= t.DataFim.Date && permissaoVO.DataFim.Date >= t.DataInicio.Date && t.SeqPermissaoInscricaoForaPrazo != permissaoVO.Seq)))
                    throw new PermissaoInscricaoDataTaxaOfertaCoincidenteException();

                if (permissaoOld != null)
                {
                    ValidaAlteracoesNasDatas(permissaoVO, permissaoOld, processo, ofertas, dataInicioOferta, dataFimOferta);
                }

                // Verifica se algum oferta exige pagamento de taxas e se o usuário não informou uma data de vencimento
                if (permissaoVO.DataVencimento == null && ofertas.Any(f => f.ExigePagamentoTaxa))
                {
                    throw new PermissaoInscricaoForaPrazoOfertaComTaxaSemVencimentoException();
                }
            }
        }

        private void ValidaInscricaoPeriodoCoincidente(PermissaoInscricaoForaPrazoVO permissaoVO)
        {
            var specPeriodoCoincidente = new PermissaoInscricaoForaPrazoCoincidenteSpecification()
            {
                Seq = permissaoVO.Seq,
                SeqProcesso = permissaoVO.SeqProcesso,
                DataInicio = permissaoVO.DataInicio,
                DataFim = permissaoVO.DataFim
            };
            if (this.Count(specPeriodoCoincidente) > 0)
            {
                throw new PermissaoInscricaoForaPrazoCoincidenteException();
            }
        }

        private void ValidaAlteracoesNasDatas(PermissaoInscricaoForaPrazoVO permissaoVO, PermissaoInscricaoForaPrazo permissaoOld, Processo processo, List<Oferta> ofertas, DateTime? dataInicioOferta, DateTime? dataFimOferta)
        {
            if (permissaoVO.DataFim < permissaoOld.DataFim
                 && InscricaoDomainService.Count(new InscricaoNoPrazoSpecification() { SeqProcesso = processo.Seq, DataInicio = permissaoVO.DataFim, DataFim = permissaoVO.DataFim }) > 0)
            {
                throw new AlteracaoPermissaoInscricaoForaPrazoComInscricaoException();
            }

            if (permissaoVO.DataInicio > permissaoOld.DataInicio
                    && InscricaoDomainService.Count(new InscricaoNoPrazoSpecification() { SeqProcesso = processo.Seq, DataInicio = permissaoVO.DataInicio, DataFim = permissaoVO.DataInicio }) > 0)
            {
                throw new AlteracaoPermissaoInscricaoForaPrazoComInscricaoException();
            }

            if (permissaoOld.DataVencimento != permissaoVO.DataVencimento
                   && InscricaoBoletoTituloDomainService.Count(new InscricaoProcessoSpecification(processo.Seq, permissaoOld.DataInicio, permissaoOld.DataFim)) > 0)
            {
                throw new PermissaoInscricaoForaPrazoInscricaoComBoletoException();
            }
        }

        #endregion Validações

        #region Executa alterações

        private void EditaPermissaoExistente(PermissaoInscricaoForaPrazoVO permissaoVO, PermissaoInscricaoForaPrazo permissaoOld, Processo processo)
        {
            var ofertasTaxas = OfertaPeriodoTaxaDomainService.SearchBySpecification(new OfertaPeriodoTaxaForaPrazoSpecification(permissaoOld.Seq));

            foreach (var ofertaTaxa in ofertasTaxas)
            {
                if (permissaoOld.DataVencimento != permissaoVO.DataVencimento)
                {
                    ofertaTaxa.SeqParametroCrei = RecuperaCrei(processo.SeqEvento.Value, permissaoVO.DataVencimento.Value, SMCContext.User.Identity.Name);
                }

                // Ajusta as datas das taxas, que possuem a mesma data fim que a permissão, para a nova data fim.
                if (permissaoVO.DataFim != permissaoOld.DataFim)
                {
                    ofertaTaxa.DataFim = permissaoVO.DataFim.Date;
                }

                // Ajusta as datas das taxas que possuem a mesma data de início que a permissão, para a nova data início.
                if (permissaoVO.DataInicio != permissaoOld.DataInicio)
                {
                    ofertaTaxa.DataInicio = permissaoVO.DataInicio.Date;
                }

                OfertaPeriodoTaxaDomainService.SaveEntity(ofertaTaxa);
            }
        }

        private int RecuperaCrei(int seqEvento, DateTime dataVencimentoTitulo, string usuario)
        {
            if (!crei.HasValue)
            {
                crei = FinanceiroService.RecuperaOuGeraCrei(seqEvento, dataVencimentoTitulo, usuario);
            }
            return crei.Value;
        }

        private OfertaPeriodoTaxa CriaNovaOfertaPeriodoTaxa(PermissaoInscricaoForaPrazoVO permissaoVO, Processo processo, OfertaPeriodoTaxa taxaCopia)
        {
            // Cria um novo período de taxa
            return new OfertaPeriodoTaxa()
            {
                SeqParametroCrei = RecuperaCrei(processo.SeqEvento.Value, permissaoVO.DataVencimento.Value, SMCContext.User.Identity.Name),
                DataInicio = permissaoVO.DataInicio.Date,
                DataFim = permissaoVO.DataFim.Date,
                NumeroMaximo = taxaCopia.NumeroMaximo,
                NumeroMinimo = taxaCopia.NumeroMinimo,
                SeqTaxa = taxaCopia.SeqTaxa,
                SeqEventoTaxa = taxaCopia.SeqEventoTaxa,
                SeqOferta = taxaCopia.SeqOferta
            };
        }

        private int? crei;

        #endregion Executa alterações
    }
}