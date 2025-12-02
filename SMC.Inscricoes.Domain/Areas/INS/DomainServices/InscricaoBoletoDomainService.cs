using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoBoletoDomainService : InscricaoContextDomain<InscricaoBoleto>
    {
        #region DomainServices

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService
        {
            get { return this.Create<InscricaoBoletoTituloDomainService>(); }
        }
        #endregion DomainServices

        #region Services

        private IIntegracaoFinanceiroService IntegracaoFinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        #endregion Services

        /// <summary>
        /// Recupera as taxas para uma inscrição
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        public List<InscricaoTaxaOfertaVO> BuscarTaxasOfertaInscricao(long seqInscricao)
        {
            var ofertas = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.Ofertas.Select(s => s.Oferta));

            var taxasOfertaInscricao = new List<InscricaoTaxaOfertaVO>();
            foreach (var oferta in ofertas)
            {
                var taxasOferta = OfertaPeriodoTaxaDomainService.BuscarTaxaInscricaoOfertaVigente(oferta.Seq, seqInscricao);
                var spec = new InscricaoBoletoFilterSpecification { SeqInscricao = seqInscricao, TipoBoleto = TipoBoleto.Inscricao };
                var boleto = this.SearchProjectionByKey(spec, p => new
                {
                    p.Seq,
                    Taxas = p.Taxas.Select(s => new
                    {
                        s.SeqTaxa,
                        s.NumeroItens,
                        s.SeqOferta
                    }),
                    ValorTituloPago = (decimal?)p.Titulos.FirstOrDefault(f => f.DataPagamento.HasValue).ValorTitulo
                });
                foreach (var taxa in taxasOferta)
                {
                    var boletoTaxa = boleto?.Taxas?.FirstOrDefault(f => f.SeqTaxa == taxa.SeqTaxa && (f.SeqOferta == oferta.Seq || f.SeqOferta == null));
                    if (boletoTaxa != null)
                    {
                        taxa.NumeroItens = boletoTaxa.NumeroItens;
                        taxa.SeqInscricaoBoleto = boleto.Seq;
                        taxa.ValorTitulo = boleto.ValorTituloPago;
                    }
                }
                taxasOfertaInscricao.AddRange(taxasOferta);
            }
            return taxasOfertaInscricao;
        }

        /// <summary>
        /// Recupera as taxas para uma inscrição
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        public List<InscricaoTaxaOfertaVO> BuscarTaxasSalvasNoBoletoInscricao(long seqInscricao)
        {
            var ofertas = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.Ofertas.Select(s => s.Oferta));

            var taxasOfertaInscricao = new List<InscricaoTaxaOfertaVO>();

            var spec = new InscricaoBoletoFilterSpecification { SeqInscricao = seqInscricao, TipoBoleto = TipoBoleto.Inscricao };
            var boleto = this.SearchProjectionByKey(spec, p => new
            {
                p.Seq,
                Taxas = p.Taxas.Select(s => new
                {
                    s.SeqTaxa,
                    s.NumeroItens,
                    s.SeqOferta
                }),
                ValorTituloPago = (decimal?)p.Titulos.FirstOrDefault(f => f.DataPagamento.HasValue).ValorTitulo
            });

            foreach (var oferta in ofertas)
            {
                var taxasOferta = OfertaPeriodoTaxaDomainService.BuscarTaxaInscricaoOfertaVigente(oferta.Seq, seqInscricao);

                foreach (var taxa in taxasOferta)
                {
                    var boletoTaxa = boleto?.Taxas?.FirstOrDefault(f => f.SeqTaxa == taxa.SeqTaxa && (f.SeqOferta == oferta.Seq || f.SeqOferta == null));
                    if (boletoTaxa != null)
                    {
                        taxa.NumeroItens = boletoTaxa.NumeroItens;
                        taxa.SeqInscricaoBoleto = boleto.Seq;
                        taxa.ValorTitulo = boleto.ValorTituloPago;

                        if (boletoTaxa.SeqOferta.HasValue)
                        {
                            taxasOfertaInscricao.Add(taxa);

                        }
                        else
                        {

                            if (!taxasOfertaInscricao.Any(a => a.SeqTaxa == taxa.SeqTaxa))
                            {

                                taxasOfertaInscricao.Add(taxa);
                            }
                        }
                    }
                }
            }
            return taxasOfertaInscricao;
        }

        /// <summary>
        /// Recupera as taxas para uma inscrição
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        public List<InscricaoTaxaOfertaVO> BuscarTaxasOfertaInscricaoConfirmacao(long seqInscricao)
        {
            var ofertas = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.Ofertas.Select(s => s.Oferta));

            var taxasOfertaInscricao = new List<InscricaoTaxaOfertaVO>();
            foreach (var oferta in ofertas)
            {
                var taxasOferta = OfertaPeriodoTaxaDomainService.BuscarTaxaInscricaoOfertaVigente(oferta.Seq, seqInscricao);
                var spec = new InscricaoBoletoFilterSpecification { SeqInscricao = seqInscricao, TipoBoleto = TipoBoleto.Inscricao };
                var boleto = this.SearchProjectionByKey(spec, p => new
                {
                    p.Seq,
                    Taxas = p.Taxas.Select(s => new
                    {
                        s.SeqTaxa,
                        s.NumeroItens,
                        s.SeqOferta
                    }),
                    ValorTituloPago = (decimal?)p.Titulos.FirstOrDefault(f => f.DataPagamento.HasValue).ValorTitulo
                });
                foreach (var taxa in taxasOferta)
                {
                    var boletoTaxa = boleto?.Taxas?.FirstOrDefault(f => f.SeqTaxa == taxa.SeqTaxa && (f.SeqOferta == oferta.Seq || f.SeqOferta == null));
                    if (boletoTaxa != null)
                    {
                        taxa.NumeroItens = boletoTaxa.NumeroItens;
                        taxa.SeqInscricaoBoleto = boleto.Seq;
                        taxa.ValorTitulo = boleto.ValorTituloPago;
                    }
                }
                taxasOfertaInscricao.AddRange(taxasOferta);
            }
            //remove todos os itens que não tem valor
            taxasOfertaInscricao.RemoveAll(r => r.NumeroItens == 0);
            //selecionar todas as ofertas que são por oferts
            var ofertasTipoOferta = taxasOfertaInscricao.Where(w => w.TipoCobranca == TipoCobranca.PorOferta).ToList();
            //selecionar todas as ofertas que não são do tipo oferta agrupando e pegando a primeira para não termos taxas duplicadas
            var ofertasDiferentesTipoOferta = taxasOfertaInscricao
                                             .Where(w => w.TipoCobranca != TipoCobranca.PorOferta)
                                             .GroupBy(g => g.SeqTaxa)
                                             .Select(g => g.FirstOrDefault())
                                             .ToList();


            List<InscricaoTaxaOfertaVO> retorno = new List<InscricaoTaxaOfertaVO>();
            retorno.AddRange(ofertasTipoOferta);
            retorno.AddRange(ofertasDiferentesTipoOferta);

            return retorno;
        }

        public bool VerificarExistenciaTaxaInscricao(long seqInscricao)
        {
            var oferta = InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao)
                , x => x.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).Oferta);
            if (oferta != null)
            {
                var spec = new OfertaPeriodoTaxaFilterSpecification { SeqOferta = oferta.Seq, SelecaoInscricao = true };
                if (OfertaPeriodoTaxaDomainService.Count(spec) == 0)
                    return false;
            }
            return true;
        }
        public bool? VerificarPagamentoTaxaInscricao(long seqInscricao)
        {
            var tituloPago = InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao)
                , x => x.Boletos.Where(w => w.SeqInscricao == seqInscricao).FirstOrDefault().Titulos.FirstOrDefault().DataPagamento != null);

            return tituloPago;
        }


        public void SalvarInscricaoTaxasOferta(long seqInscricao, long seqOferta, List<InscricaoTaxaOfertaVO> inscricaoTaxasOferta, short? qtdOfertas, bool teveAlteracaoTaxas)
        {
            //RN_INS_072 - Gravação da Oferta

            // Busca o boleto da inscrição ou cria um caso não exista
            InscricaoBoleto boleto = null;
            if (inscricaoTaxasOferta.Any(x => x.SeqInscricaoBoleto.HasValue))
            {
                long seqBoleto = inscricaoTaxasOferta.First(x => x.SeqInscricaoBoleto.HasValue).SeqInscricaoBoleto.Value;
                boleto = this.SearchByKey(new SMCSeqSpecification<InscricaoBoleto>(seqBoleto), IncludesInscricaoBoleto.Titulos);
            }
            else
            {
                boleto = this.SearchBySpecification(new InscricaoBoletoFilterSpecification
                {
                    SeqInscricao = seqInscricao,
                    TipoBoleto = TipoBoleto.Inscricao
                }, IncludesInscricaoBoleto.Titulos).FirstOrDefault();

                if (boleto == null)
                {
                    boleto = new InscricaoBoleto
                    {
                        TipoBoleto = TipoBoleto.Inscricao,
                        SeqInscricao = seqInscricao,
                    };
                }
            }

            //Cancelar o título existente no histórico de títulos da inscrição, informando a data atual como a data de cancelamento.
            if (boleto.Titulos != null && boleto.Titulos.Count > 0)
            {
                if (teveAlteracaoTaxas)
                {
                    foreach (var titulo in boleto.Titulos.Where(x => !x.Cancelado))
                    {
                        titulo.DataCancelamento = DateTime.Now;
                        IntegracaoFinanceiroService.CancelarTitulo(new CancelarTituloData()
                        {
                            SeqTitulo = (int)titulo.SeqTitulo,
                            Descricao = "Houve alteração nas taxas da inscrição. Um novo boleto será gerado.",
                            UsuarioOperacao = SMCContext.User.Identity.Name
                        });
                    }
                }
            }

            // Informa as taxas do boleto
            boleto.Taxas = new List<InscricaoBoletoTaxa>();
            foreach (var taxa in inscricaoTaxasOferta)
            {
                // Validar número
                var ofertaPeriodo = OfertaPeriodoTaxaDomainService.SearchBySpecification(
                    new OfertaPeriodoTaxaVigenteSpecification() &
                    new OfertaPeriodoTaxaFilterSpecification { SeqOferta = seqOferta, SeqTaxa = taxa.SeqTaxa }
                    , x => x.Taxa.TipoTaxa).FirstOrDefault();
                if (ofertaPeriodo == null)
                {
                    throw new OfertaPeriodoTaxaFechadaException();
                }
                else if (taxa.NumeroItens > ofertaPeriodo.NumeroMaximo)
                {
                    // Excecao de maior maximo
                    throw new NumeroMaximoTaxasExcedidoException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, ofertaPeriodo.NumeroMaximo.HasValue ? ofertaPeriodo.NumeroMaximo.Value : 0);
                }
                else if (taxa.NumeroItens < ofertaPeriodo.NumeroMinimo)
                {
                    // Exceção de numero mínimo
                    throw new NumeroMinimoTaxasExcedidoException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, ofertaPeriodo.NumeroMinimo.HasValue ? ofertaPeriodo.NumeroMinimo.Value : 0);
                }

                // Verificação de segurança, para garantir que o usuário não tenha modificado o valor do campo readonly editando o HTML.
                if (ofertaPeriodo.Taxa.CobrarPorOferta.HasValue && ofertaPeriodo.Taxa.CobrarPorOferta.Value)
                {
                    taxa.NumeroItens = qtdOfertas;
                }
                else if (ofertaPeriodo.NumeroMaximo == ofertaPeriodo.NumeroMinimo)
                {
                    taxa.NumeroItens = ofertaPeriodo.NumeroMinimo;
                }

                if (taxa.NumeroItens > 0)
                {
                    var boletoTaxa = new InscricaoBoletoTaxa
                    {
                        SeqTaxa = taxa.SeqTaxa,
                        SeqInscricaoBoleto = boleto.Seq,
                        NumeroItens = (short)taxa.NumeroItens
                    };
                    boleto.Taxas.Add(boletoTaxa);
                }
            }

            // Salva o boleto
            this.SaveEntity(boleto);
        }

        public void SalvarInscricaoTaxasOfertaAngular(long seqInscricao, List<InscricaoOferta> ofertas, List<InscricaoTaxaOfertaVO> inscricaoTaxasOferta, short? qtdOfertas, bool teveAlteracaoTaxas, long seqGrupoOferta)
        {
            //RN_INS_072 - Gravação da Oferta

            // Busca o boleto da inscrição ou cria um caso não exista
            InscricaoBoleto boleto = null;

            var taxas = new List<InscricaoBoletoTaxa>();

            if (inscricaoTaxasOferta.Any(x => x.SeqInscricaoBoleto.HasValue))
            {
                long seqBoleto = inscricaoTaxasOferta.First(x => x.SeqInscricaoBoleto.HasValue).SeqInscricaoBoleto.Value;
                boleto = this.SearchByKey(new SMCSeqSpecification<InscricaoBoleto>(seqBoleto), IncludesInscricaoBoleto.Titulos | IncludesInscricaoBoleto.Taxas);
            }
            else
            {
                boleto = this.SearchBySpecification(new InscricaoBoletoFilterSpecification
                {
                    SeqInscricao = seqInscricao,
                    TipoBoleto = TipoBoleto.Inscricao,
                }, IncludesInscricaoBoleto.Titulos | IncludesInscricaoBoleto.Taxas).FirstOrDefault();

                if (boleto == null)
                {
                    boleto = new InscricaoBoleto
                    {
                        TipoBoleto = TipoBoleto.Inscricao,
                        SeqInscricao = seqInscricao,
                        Taxas = new List<InscricaoBoletoTaxa>()
                    };
                }
            }

            //Cancelar o título existente no histórico de títulos da inscrição, informando a data atual como a data de cancelamento.
            if (boleto.Titulos != null && boleto.Titulos.Count > 0)
            {
                if (teveAlteracaoTaxas)
                {
                    foreach (var titulo in boleto.Titulos.Where(x => !x.Cancelado))
                    {
                        titulo.DataCancelamento = DateTime.Now;
                        IntegracaoFinanceiroService.CancelarTitulo(new CancelarTituloData()
                        {
                            SeqTitulo = (int)titulo.SeqTitulo,
                            Descricao = "Houve alteração nas taxas da inscrição. Um novo boleto será gerado.",
                            UsuarioOperacao = SMCContext.User.Identity.Name
                        });
                    }
                }
            }

            var taxasPorInscricao = inscricaoTaxasOferta.Where(s => s.TipoCobranca == TipoCobranca.PorInscricao && s.NumeroItens != 0).ToList();

            var taxasPorOferta = inscricaoTaxasOferta.Where(s => s.TipoCobranca == TipoCobranca.PorOferta && s.NumeroItens != 0).ToList();

            var taxasPorQuantidadeOferta = inscricaoTaxasOferta.Where(s => s.TipoCobranca == TipoCobranca.PorQuantidadeOfertas && s.NumeroItens != 0).ToList();

            var taxasPorGrupo = inscricaoTaxasOferta.Where(w => w.GrupoTaxa != null && w.GrupoTaxa.Itens.Any(a => a.SeqTaxa == w.SeqTaxa && w.NumeroItens != 0))
                                .ToList();

            var inscricaoTaxa = new List<InscricaoTaxaOfertaVO>();

            taxasPorInscricao = taxasPorInscricao
                                .Where(w => (w.GrupoTaxa != null && !w.GrupoTaxa.Itens.Any(a => a.SeqTaxa == w.SeqTaxa)) || w.GrupoTaxa == null)
                                .SMCDistinct(s => s.SeqTaxa) // Aplicando o Distinct após o filtro
                                .ToList();

            taxasPorQuantidadeOferta = taxasPorQuantidadeOferta
                                .Where(w => (w.GrupoTaxa != null && !w.GrupoTaxa.Itens.Any(a => a.SeqTaxa == w.SeqTaxa)) || w.GrupoTaxa == null)
                                .SMCDistinct(s => s.SeqTaxa) // Aplicando o Distinct após o filtro
                                .ToList();

            taxasPorInscricao.SMCForEach(f => f.SeqOferta = null);
            taxasPorQuantidadeOferta.SMCForEach(f => f.SeqOferta = null);

            taxasPorGrupo.SMCForEach(f =>
            {
                if (f.TipoCobranca == TipoCobranca.PorInscricao)
                {
                    f.SeqOferta = null;
                }
            });

            inscricaoTaxa.AddRange(taxasPorInscricao);
            inscricaoTaxa.AddRange(taxasPorQuantidadeOferta);
            inscricaoTaxa.AddRange(taxasPorOferta);
            inscricaoTaxa.AddRange(taxasPorGrupo);

            if (boleto.Taxas == null)
            {
                boleto.Taxas = new List<InscricaoBoletoTaxa>();
            }

            foreach (var taxa in inscricaoTaxa)
            {

                var ofertaPeriodo = OfertaPeriodoTaxaDomainService.SearchBySpecification(
                 new OfertaPeriodoTaxaVigenteSpecification() &
                 new OfertaPeriodoTaxaFilterSpecification { SeqOferta = taxa.SeqOferta, SeqTaxa = taxa.SeqTaxa }
                 , x => x.Taxa.TipoTaxa).FirstOrDefault();

                if (taxa.TipoCobranca != TipoCobranca.PorOferta && taxa.SeqOferta == null)
                {
                    ofertaPeriodo = OfertaPeriodoTaxaDomainService.SearchBySpecification(
                    new OfertaPeriodoTaxaVigenteSpecification() &
                    new OfertaPeriodoTaxaFilterSpecification { SeqTaxa = taxa.SeqTaxa, SeqGrupoOferta = seqGrupoOferta }
                    , x => x.Taxa.TipoTaxa).FirstOrDefault();
                }

                if (ofertaPeriodo == null)
                {
                    throw new OfertaPeriodoTaxaFechadaException();
                }
                else if (taxa.NumeroItens > ofertaPeriodo.NumeroMaximo)
                {
                    // Excecao de maior maximo
                    throw new NumeroMaximoTaxasExcedidoException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, ofertaPeriodo.NumeroMaximo.HasValue ? ofertaPeriodo.NumeroMaximo.Value : 0);
                }
                else if (taxa.NumeroItens < ofertaPeriodo.NumeroMinimo)
                {
                    // Exceção de numero mínimo
                    throw new NumeroMinimoTaxasExcedidoException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, ofertaPeriodo.NumeroMinimo.HasValue ? ofertaPeriodo.NumeroMinimo.Value : 0);
                }

                // Verificação de segurança, para garantir que o usuário não tenha modificado o valor do campo readonly editando o HTML.
                if (ofertaPeriodo.Taxa.CobrarPorOferta.HasValue && ofertaPeriodo.Taxa.CobrarPorOferta.Value)
                {
                    taxa.NumeroItens = qtdOfertas;
                }
                else if (ofertaPeriodo.NumeroMaximo == ofertaPeriodo.NumeroMinimo && taxa.TipoCobranca != TipoCobranca.PorQuantidadeOfertas)
                {
                    taxa.NumeroItens = ofertaPeriodo.NumeroMinimo;
                }

                if (taxa.NumeroItens > 0)
                {

                    if (boleto.Taxas.Any(a => a.SeqInscricaoBoleto == boleto.Seq && a.SeqTaxa == taxa.SeqTaxa && a.SeqOferta == taxa.SeqOferta))
                    {
                        foreach (var taxaBoleto in boleto.Taxas)
                        {
                            if (taxaBoleto.SeqInscricaoBoleto == boleto.Seq && taxaBoleto.SeqTaxa == taxa.SeqTaxa && taxaBoleto.SeqOferta == taxa.SeqOferta)
                            {
                                taxaBoleto.NumeroItens = (short)taxa.NumeroItens;
                                taxaBoleto.SeqOferta = taxa.SeqOferta;
                            }
                        }
                    }
                    else
                    {
                        boleto.Taxas.Add(new InscricaoBoletoTaxa
                        {
                            SeqTaxa = taxa.SeqTaxa,
                            SeqInscricaoBoleto = boleto.Seq,
                            NumeroItens = (short)taxa.NumeroItens,
                            SeqOferta = taxa.SeqOferta
                        });
                    }
                }
            }

            var taxasSomenteBanco = boleto.Taxas.Where(p => !inscricaoTaxa.Any(o => o.SeqTaxa == p.SeqTaxa && o.SeqOferta == p.SeqOferta)).ToList();

            // Remove as taxas que estão no banco e não estão na tela
            if (taxasSomenteBanco.Any())
            {
                foreach (var taxa in taxasSomenteBanco)
                {
                    boleto.Taxas.Remove(taxa);
                }
            }

            // Salva o boleto
            this.SaveEntity(boleto);
        }

        /// <summary>
        /// Busca o boleto para uma determinada inscrição
        /// </summary>
        public BoletoData GerarBoletoInscricao(long seqInscricao)
        {
            // Recuperar o seqInscricaoBoleto para o boleto de taxa de inscrição
            var spec = new InscricaoBoletoFilterSpecification { SeqInscricao = seqInscricao, TipoBoleto = TipoBoleto.Inscricao };
            var seqInscricaoBoleto = this.SearchProjectionBySpecification(spec, x => x.Seq).FirstOrDefault();
            return InscricaoBoletoTituloDomainService.GerarOuRecuperarTitulo(seqInscricaoBoleto);
        }
    }
}