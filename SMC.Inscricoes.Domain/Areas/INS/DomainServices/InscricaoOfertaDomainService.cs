using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Security;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoOferta;
using SMC.Inscricoes.Common.Areas.SEL.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects.AcompanhamentoCheckin;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using SMC.Inscricoes.Domain.Areas.SEL.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.AcompanhamentoCheckin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Data;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoOfertaDomainService : InscricaoContextDomain<InscricaoOferta>
    {
        #region DomainService

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private InscricaoBoletoDomainService InscricaoBoletoDomainService
        {
            get { return this.Create<InscricaoBoletoDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return Create<EtapaProcessoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }

        private InscricaoHistoricoSituacao InscricaoHistoricoSituacao
        {
            get { return Create<InscricaoHistoricoSituacao>(); }
        }

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return Create<TipoProcessoSituacaoDomainService>(); }
        }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService
        {
            get { return Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }
        }

        public InscritoDomainService InscritoDomainService
        {
            get { return Create<InscritoDomainService>(); }
        }

        private TipoProcessoDomainService TipoProcessoDomainService => Create<TipoProcessoDomainService>();

        private InscricaoDadoFormularioCampoDomainService InscricaoDadoFormularioCampoDomainService => Create<InscricaoDadoFormularioCampoDomainService>();
        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService => Create<HierarquiaOfertaDomainService>();

        #endregion DomainService

        #region Services

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        private ITemplateProcessoService TemplateProcessoService
        {
            get { return Create<ITemplateProcessoService>(); }
        }

        private IIntegracaoAcademicoService IntegracaoAcademicoService
        {
            get { return Create<IIntegracaoAcademicoService>(); }
        }

        #endregion Services

        /// <summary>
        /// Salva a lista de ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        public void SalvarListaInscricaoOferta(List<InscricaoOferta> ofertas, short? numeroOpcoesDesejadas, List<InscricaoTaxaOfertaVO> inscricaoTaxasOferta = null)
        {
            // Verifica se todas as InscricaoOferta recebidas como parametro são de uma mesma inscrição
            if (ofertas.Select(o => o.SeqInscricao).Distinct().Count() > 1)
                throw new InscricaoInvalidaException();

            // Busca o sequencial da inscrição
            long seqInscricao = ofertas.Select(o => o.SeqInscricao).Distinct().SingleOrDefault();

            // Busca a inscrição
            IncludesInscricao includes = IncludesInscricao.Ofertas |
                                         IncludesInscricao.ConfiguracaoEtapa |
                                         IncludesInscricao.Processo |
                                         IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso |
                                         IncludesInscricao.Boletos |
                                         IncludesInscricao.Processo_TipoProcesso;

            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(spec, includes);

            //busca dados inscrito
            var inscrito = InscritoDomainService.BuscarInscrito(inscricao.SeqInscrito);

            // Verifica as regras para avançar na inscrição
            InscricaoDomainService.VerificarRegrasAvancarInscricao(inscricao);

            // Verifica se o usuário preencheu corretamente as opções para qual deseja ser convocado
            if (inscricao.ConfiguracaoEtapa.NumeroMaximoOfertaPorInscricao.HasValue && inscricao.ConfiguracaoEtapa.NumeroMaximoOfertaPorInscricao.Value > 1)
            {
                if (inscricao.ConfiguracaoEtapa.NumeroMaximoConvocacaoPorInscricao.HasValue)
                {
                    if (!numeroOpcoesDesejadas.HasValue)
                    {
                        throw new InscricaoSemOpcoesDesejadasException();
                    }

                    if (numeroOpcoesDesejadas.Value > ofertas.Count)
                    {
                        throw new InscricaoComMaisOpcoesDesejadasQueOfertasException();
                    }
                }
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    ValidaListaOfertas(inscricao.Seq, ofertas);

                    var seqPrimeiraOpcaoOferta = ofertas.FirstOrDefault(x => x.NumeroOpcao == 1).SeqOferta;
                    // Percorre as ofertas encontradas no banco atualizando as que foram recebidas como parâmetro
                    // e excluindo as que estão apenas no banco
                    var exigePagamentoTaxa = this.OfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Oferta>(seqPrimeiraOpcaoOferta), x => x.ExigePagamentoTaxa);
                    if (exigePagamentoTaxa && (inscricaoTaxasOferta == null || inscricaoTaxasOferta.Count == 0))
                        throw new InscricaoSemTaxaException();

                    if (inscricaoTaxasOferta != null)
                    {
                        // Valida se teve alteração de taxas
                        var taxasValidar = inscricaoTaxasOferta.Select(x => new InscricaoTaxaVO()
                        {
                            SeqTaxa = x.SeqTaxa,
                            NumeroItens = x.NumeroItens.GetValueOrDefault(),
                            ValorItem = x.ValorEventoTaxa
                        }).ToList();
                        var teveAlteracaoTaxas = InscricaoDomainService.VerificaBoletoInscricaoAlteracaoTaxa(inscricao.Seq, taxasValidar);

                        InscricaoBoletoDomainService.SalvarInscricaoTaxasOferta(seqInscricao, seqPrimeiraOpcaoOferta, inscricaoTaxasOferta, (short)ofertas.Count, teveAlteracaoTaxas);
                    }
                    else
                    {
                        foreach (var boleto in inscricao.Boletos)
                        {
                            InscricaoBoletoDomainService.DeleteEntity(boleto);
                        }
                    }

                    SincronizarOfertasTelaComOfertasBanco(ofertas, seqInscricao);

                    // Armazena o tipo do processo
                    var tipoProcesso = inscricao.Processo.TipoProcesso;

                    // Persiste alterações no numero de opções desejadas da inscrição.
                    if (numeroOpcoesDesejadas.HasValue)
                    {
                        InscricaoDomainService.UpdateFields(new Inscricao() { Seq = inscricao.Seq, NumeroOpcoesDesejadas = numeroOpcoesDesejadas }, i => i.NumeroOpcoesDesejadas);
                    }

                    // Verifica as consistencias.
                    if (TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, TipoConsistencia.VinculoAcademico))
                    {
                        VerificaVinculoAcademico(ofertas, inscricao, tipoProcesso.Seq, unitOfWork);
                    }


                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Salva a lista de ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        public void SalvarListaInscricaoOfertaAngular(List<InscricaoOferta> ofertas, short? numeroOpcoesDesejadas,long seqGrupoOferta, List<InscricaoTaxaOfertaVO> inscricaoTaxasOferta = null)
        {
            // Verifica se todas as InscricaoOferta recebidas como parametro são de uma mesma inscrição
            if (ofertas.Select(o => o.SeqInscricao).Distinct().Count() > 1)
                throw new InscricaoInvalidaException();

            // Busca o sequencial da inscrição
            long seqInscricao = ofertas.Select(o => o.SeqInscricao).Distinct().SingleOrDefault();

            // Busca a inscrição
            IncludesInscricao includes = IncludesInscricao.Ofertas |
                                         IncludesInscricao.ConfiguracaoEtapa |
                                         IncludesInscricao.Processo |
                                         IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso |
                                         IncludesInscricao.Boletos |
                                         IncludesInscricao.Processo_TipoProcesso;

            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(spec, includes);

            //busca dados inscrito
            var inscrito = InscritoDomainService.BuscarInscrito(inscricao.SeqInscrito);

            // Verifica as regras para avançar na inscrição
            InscricaoDomainService.VerificarRegrasAvancarInscricao(inscricao);

            // Verifica se o usuário preencheu corretamente as opções para qual deseja ser convocado
            if (inscricao.ConfiguracaoEtapa.NumeroMaximoOfertaPorInscricao.HasValue && inscricao.ConfiguracaoEtapa.NumeroMaximoOfertaPorInscricao.Value > 1)
            {
                if (inscricao.ConfiguracaoEtapa.NumeroMaximoConvocacaoPorInscricao.HasValue)
                {
                    if (!numeroOpcoesDesejadas.HasValue)
                    {
                        throw new InscricaoSemOpcoesDesejadasException();
                    }

                    if (numeroOpcoesDesejadas.Value > ofertas.Count)
                    {
                        throw new InscricaoComMaisOpcoesDesejadasQueOfertasException();
                    }
                }
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    ValidaListaOfertas(inscricao.Seq, ofertas);

                    // Percorre as ofertas encontradas no banco atualizando as que foram recebidas como parâmetro
                    // e excluindo as que estão apenas no banco
                    var seqPrimeiraOpcaoOferta = ofertas.FirstOrDefault(x => x.NumeroOpcao == 1).SeqOferta;
                    var exigePagamentoTaxa = this.OfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Oferta>(seqPrimeiraOpcaoOferta), x => x.ExigePagamentoTaxa);
                    if (exigePagamentoTaxa && (inscricaoTaxasOferta == null || inscricaoTaxasOferta.Count == 0 || !inscricaoTaxasOferta.Any(a=> a.NumeroItens > 0)))
                        throw new InscricaoSemTaxaException();

                     if (inscricaoTaxasOferta != null)
                    {
                        // Valida se teve alteração de taxas
                        var taxasValidar = inscricaoTaxasOferta.Select(x => new InscricaoTaxaVO()
                        {
                            SeqTaxa = x.SeqTaxa,
                            NumeroItens = x.NumeroItens.GetValueOrDefault(),
                            ValorItem = x.ValorEventoTaxa,
                            TipoCobranca = x.TipoCobranca,
                            SeqOferta = x.SeqOferta ?? 0
                        }).ToList();

                        var teveAlteracaoTaxas = InscricaoDomainService.VerificaBoletoInscricaoAlteracaoTaxa(inscricao.Seq, taxasValidar);


                        InscricaoBoletoDomainService.SalvarInscricaoTaxasOfertaAngular(seqInscricao, ofertas, inscricaoTaxasOferta, (short)ofertas.Count, teveAlteracaoTaxas, seqGrupoOferta);

                    }
                    else
                    {
                        foreach (var boleto in inscricao.Boletos)
                        {
                            InscricaoBoletoDomainService.DeleteEntity(boleto);
                        }
                    }


                    SincronizarOfertasTelaComOfertasBanco(ofertas, seqInscricao);

                    // Armazena o tipo do processo
                    var tipoProcesso = inscricao.Processo.TipoProcesso;

                    // Persiste alterações no numero de opções desejadas da inscrição.
                    if (numeroOpcoesDesejadas.HasValue)
                    {
                        InscricaoDomainService.UpdateFields(new Inscricao() { Seq = inscricao.Seq, NumeroOpcoesDesejadas = numeroOpcoesDesejadas }, i => i.NumeroOpcoesDesejadas);
                    }

                    // Verifica as consistencias.
                    if (TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, TipoConsistencia.VinculoAcademico))
                    {
                        VerificaVinculoAcademico(ofertas, inscricao, tipoProcesso.Seq, unitOfWork);
                    }


                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }
        public void ValidaListaOfertas(long seqInscricao, List<InscricaoOferta> ofertas = null)
        {
            // Caso ja tenha cadastrado uma lista de ofertas na seção de ofertas e esteja revalidando na etapa de finalização da inscrição

            // Busca a inscrição
            var includes = IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta;
            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(spec, includes);



            if (ofertas == null)
            {
                ofertas = inscricao.Ofertas.ToList();
            }
            else
            {

                var ofertasOld = inscricao.Ofertas.ToList();

                if (ofertasOld.Any(a => a.DataCheckin != null))
                {
                    foreach (var item in ofertasOld)
                    {
                        if (item.DataCheckin.HasValue)
                        {
                            if (!ofertas.Any(a => a.SeqOferta == item.SeqOferta))
                            {
                                var of = OfertaDomainService.BuscarHierarquiaOfertaCompleta(item.SeqOferta);
                                throw new CheckinRealizadoException(of.DescricaoCompleta);
                            }
                        }
                    }
                }
            }

            // Verifica se o processo possui a regra de calculo de bolsa social.
            var processo = InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
            x => new
            {
                x.SeqProcesso,
                gestaoEvento = x.Processo.TipoProcesso.GestaoEventos,
                Idioma = x.Processo.Idiomas.Where(w => w.SeqProcesso == x.SeqProcesso).ToList(),
                VerificaCoincidenciaHorario = x.Processo.VerificaCoincidenciaHorario,
                ExigeJustificativa = x.ConfiguracaoEtapa.ExigeJustificativaOferta
            });

            if (processo.ExigeJustificativa && ofertas.Any(a => string.IsNullOrEmpty(a.JustificativaInscricao)))
            {
                throw new ExigeJustificativaException();
            }

            var inscricaoForaPrazo = InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(processo.SeqProcesso);

            var existeOfertaImpedida = false;

            existeOfertaImpedida = inscricao.Ofertas.Any(ofertaInscrita =>
                                (ofertaInscrita.Oferta.DataCancelamento.HasValue || // Oferta cancelada
                                    !ofertaInscrita.Oferta.Ativo || // Oferta inativa
                                    (!inscricaoForaPrazo && (DateTime.Now < ofertaInscrita.Oferta.DataInicio || DateTime.Now > ofertaInscrita.Oferta.DataFim))) // Oferta fora do período de validade
                                && ofertas.Any(ofertaParametro => ofertaParametro.SeqOferta == ofertaInscrita.SeqOferta) // E a oferta inscrita está na lista de ofertas do parâmetro
                                );

            if (existeOfertaImpedida)
            {
                throw new OfertaImpedidaException();
            }

            //verifica se o processo permite gestão de evento
            if (processo.gestaoEvento && processo.VerificaCoincidenciaHorario.HasValue && processo.VerificaCoincidenciaHorario.Value)
            {

                if (ofertas.Count() > 0)
                {
                    var inscricoesOfertasInscrito = this.BuscarOfertasInscrito(new InscricaoOfertaFilterSpecification() { SeqInscrito = inscricao.SeqInscrito }).Where(w => w.Oferta.SeqProcesso == processo.SeqProcesso).ToList();

                    var listaOfertas = OfertaDomainService.BuscarListaHierarquiaOfertaCompleta(ofertas.Select(s => s.SeqOferta).ToList()).Select(s => new { s.DataInicioAtividade, s.DataFimAtividade, s.Seq }).ToList();

                    inscricoesOfertasInscrito = inscricoesOfertasInscrito.Where(w => w.SeqInscricao != seqInscricao).ToList();


                    foreach (var item in ofertas)
                    {
                        bool coincideOfertaSelecionada = false;
                        bool coincideOfertaOutraInscricao = false;

                        var ofertacompleta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(item.SeqOferta, false);

                        if (item.Seq != 0)
                        {
                            inscricoesOfertasInscrito = inscricoesOfertasInscrito.Where(w => w.Seq != item.Seq).ToList();
                        }

                        // valida coincidencia de horário com outras inscrições
                        coincideOfertaOutraInscricao = inscricoesOfertasInscrito.Any(a =>
                        {
                            return (VerificaCoincidencia(ofertacompleta.DataInicioAtividade.Value, ofertacompleta.DataFimAtividade.Value, a.Oferta.DataInicioAtividade.Value, a.Oferta.DataFimAtividade.Value) &&
                                    VerificaCoincidencia(a.Oferta.DataInicioAtividade.Value, a.Oferta.DataFimAtividade.Value, ofertacompleta.DataInicioAtividade.Value, ofertacompleta.DataFimAtividade.Value));

                        });

                        // valida coincidencia de horário entre as ofertas selecionadas
                        coincideOfertaSelecionada = listaOfertas.Any(a => item.SeqOferta != a.Seq && ofertacompleta.DataInicioAtividade >= a.DataInicioAtividade && ofertacompleta.DataInicioAtividade <= a.DataFimAtividade || ofertacompleta.DataFimAtividade <= a.DataInicioAtividade && ofertacompleta.DataFimAtividade >= a.DataFimAtividade);

                        if (coincideOfertaSelecionada || coincideOfertaOutraInscricao)
                        {
                            var labelOferta = "oferta";

                            if (processo.Idioma != null && processo.Idioma.Any())
                            {
                                labelOferta = processo.Idioma.LastOrDefault().LabelOferta ?? "oferta";
                            }

                            throw new HorarioEDataCoicidenteException(labelOferta.ToLower());
                        }

                    }
                }
            }
        }

        private void SincronizarOfertasTelaComOfertasBanco(List<InscricaoOferta> ofertasTela, long seqInscricao)
        {
            var spec = new InscricaoOfertaFilterSpecification() { SeqInscricao = seqInscricao };
            var ofertasInscricaoInDB = this.SearchBySpecification(spec, p => p.Oferta).ToList();

            var ofertasSomenteBanco = ofertasInscricaoInDB.Where(p => !ofertasTela.Any(o => o.SeqOferta == p.SeqOferta)).ToList();
            var ofertasSomenteTela = ofertasTela.Where(o => !ofertasInscricaoInDB.Any(p => p.SeqOferta == o.SeqOferta)).ToList();
            var ofertasParaAtualizacao = ofertasInscricaoInDB.Where(p => ofertasTela.Any(o => o.SeqOferta == p.SeqOferta)).ToList();

            // Remove as ofertas que estão no banco e não estão na tela
            if (ofertasSomenteBanco.Any())
            {
                foreach (var oferta in ofertasSomenteBanco)
                {
                    this.DeleteEntity(oferta);
                }
            }

            // Adiciona as ofertas que estão na tela e não estão no banco
            if (ofertasSomenteTela.Any())
            {
                foreach (var oferta in ofertasSomenteTela)
                {
                    oferta.SeqOfertaOriginal = oferta.SeqOferta;
                    oferta.UidInscricaoOferta = Guid.NewGuid();
                    this.InsertEntity(oferta);
                }
            }

            // Atualiza as ofertas que estão na tela e no banco
            if (ofertasParaAtualizacao.Any())
            {
                foreach (var oferta in ofertasParaAtualizacao)
                {
                    var ofertaInDB = ofertasInscricaoInDB.FirstOrDefault(p => p.SeqOferta == oferta.SeqOferta);
                    ofertaInDB.NumeroOpcao = ofertasTela.FirstOrDefault(p => p.SeqOferta == oferta.SeqOferta).NumeroOpcao;
                    ofertaInDB.JustificativaInscricao = ofertasTela.FirstOrDefault(p => p.SeqOferta == oferta.SeqOferta).JustificativaInscricao;
                    this.UpdateFields(ofertaInDB, p => p.NumeroOpcao, p => p.JustificativaInscricao);
                }
            }
        }

        private void VerificaVinculoAcademico(List<InscricaoOferta> ofertas, Inscricao inscricao, long seqTipoProcesso, ISMCUnitOfWork unitOfWork)
        {
            string inscritoCpf = InscritoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito),
                                                                x => x.Cpf);
            var cod_origem = OfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Oferta>(ofertas.First().SeqOferta),
                                                    x => x.CodigoOrigem);

            // Consulta o SGA.
            var mensagemValidacaoMatricula = IntegracaoAcademicoService.VerificarAlunoMatriculado(new AlunoMatriculadoData()
            {
                Cpf = inscritoCpf,
                Ano = inscricao.Processo.AnoReferencia,
                Semestre = inscricao.Processo.SemestreReferencia,
                CodOrigem = cod_origem,
                SeqTipoProcesso = seqTipoProcesso
            }, out bool error);
            if (!string.IsNullOrWhiteSpace(mensagemValidacaoMatricula))
            {
                InscricaoDomainService.CancelarInscricao(inscricao.Seq, inscricao.SeqProcesso, TOKENS.MOTIVO_INSCRICAO_CANCELADA_VINCULO_ACADEMICO);
                if (!error)
                {
                    // Se não houve erros, grava as informações e interrompe a inscrição.
                    unitOfWork.Commit();
                }
                // Interrompe a operação.
                throw new Exception(mensagemValidacaoMatricula);
            }
        }

        public List<ConsultaCandidatosProcessoVO> BuscarCandidatosSelecaoProcesso
                (CandidatosSelecaoProcessoSpecification filtro, out int total)
        {
            var numeroPagina = filtro.PageNumber;
            var tamanhoPagina = filtro.MaxResults;
            filtro.PageNumber = 1;
            filtro.MaxResults = Int32.MaxValue;

            var resultado = this.SearchProjectionBySpecification(filtro,
                                x => new ConsultaCandidatosProcessoVO
                                {
                                    SeqInscricaoOferta = x.Seq,
                                    SeqInscricao = x.SeqInscricao,
                                    SeqInscrito = x.Inscricao.SeqInscrito,
                                    SeqProcesso = x.Inscricao.SeqProcesso,
                                    SeqGrupoOferta = x.Oferta.SeqGrupoOferta.Value,
                                    SeqOferta = x.SeqOferta,
                                    SeqOfertaOriginal = x.SeqOfertaOriginal,
                                    SeqMotivoSituacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).SeqMotivoSituacao,
                                    Candidato = (x.Inscricao.Inscrito.NomeSocial != null) ?
                                                    x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" :
                                                    x.Inscricao.Inscrito.Nome,
                                    NumeroIdentidade = x.Inscricao.Inscrito.NumeroIdentidade,
                                    Cpf = x.Inscricao.Inscrito.Cpf,
                                    DataNascimento = x.Inscricao.Inscrito.DataNascimento,
                                    Opcao = x.NumeroOpcao + "ª",
                                    Oferta = x.Oferta.DescricaoCompleta,
                                    OfertaVo = new OfertaVO()
                                    {
                                        Nome = x.Oferta.Nome,
                                        DescricaoOferta = x.Oferta.DescricaoCompleta, //y.Oferta.Nome,                                        
                                        DataInicioAtividade = x.Oferta.DataInicioAtividade,
                                        DataFimAtividade = x.Oferta.DataFimAtividade,
                                        CargaHorariaAtividade = x.Oferta.CargaHorariaAtividade,
                                        ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta
                                    },
                                    PossuiJustificativa = !string.IsNullOrEmpty(x.JustificativaInscricao),
                                    SeqInscricaoHistoricoSituacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual &&
                                                                                            (f.SeqMotivoSituacao.HasValue || !string.IsNullOrEmpty(f.Justificativa))).Seq,
                                    Situacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                                    Nota = x.ValorNota,
                                    SegundaNota = x.ValorSegundaNota,
                                    Classificacao = x.NumeroClassificacao,
                                    // Utiliza a hierarquia para filtrar
                                    HierarquiaCompleta = x.Oferta.HierarquiaCompleta,
                                    Email = x.Inscricao.Inscrito.Email,
                                    Telefones = x.Inscricao.Inscrito.Telefones.Select(f => new TelefonePessoaIntegracaoVO
                                    {
                                        TipoTelefone = f.TipoTelefone,
                                        Numero = f.Numero,
                                        CodigoArea = f.CodigoArea,
                                        CodigoPais = f.CodigoPais
                                    }).ToList(),
                                    Enderecos = x.Inscricao.Inscrito.Enderecos.Select(f => new EnderecoPessoaIntegracaoVO
                                    {
                                        TipoEndereco = f.TipoEndereco,
                                        Logradouro = f.Logradouro,
                                        Numero = f.Numero,
                                        Complemento = f.Complemento,
                                        Bairro = f.Bairro,
                                        Cep = f.Cep,
                                        NomeCidade = f.NomeCidade,
                                        SiglaUf = f.Uf,
                                        CodigoPais = f.CodigoPais,
                                        Correspondencia = f.Correspondencia
                                    }).ToList(),
                                    DataInscricao = x.Inscricao.DataInscricao
                                }, out total).ToList();

            //preenche os dados da oferta completo se tiver a flag ind_exibir_periodo_atividade_oferta 
            foreach (var item in resultado)
            {
                item.Oferta = PreencheDadosCompletos(item.OfertaVo);
            }


            if (filtro.SeqItemHierarquiaOferta.HasValue)
            {
                var tmpList = new List<ConsultaCandidatosProcessoVO>();
                foreach (var item in resultado)
                {
                    var oferta = new Oferta() { HierarquiaCompleta = item.HierarquiaCompleta };
                    if (oferta.VerificarHierarquia(filtro.SeqItemHierarquiaOferta.Value))
                        tmpList.Add(item);
                }
                resultado = tmpList;
            }

            var seqsMotivoSituacao = resultado.Where(w => w.SeqMotivoSituacao.HasValue).Select(s => s.SeqMotivoSituacao.Value).Distinct().ToArray();
            if (seqsMotivoSituacao.Any())
            {
                var motivos = SituacaoService.BuscarDescricaoMotivos(seqsMotivoSituacao);
                foreach (var item in resultado)
                {
                    if (item.SeqMotivoSituacao.HasValue)
                        item.Motivo = motivos.First(f => f.Seq == item.SeqMotivoSituacao).Descricao;
                }
            }

            total = resultado.Count();
            return resultado.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        public List<LancamentoResultadoItemVO> BuscarInscricoesOferta(long seqProcesso, IEnumerable<long> inscricoesOfertas)
        {
            // Verifica se a etapa de seleção existe para o processo, se está vigente e liberada.
            var etapa = EtapaProcessoDomainService.SearchByKey(new EtapaProcessoFilterSpecification(seqProcesso) { Token = TOKENS.ETAPA_SELECAO });
            if (etapa == null || !etapa.Vigente || etapa.SituacaoEtapa != SituacaoEtapa.Liberada)
            {
                throw new EtapaSelecaoProcessoNaoExistenteException();
            }
            var spec = new SMCContainsSpecification<InscricaoOferta, long>(f => f.Seq, inscricoesOfertas.ToArray());

            var projection = this.SearchProjectionBySpecification(spec,
                                            x => new
                                            {
                                                SeqInscricaoOferta = x.Seq,
                                                x.SeqInscricao,
                                                Candidato = (x.Inscricao.Inscrito.NomeSocial != null) ? x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" : x.Inscricao.Inscrito.Nome,
                                                Opcao = x.NumeroOpcao + "ª",
                                                x.Inscricao.NumeroOpcoesDesejadas,
                                                Justificativa = x.JustificativaInscricao,
                                                Nota = x.ValorNota,
                                                SegundaNota = x.ValorSegundaNota,
                                                Classificacao = x.NumeroClassificacao,
                                                SituacaoInscrito = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,

                                                // Busca a situação atual da etapa de seleção que não seja candidato confirmado
                                                HistoricosSituacao = x.HistoricosSituacao.FirstOrDefault(f => f.AtualEtapa
                                                                                && f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_CONFIRMADO
                                                                                && f.EtapaProcesso.Token == TOKENS.ETAPA_SELECAO),
                                                DescricaoResultadoSelecao = x.HistoricosSituacao.FirstOrDefault(f => f.AtualEtapa
                                                                                && f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_CONFIRMADO
                                                                                && f.EtapaProcesso.Token == TOKENS.ETAPA_SELECAO).TipoProcessoSituacao.Descricao
                                            }).ToList();

            // Busca as situacoes do processo
            var seqTemplateProcesso = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                                            x => x.SeqTemplateProcessoSGF);
            var situacoes = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(seqTemplateProcesso);
            //Verifica se a situação dos candidatos é deferida se existir esta situação no template. Caso contrário, verifica se a situação é confirmada.
            if (situacoes.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA))
            {
                if (projection.Any(f => f.SituacaoInscrito != TOKENS.SITUACAO_INSCRICAO_DEFERIDA))
                {
                    throw new InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException("deferida");
                }
            }
            else
            {
                if (projection.Any(f => f.SituacaoInscrito != TOKENS.SITUACAO_INSCRICAO_CONFIRMADA))
                {
                    throw new InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException("confirmada");
                }
            }

            var retorno = new List<LancamentoResultadoItemVO>();
            foreach (var inscricaoOferta in inscricoesOfertas)
            {
                // Ordena o resultado, baseado na ordem que chegou as inscrições oferta.
                var item = projection.First(f => f.SeqInscricaoOferta == inscricaoOferta);

                var lancamento = SMCMapperHelper.Create<LancamentoResultadoItemVO>(item);

                lancamento.SeqTipoProcessoSituacao = item.HistoricosSituacao?.SeqTipoProcessoSituacao;
                lancamento.SeqResultadoSelecao = item.HistoricosSituacao?.SeqTipoProcessoSituacao;
                lancamento.SeqMotivo = item.HistoricosSituacao?.SeqMotivoSituacao;
                lancamento.ParecerResponsavel = item.HistoricosSituacao?.Justificativa;

                // Busca a descrição dos motivos no SGF, caso esteja na situação CANDIDADO_CONFIRMADO
                if (lancamento.SeqMotivo.HasValue && !string.IsNullOrWhiteSpace(item.DescricaoResultadoSelecao))
                {
                    var motivo = SituacaoService.BuscarMotivo(lancamento.SeqMotivo.Value);
                    if (motivo != null)
                    {
                        lancamento.Motivo = motivo.Descricao;
                    }
                }

                retorno.Add(lancamento);
            }
            return retorno;
        }

        public List<InscritoOfertaVO> BuscarInscricoesOfertaParaConvocacao(long seqProcesso, List<long> seqsInscricaoOferta)
        {
            var etapaTokenSpec = new EtapaProcessoFilterSpecification(seqProcesso) { Token = TOKENS.ETAPA_CONVOCACAO, SituacaoEtapa = SituacaoEtapa.Liberada };
            var etapaVigente = new EtapaProcessoVigenteSpecification();
            // Verifica se a etapa de convocação existe para o processo.
            if (EtapaProcessoDomainService.Count(etapaTokenSpec & etapaVigente) == 0)
            {
                throw new EtapaConvocacaoProcessoNaoExistenteException();
            }

            var spec = new SMCContainsSpecification<InscricaoOferta, long>(f => f.Seq, seqsInscricaoOferta.ToArray());
            var candidatos = SearchProjectionBySpecification(spec,
                                    x => new InscritoOfertaVO
                                    {
                                        SeqInscricaoOferta = x.Seq,
                                        SeqInscricao = x.SeqInscricao,
                                        Nome = (x.Inscricao.Inscrito.NomeSocial != null) ? x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" : x.Inscricao.Inscrito.Nome,
                                        Situacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                                        TokenSituacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                                        TokenEtapa = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).EtapaProcesso.Token,
                                        Exportado = x.Exportado,
                                        IntegraSGALegado = x.Inscricao.Processo.TipoProcesso.IntegraSGALegado
                                    }).ToList();

            // Verifica se existe mais de uma situação entre os candidatos
            if (candidatos.GroupBy(g => g.TokenSituacao).Count() > 1)
            {
                throw new MultiplasSituacoesParaConvocacaoException();
            }

            // Verifica se algum candidato não está na situação de EXCEDENTE OU SELECIONADO
            if (candidatos.Any(f => f.TokenSituacao != TOKENS.SITUACAO_CANDIDATO_EXCEDENTE
                                 && f.TokenSituacao != TOKENS.SITUACAO_CANDIDATO_SELECIONADO
                                 && f.TokenSituacao != TOKENS.SITUACAO_CONVOCADO
                                 && f.TokenSituacao != TOKENS.SITUACAO_CONVOCADO_CONFIRMADO
                                 && f.TokenSituacao != TOKENS.SITUACAO_CONVOCADO_DESISTENTE
                        )
                    )
            {
                throw new CandidatoComSituacaoInvalidaParaConvocacaoException();
            }

            // Se o tipo de processo do processo em questão não estiver configurado para integrar como SGA Legado,
            // houver candidatos com situação atual "Candidato Convocado" e ind_exportado igual a "sim",
            // abortar a operação e emitir a mensagem de erro:
            // "A convocação não pode ser alterada para candidatos que já foram exportados"
            if (candidatos.Any(f => !f.IntegraSGALegado && f.TokenSituacao == TOKENS.SITUACAO_CONVOCADO && f.Exportado.GetValueOrDefault()))
            {
                throw new ConvocadoExportadoException();
            }

            var retorno = new List<InscritoOfertaVO>();
            // Ordena os candidatos pela ordem q os seq chegaram
            foreach (var seqInscricaoOferta in seqsInscricaoOferta)
            {
                retorno.Add(candidatos.First(f => f.SeqInscricaoOferta == seqInscricaoOferta));
            }

            return retorno;
        }

        public bool VerificaDisponibilidadeVagas(long seqOferta, List<LancamentoResultadoItemVO> list)
        {
            // Busca as informações da oferta.
            var oferta = OfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Oferta>(seqOferta),
                            x => new
                            {
                                NumeroVagas = x.NumeroVagas,
                                SeqTipoProcesso = x.Processo.SeqTipoProcesso
                            });

            var seqSituacaoCandidatoSelecionado = BuscarSeqTipoProcessoSituacaoCandidatoSelecionado(oferta.SeqTipoProcesso);

            // Conta quantos inscritos estão sendo modificados para se tornarem candidados selecionados
            var seqsInscritosSelecionados = list.Where(c => c.SeqResultadoSelecao.HasValue && c.SeqResultadoSelecao == seqSituacaoCandidatoSelecionado).Select(x => x.SeqInscricaoOferta);

            // Se os  candidatos não estiverem com a SITUACAO_CANDIDATO_SELECIONADO, não é necessário validar a disponibilidade de vagas
            if (!seqsInscritosSelecionados.SMCAny()) { return true; }

            // Busca a quantidade de inscritos que já estão ocupando alguma vaga
            int qtdVagasOcupadas = BuscarVagasOcupadasInscricaoOferta(seqOferta, seqsInscritosSelecionados);

            return seqsInscritosSelecionados.Count() + qtdVagasOcupadas <= oferta.NumeroVagas;
        }

        /// <summary>
        /// Busca a quantidade de inscritos que já estão ocupando alguma vaga
        /// Conforme os tokens
        /// SITUACAO_CANDIDATO_SELECIONADO
        /// SITUACAO_CONVOCADO
        /// SITUACAO_CONVOCADO_CONFIRMADO
        /// E desconsiderando os inscritos, já selecionados, passados por parâmetro
        /// </summary>
        /// <param name="seqOferta"></param>
        /// <param name="seqsInscritosSelecionados"></param>
        /// <returns></returns>
        private int BuscarVagasOcupadasInscricaoOferta(long seqOferta, IEnumerable<long> seqsInscritosSelecionados)
        {
            var spec = new InscricaoOfertaHistoricoPorTokensSpecification()
            {
                SeqOferta = seqOferta,
                Tokens = new string[]
                       {
                            TOKENS.SITUACAO_CANDIDATO_SELECIONADO,
                            TOKENS.SITUACAO_CONVOCADO,
                            TOKENS.SITUACAO_CONVOCADO_CONFIRMADO
                       }
            };

            return InscricaoOfertaHistoricoSituacaoDomainService.SearchProjectionBySpecification(spec,
                  x => x.SeqInscricaoOferta
                  )
                    .Where(
                          s => !seqsInscritosSelecionados.Contains(s)
              ).Count();
        }

        private long BuscarSeqTipoProcessoSituacaoCandidatoSelecionado(long seqTipoProcesso)
        {
            // Busca o sequencial da situação SITUACAO_CANDIDATO_SELECIONADO
            var seqTipoProcessoSituacaoCandidatoSelecionado = TipoProcessoSituacaoDomainService.SearchProjectionByKey(
                                                    new TipoProcessoSituacaoFilterSpecification()
                                                    {
                                                        SeqTipoProcesso = seqTipoProcesso,
                                                        Token = TOKENS.SITUACAO_CANDIDATO_SELECIONADO
                                                    },
                                                    x => x.Seq);

            return seqTipoProcessoSituacaoCandidatoSelecionado;
        }

        public void SalvarLancamentos(List<LancamentoResultadoItemVO> lancamentos)
        {
            if (lancamentos.Count == 0)
                throw new SMCArgumentException("lancamentos");

            VerificaClassificacaoDuplicada(lancamentos);

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var item in lancamentos)
                    {
                        var inscricaoOferta = SearchByKey(new SMCSeqSpecification<InscricaoOferta>(item.SeqInscricaoOferta),
                                                                    IncludesInscricaoOferta.HistoricosSituacao_TipoProcessoSituacao);

                        var oferta = OfertaDomainService.BuscarOfertaPorSeq(inscricaoOferta.SeqOferta);

                        var situacaoAtual = inscricaoOferta.HistoricosSituacao.First(f => f.Atual);

                        // Se a consistência "Cálculos da bolsa social" estiver configurada para o tipo de processo em questão,
                        // verificar se o somatório dos campos val_nota e val_segunda_nota é maior que o valor calculado
                        // para o campo do formulário de inscrição cujo token é PERCENTUAL_MAXIMO_BOLSA
                        var tipoProcesso = TipoProcessoDomainService.SearchByKey(new SMCSeqSpecification<TipoProcesso>(situacaoAtual.TipoProcessoSituacao.SeqTipoProcesso),
                                                                        x => x.Consistencias);
                        var possuiBolsaSocial = TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, TipoConsistencia.CalculoBolsaSocial);
                        if (possuiBolsaSocial)
                        {
                            VerificarLimiteBolsaSocial(inscricaoOferta.SeqInscricao, item.Nota.GetValueOrDefault(), item.SegundaNota.GetValueOrDefault());
                        }
                        if (tipoProcesso.ValidaLimiteDesconto)
                        {
                            if (item.Nota > oferta.LimitePercentualDesconto)
                                throw new BolsaNaoPodeUltrapassarDescontoException();
                        }

                        // TSK 54684
                        // Se o Resultado for "Candidato Selecionado" e a consistência "Cálculos da bolsa social" estiver configurada para
                        // o tipo de processo em questão, ou o tipo de processo do processo em questão estiver configurado para validar o
                        // limite de desconto, verificar se o % Bolsa informado é igual a zero. Se for, abortar a operação e emitir a
                        // mensagem de erro:  "O % Bolsa não pode ser 0 (zero).".
                        var seqSituacaoCandidatoSelecionado = BuscarSeqTipoProcessoSituacaoCandidatoSelecionado(tipoProcesso.Seq);
                        if (item.SeqResultadoSelecao.Value == seqSituacaoCandidatoSelecionado &&
                            (possuiBolsaSocial || tipoProcesso.ValidaLimiteDesconto) &&
                            item.Nota.GetValueOrDefault() == 0)
                        {
                            throw new PercentualBolsaNaoPodeSerZeroException();
                        }

                        if (situacaoAtual.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_CONFIRMADO)
                        {
                            // Se por algum motivo não existir um resultado de seleção quando estiver alterando a situação, retorna um erro.
                            if (!item.SeqResultadoSelecao.HasValue)
                            {
                                throw new SMCArgumentException("SeqResultadoSelecao");
                            }

                            // Verifica se os campos motivo e justificativa foram preenchidos corretamente.
                            VerificaPreenchimentoMotivoJustificativa(item);

                            // Modifica a situação atual
                            situacaoAtual.Atual = false;
                            situacaoAtual.AtualEtapa = false;
                            InscricaoOfertaHistoricoSituacaoDomainService.SaveEntity(situacaoAtual.SMCClone());

                            // Cria a nova situação
                            inscricaoOferta.HistoricosSituacao.Add(new InscricaoOfertaHistoricoSituacao()
                            {
                                Atual = true,
                                AtualEtapa = true,
                                DataSituacao = DateTime.Now,
                                SeqEtapaProcesso = situacaoAtual.SeqEtapaProcesso,
                                SeqInscricaoOferta = item.SeqInscricaoOferta,

                                SeqTipoProcessoSituacao = item.SeqResultadoSelecao.Value,
                                SeqMotivoSituacao = item.SeqMotivo,
                                Justificativa = item.ParecerResponsavel
                            });
                        }

                        inscricaoOferta.ValorNota = item.Nota;
                        inscricaoOferta.ValorSegundaNota = item.SegundaNota;
                        inscricaoOferta.NumeroClassificacao = item.Classificacao;
                        SaveEntity(inscricaoOferta);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Verifica se existem classificações duplicadas 
        /// </summary>
        /// <param name="lancamentos">lançamentos para atualização</param>
        private void VerificaClassificacaoDuplicada(List<LancamentoResultadoItemVO> lancamentos)
        {
            // Busca a oferta
            var specOferta = new SMCSeqSpecification<InscricaoOferta>(lancamentos[0].SeqInscricaoOferta);
            var seqHierarquiaOferta = SearchProjectionByKey(specOferta, x => x.SeqOferta);

            // Busca as classificações já lançadas nessa oferta em banco
            var specInscricaoOferta = new InscricaoOfertaFilterSpecification() { SeqOferta = seqHierarquiaOferta };
            var classificacoes = SearchProjectionBySpecification(specInscricaoOferta, x => new InscricaoOfertaVO
            {
                SeqInscricao = x.SeqInscricao,
                NumeroClassificacao = x.NumeroClassificacao
            }).Where(f => f.NumeroClassificacao.HasValue).ToList();

            // Caso seja atualização de uma classificação, ajusta para o novo valor
            foreach (var item in lancamentos.Where(l => classificacoes.Any(x => x.SeqInscricao == l.SeqInscricao)))
            {
                classificacoes.First(c => c.SeqInscricao == item.SeqInscricao).NumeroClassificacao = item.Classificacao;
            }

            // Adiciona os registros que ainda não estão em banco na lista de classificações
            classificacoes.AddRange(
                lancamentos.Where(f => f.Classificacao.HasValue && !classificacoes.Any(x => x.SeqInscricao == f.SeqInscricao)).Select(s => new InscricaoOfertaVO
                {
                    SeqInscricao = s.SeqInscricao,
                    NumeroClassificacao = s.Classificacao.Value
                }));

            // Verifica duplicidade
            var duplicadas = classificacoes.GroupBy(x => x.NumeroClassificacao).Where(f => f.Count() > 1);
            if (duplicadas.Any())
            {
                throw new LancamentoNotaClassificacaoDuplicadaException(duplicadas.First().First().NumeroClassificacao.Value);
            }
        }

        private void VerificarLimiteBolsaSocial(long seqInscricao, decimal valNota, decimal valSegundaNota)
        {
            // Task 36962 - Comentado e substituído a pedido da task
            //var spec = new InscricaoDadoFormularioCampoPorTokenSpecification(seqInscricao, TOKENS_BOLSA_SOCIAL.PERCENTUAL_MAXIMO_BOLSA);
            //var formularios = InscricaoDadoFormularioCampoDomainService.SearchBySpecification(spec, x => x.InscricaoDadoFormulario.ConfiguracaoEtapaPaginaIdioma);

            //// Procura pelo formulário de gestão. Se não existir, pegar o primeiro (inscrição).
            //var dadoFormulario = formularios.FirstOrDefault(f => f.InscricaoDadoFormulario.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF == f.InscricaoDadoFormulario.SeqVisao) ??
            //                        formularios.FirstOrDefault();

            //if (dadoFormulario != null && decimal.TryParse(dadoFormulario.Valor, out decimal percentualMaximoBolsa))
            //{
            //    var percentualTotal = valNota + valSegundaNota;
            //    if (percentualTotal > percentualMaximoBolsa)
            //    {
            //        if (percentualMaximoBolsa >= 50)
            //        {
            //            throw new LancamentoBolsaUltrapassaLimiteException();
            //        }
            //        else if (!(valNota == 0 && valSegundaNota <= 50))
            //        {
            //            throw new LancamentoBolsaUltrapassaLimiteComExcecaoException();
            //        }
            //    }
            //}

            var spec = new InscricaoDadoFormularioCampoPorTokenSpecification(seqInscricao, TOKENS_BOLSA_SOCIAL.PERCENTUAL_MAXIMO_BOLSA);
            var formularios = InscricaoDadoFormularioCampoDomainService.SearchBySpecification(spec, x => x.InscricaoDadoFormulario.ConfiguracaoEtapaPaginaIdioma);

            // Procura pelo formulário de gestão. Se não existir, pegar o primeiro (inscrição).
            var dadoFormulario = formularios.FirstOrDefault(f => f.InscricaoDadoFormulario.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF == f.InscricaoDadoFormulario.SeqVisao) ??
                                    formularios.FirstOrDefault();

            if (dadoFormulario != null && decimal.TryParse(dadoFormulario.Valor, out decimal percentualMaximoBolsa))
            {
                if (valNota > percentualMaximoBolsa)
                    throw new PercentualBolsaUltrapassaPercentualMaximoException();
            }
        }

        private void VerificaPreenchimentoMotivoJustificativa(LancamentoResultadoItemVO item)
        {
            var tipoProcessoSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(
                                        new SMCSeqSpecification<TipoProcessoSituacao>(item.SeqResultadoSelecao.Value),
                                            f => new
                                            {
                                                SeqSituacao = f.SeqSituacao
                                            });

            var requisitosSituacao = SituacaoService.VerificarRequisitosSituacao(tipoProcessoSituacaoDestino.SeqSituacao, item.SeqMotivo);

            if (requisitosSituacao.ExigeMotivo && item.SeqMotivo.GetValueOrDefault() == 0)
            {
                throw new SituacaoExigeMotivoException();
            }

            if (requisitosSituacao.ExigeJustificativa.GetValueOrDefault() && string.IsNullOrEmpty(item.ParecerResponsavel))
            {
                throw new SitucaoExigeJustificativaException();
            }
        }

        public void DesfazerLancamentoResultado(List<long> seqsInscricoesOferta)
        {
            // Dicionario das situações do SGF, para não precisar utilizar o serviço mais de uma vez no loop.
            var situacoesPermiteRetornarCache = new Dictionary<long, bool>();

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    //Verificar se a etapa de seleção está cadastrada, liberada e vigente
                    ValidarEtapaSelecaoValida(seqsInscricoesOferta);

                    var inscricoes = SearchBySpecification(new InscricaoOfertaFilterSpecification() { Seqs = seqsInscricoesOferta },
                                                            IncludesInscricaoOferta.HistoricosSituacao_TipoProcessoSituacao).ToList();

                    foreach (var seqInscricaoOferta in seqsInscricoesOferta)
                    {
                        var inscricaoOferta = inscricoes.FirstOrDefault(x => x.Seq == seqInscricaoOferta);

                        var situacaoAtual = inscricaoOferta.HistoricosSituacao.First(x => x.Atual);

                        if (situacaoAtual.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_DESISTENTE &&
                            situacaoAtual.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_EXCEDENTE &&
                            situacaoAtual.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_REPROVADO &&
                            situacaoAtual.TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_SELECIONADO)
                            throw new DesfazerLancamentoException(seqsInscricoesOferta.Count == 0);

                        if (!situacoesPermiteRetornarCache.ContainsKey(situacaoAtual.TipoProcessoSituacao.SeqSituacao))
                        {
                            //Consultar as situações de destino no SGF
                            var situacaoSgf = SituacaoService.BuscarSituacao(situacaoAtual.TipoProcessoSituacao.SeqSituacao);
                            situacoesPermiteRetornarCache.Add(situacaoAtual.TipoProcessoSituacao.SeqSituacao, situacaoSgf.PermiteRetornar);
                        }

                        ///Tratamento para a regra de retorno
                        if (!situacoesPermiteRetornarCache[situacaoAtual.TipoProcessoSituacao.SeqSituacao])
                        {
                            throw new SituacaoNaoPermiteDesfazerLancamentoException(situacaoAtual.TipoProcessoSituacao.Descricao);
                        }

                        situacaoAtual.Atual = false;
                        situacaoAtual.AtualEtapa = false;

                        // Busca o seq do TipoProcessoSituacao de alguma etapa confirmado.
                        var seqTipoProcessoSituacao = inscricaoOferta.HistoricosSituacao
                                                                     .First(f => f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_CONFIRMADO)
                                                                     .SeqTipoProcessoSituacao;

                        inscricaoOferta.HistoricosSituacao.Add(new InscricaoOfertaHistoricoSituacao()
                        {
                            Atual = true,
                            AtualEtapa = true,
                            DataSituacao = DateTime.Now,
                            SeqEtapaProcesso = situacaoAtual.SeqEtapaProcesso,
                            SeqInscricaoOferta = seqInscricaoOferta,

                            SeqTipoProcessoSituacao = seqTipoProcessoSituacao,
                        });
                        if (inscricaoOferta.ValorNota.HasValue)
                            inscricaoOferta.ValorNota = null;
                        if (inscricaoOferta.NumeroClassificacao.HasValue)
                            inscricaoOferta.NumeroClassificacao = null;

                        SaveEntity(inscricaoOferta);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Se não existir etapa de seleção cadastrada, liberada e vigente (data atual entre data início e fim da etapa de seleção),
        /// abortar a operação emitir a mensagem de erro:
        /// "Não existe etapa de seleção ativa para este processo. Não é possível desfazer o lançamento de resultado."
        /// </summary>
        /// <param name="seqsInscricoesOferta"></param>
        private void ValidarEtapaSelecaoValida(List<long> seqsInscricoesOferta)
        {
            var etapasSelecao = SearchProjectionBySpecification(new InscricaoOfertaFilterSpecification() { Seqs = seqsInscricoesOferta },
                    i => i.Inscricao.Processo.EtapasProcesso.Select(x => new EtapaProcessoInscricaoOfertaVO()
                    {
                        SeqInscricaoOferta = i.Seq,
                        DataInicioEtapa = x.DataInicioEtapa,
                        DataFimEtapa = x.DataFimEtapa,
                        EtapaProcessoToken = x.Token,
                        SituacaoEtapa = x.SituacaoEtapa
                    }).Where(t => t.EtapaProcessoToken == TOKENS.ETAPA_SELECAO &&
                                    t.SituacaoEtapa == SituacaoEtapa.Liberada &&
                                    t.DataInicioEtapa <= DateTime.Now && t.DataFimEtapa > DateTime.Now
                    ).ToList()).SelectMany(c => c.Select(f => f)).ToList();

            if (!etapasSelecao.SMCAny() || !etapasSelecao.TrueForAll(x => seqsInscricoesOferta.Contains(x.SeqInscricaoOferta.Value)))
            {
                throw new DesfazerLancamentoEtapaInvalidaException();
            }
        }

        public void AtualizarExportacaoInscricao(List<long> inscricaoOfertas)
        {
            var seqs = string.Join(",", inscricaoOfertas);

            ExecuteSqlCommandAsync(
                    $@"UPDATE
                            inscricao_oferta
                        SET
                            ind_exportado = 1,
                            dat_alteracao = GETDATE(),
                            usu_alteracao = 'JOB\GPI'
                        WHERE
                            seq_inscricao_oferta IN ({seqs})"
            ).Wait();
        }

        #region Alteração Oferta

        public OfertaCabecalhoVO BuscarOfertaCabecalho(InscricaoOfertaFilterSpecification filtro)
        {
            var resultado = this.SearchProjectionByKey(filtro,
                                x => new OfertaCabecalhoVO
                                {
                                    NumeroInscricao = x.SeqInscricao.ToString(),
                                    Candidato = (x.Inscricao.Inscrito.NomeSocial != null) ?
                                                    x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" :
                                                    x.Inscricao.Inscrito.Nome,
                                    Opcao = x.NumeroOpcao + "ª",
                                    Situacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,

                                    OfertaOriginal = x.OfertaOriginal.DescricaoCompleta,
                                    ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                                    DataInicioAtividade = x.OfertaOriginal.DataInicioAtividade,
                                    DataFimAtividade = x.OfertaOriginal.DataFimAtividade,
                                    CargaHorariaAtividade = x.OfertaOriginal.CargaHorariaAtividade,
                                    Nome = x.Oferta.Nome

                                });

            var of = new Oferta()
            {
                Nome = resultado.Nome,
                DataInicioAtividade = resultado.DataInicioAtividade,
                DescricaoCompleta = resultado.OfertaOriginal,
                DataFimAtividade = resultado.DataFimAtividade,
                CargaHorariaAtividade = resultado.CargaHorariaAtividade,
                Processo = new Processo()
                {
                    ExibirPeriodoAtividadeOferta = resultado.ExibirPeriodoAtividadeOferta
                }
            };

            OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, false);

            resultado.OfertaOriginal = of.DescricaoCompleta;

            return resultado;
        }

        public OfertaVO BuscarOferta(InscricaoOfertaFilterSpecification filtro)
        {
            var resultado = this.SearchProjectionByKey(filtro,
                                x => new OfertaVO
                                {
                                    SeqInscricaoOferta = x.Seq,
                                    SeqInscricao = x.SeqInscricao,
                                    SeqOferta = x.SeqOferta,
                                    SeqOfertaAtual = x.SeqOferta,
                                    Exportado = x.Exportado,
                                    SeqGrupoOferta = x.Oferta.SeqGrupoOferta,
                                    SeqOfertaOriginal = x.SeqOfertaOriginal,
                                    JustificativaAlteracaoOferta = x.JustificativaAlteracaoOferta,
                                    NomeInscrito = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                                    DescricaoOferta = x.Oferta.DescricaoCompleta,
                                    ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                                    DataInicioAtividade = x.Oferta.DataInicioAtividade,
                                    DataFimAtividade = x.Oferta.DataFimAtividade,
                                    CargaHorariaAtividade = x.Oferta.CargaHorariaAtividade,
                                    Nome = x.Oferta.Nome

                                });

            var of = new Oferta()
            {
                Nome = resultado.Nome,
                DescricaoCompleta = resultado.DescricaoOferta,
                DataInicioAtividade = resultado.DataInicioAtividade,
                DataFimAtividade = resultado.DataFimAtividade,
                CargaHorariaAtividade = resultado.CargaHorariaAtividade,
                Processo = new Processo()
                {
                    ExibirPeriodoAtividadeOferta = resultado.ExibirPeriodoAtividadeOferta
                }
            };

            OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, false);

            resultado.DescricaoOferta = of.DescricaoCompleta;

            if (resultado.Exportado.GetValueOrDefault())
                throw new InscricaoOfertaAlterarOfertaCandidatoJaExportadoException(resultado.NomeInscrito, resultado.DescricaoOferta);

            return resultado;
        }

        public void ValidaCoincidenciaHorarioAdministrativo(OfertaVO oferta)
        {
            var processo = ProcessoDomainService.BuscarProcessoComTipoProcesso(oferta.SeqProcesso);


            if (oferta.SeqOferta.HasValue && oferta.SeqInscricaoOferta.HasValue && (processo.TipoProcesso.GestaoEventos && (processo.VerificaCoincidenciaHorario.HasValue && processo.VerificaCoincidenciaHorario.Value)))
            {
                var inscricoesOfertaInscrito = this.BuscarOfertasInscrito(new InscricaoOfertaFilterSpecification() { SeqInscrito = oferta.SeqInscrito }).Where(w => w.Oferta.SeqProcesso == oferta.SeqProcesso && w.Seq != oferta.SeqInscricaoOferta).ToList();

                bool coincide = false;

                var ofertacompleta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta.Value, false);

                //neste caso precisamos validar as datas indo e voltando
                coincide = inscricoesOfertaInscrito.Any(a =>
                {
                    return (VerificaCoincidencia(ofertacompleta.DataInicioAtividade.Value, ofertacompleta.DataFimAtividade.Value, a.Oferta.DataInicioAtividade.Value, a.Oferta.DataFimAtividade.Value) &&
                            VerificaCoincidencia(a.Oferta.DataInicioAtividade.Value, a.Oferta.DataFimAtividade.Value, ofertacompleta.DataInicioAtividade.Value, ofertacompleta.DataFimAtividade.Value));
                });

                if (coincide)
                {
                    var labelOferta = "oferta";

                    if (processo.Idiomas != null && processo.Idiomas.Any())
                    {
                        labelOferta = processo.Idiomas.LastOrDefault().LabelOferta ?? "oferta";
                    }

                    throw new HorarioEDataCoicidenteException(labelOferta.ToLower());
                }

            }
        }
        /// <summary>
        /// Valida se as datas e sobrepoe
        /// </summary>
        /// <param name="incio1"></param>
        /// <param name="fim1"></param>
        /// <param name="incio2"></param>
        /// <param name="fim2"></param>
        /// <returns></returns>
        private bool VerificaCoincidencia(DateTime incio1, DateTime fim1, DateTime incio2, DateTime fim2)
        {
            var retorno = incio1 <= fim2 && incio2 <= fim1;

            return retorno;
        }

        /// <summary>
        /// RN_SEL_014 Alteração de oferta
        /// A nova oferta deverá ser salva no campo seq_hierarquia_oferta da tabela inscricao_oferta.
        /// Ao trocar a oferta, salvar o usuário logado no campo usu_responsavel_alteracao_oferta
        /// e a data corrente no campo dat_alteracao_oferta, da tabela inscricao_oferta.
        /// Caso o usuário clique em "Salvar" sem ter alterado nenhuma informação na tela, o
        /// usuário e a data de alteração da oferta não deverão ser atualizados na tabela.Isso é
        /// necessário porque as informações de alteração da oferta serão exibidas em outra tela e,
        /// portanto, deverão ser registradas somente se houver alteração.
        /// </summary>
        /// <param name="oferta"></param>
        public void SalvarAlteracaoOferta(OfertaVO oferta)
        {


            /* Quando o candidato faz sua inscrição, o sistema salva na tabela inscricao_oferta o seq_hierarquia_oferta e o seq_hierarquia_oferta_original, com o mesmo valor,
             * que é o sequencial da oferta selecionada. Quando o usuário troca a oferta nessa tela que você tá olhando, o sistema deverá salvar a nova oferta no campo
             * seq_hierarquia_oferta da tabela inscricao_oferta, informando os dados de auditoria da alteração. Porém, se o usuário, entrar nessa tela e não alterar o
             * seq_hierarquia_oferta, ou seja, entrar só pra ver e não fazer mais nada, o sistema não deverá atualizar os dados de auditoria, porque não houve alteração.
             * Não deve ser feito nenhum update no banco.*/

            var dadosInscricaoOferta = this.SearchProjectionByKey(oferta.SeqInscricaoOferta.Value, x => new
            {
                x.Seq,
                x.SeqOferta,
                x.Inscricao.SeqInscrito,
                x.SeqOfertaOriginal,
                x.JustificativaAlteracaoOferta,
                x.Exportado,
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                DescricaoOferta = x.Oferta.DescricaoCompleta,
                x.Oferta.LimitePercentualDesconto,
                CalculoBolsaSocial = (bool?)x.Oferta.Processo.TipoProcesso.Consistencias.Any(c => c.TipoConsistencia == TipoConsistencia.CalculoBolsaSocial) ?? false
            });

            oferta.SeqInscrito = dadosInscricaoOferta.SeqInscrito;
            ValidaCoincidenciaHorarioAdministrativo(oferta);

            if (dadosInscricaoOferta.Exportado.GetValueOrDefault())
                throw new InscricaoOfertaAlterarOfertaCandidatoJaExportadoException(dadosInscricaoOferta.Nome, dadosInscricaoOferta.DescricaoOferta);

            string justificativaAlteracaoSalvar = oferta.JustificativaAlteracaoOferta;

            // Caso tenha alterado a oferta
            if (oferta.SeqOferta.HasValue && oferta.SeqOferta != dadosInscricaoOferta.SeqOferta)
            {
                // Caso tenha voltado pra oferta original, limpa os dados da alteração
                if (oferta.SeqOferta == dadosInscricaoOferta.SeqOfertaOriginal)
                    justificativaAlteracaoSalvar = null;

                // Verifica se teve alteração no percentual de bolsa
                if (dadosInscricaoOferta.CalculoBolsaSocial)
                {
                    var limiteOfertaSelecionada = OfertaDomainService.SearchProjectionByKey(oferta.SeqOferta.Value, x => x.LimitePercentualDesconto);
                    if (limiteOfertaSelecionada != dadosInscricaoOferta.LimitePercentualDesconto)
                        throw new InscricaoOfertaAlterarOfertaLimiteDescontoDiferenteException();
                }

                UpdateFields(new InscricaoOferta
                {
                    Seq = dadosInscricaoOferta.Seq,
                    SeqOferta = oferta.SeqOferta.Value,
                    JustificativaAlteracaoOferta = justificativaAlteracaoSalvar,
                    UsuarioAlteracaoOferta = SMCContext.User.SMCGetNome(),
                    DataAlteracaoOferta = DateTime.Now
                }, x => x.SeqOferta, x => x.JustificativaAlteracaoOferta, x => x.UsuarioAlteracaoOferta, x => x.DataAlteracaoOferta);
            }
            else if (dadosInscricaoOferta.JustificativaAlteracaoOferta != oferta.JustificativaAlteracaoOferta)
            {
                UpdateFields(new InscricaoOferta
                {
                    Seq = dadosInscricaoOferta.Seq,
                    JustificativaAlteracaoOferta = justificativaAlteracaoSalvar,
                    UsuarioAlteracaoOferta = SMCContext.User.SMCGetNome(),
                    DataAlteracaoOferta = DateTime.Now
                }, x => x.JustificativaAlteracaoOferta, x => x.UsuarioAlteracaoOferta, x => x.DataAlteracaoOferta);
            }
        }

        #endregion Alteração Oferta

        public OfertaVO BuscarDadosOferta(InscricaoOfertaFilterSpecification filtro)
        {
            var oferta = BuscarDados(filtro);

            oferta.OfertaAtual = PreencheDadosCompletos(oferta);

            if (oferta.SeqOfertaOriginal != null)
            {
                if (oferta.SeqOfertaOriginal.HasValue)
                    oferta.OfertaOriginal = OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOfertaOriginal.Value, false).DescricaoCompleta;
            }
            return oferta;

        }

        /// <summary>
        /// Efeturar o checkin do inscrito
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        public RespostaCheckinVO EfetuarCheckin(CheckinVO dados) // Use CheckinData para consistência
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            try
            {
                Guid guidInscricao = Guid.Empty;
                // Valida se é um guid que veio do QRCode
                if (!Guid.TryParse(dados.Guid, out guidInscricao))
                {
                    retorno.Mensagem = "QRcode não é válido. Verifique se o código está correto.";
                    retorno.StatusCode = HttpStatusCode.BadRequest;
                    return retorno;
                }

                // Buscar dados da inscrição e oferta
                var spec = new InscricaoOfertaFilterSpecification() { Guid = guidInscricao };
                var inscricaoOferta = this.SearchProjectionBySpecification(spec, x => new
                {
                    x.Seq,
                    x.SeqInscricao,
                    x.SeqOferta,
                    x.Inscricao.SeqProcesso,
                    x.Inscricao.SeqGrupoOferta,
                    Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                    DescricaoOferta = x.Oferta.DescricaoCompleta,
                    DescricaoProcesso = x.Inscricao.Processo.Descricao,
                    DataFimAtividade = x.Oferta.DataFimAtividade.Value,
                    DataInicioAtividade = x.Oferta.DataInicioAtividade.Value,
                    SituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                    DescricaoSituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                    x.DataCheckin,
                    x.UsuarioCheckin,
                    x.TipoCheckin,
                    x.Inscricao.Processo.HoraAberturaCheckin
                }).FirstOrDefault();

                // Chama a função de validação das regras
                if (!ValidacoesRegrasCheckin(inscricaoOferta, retorno, dados))
                {
                    // Se a validação falhar, 'retorno' já estará preenchido com a mensagem e status
                    return retorno;
                }

                // Efetuar o checkin (apenas se todas as validações passarem)
                var checkin = new InscricaoOferta()
                {
                    Seq = inscricaoOferta.Seq,
                    TipoCheckin = dados.TipoCheckin,
                    DataCheckin = DateTime.Now,
                    UsuarioCheckin = SMCContext.User.SMCGetNome()
                };

                UpdateFields(checkin, x => x.DataCheckin, x => x.UsuarioCheckin, x => x.TipoCheckin);

                
                retorno.Mensagem = $"Check-in realizado com sucesso para: {inscricaoOferta.Nome}";
                retorno.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                retorno.Mensagem = $"Ocorreu um erro ao efetuar o check-in: {ex.Message}";
                retorno.StatusCode = HttpStatusCode.InternalServerError;
            }

            return retorno;
        }

        public RespostaCheckinVO EfetuarCheckin_(CheckinVO dados)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            Guid guidInscricao = Guid.Empty;
            //Valida se é um guid que veio do QRCode
            if (!Guid.TryParse(dados.Guid, out guidInscricao))
            {
                retorno.Mensagem = "QRcode não é valido";
                retorno.StatusCode = HttpStatusCode.BadRequest;
                return retorno;
            }

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification() { Guid = guidInscricao };
            var inscricaoOferta = this.SearchProjectionBySpecification(spec, x => new
            {
                x.Seq,
                x.SeqInscricao,
                x.SeqOferta,
                x.Inscricao.SeqProcesso,
                x.Inscricao.SeqGrupoOferta,
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                DescricaoOferta = x.Oferta.DescricaoCompleta,
                DescricaoProcesso = x.Inscricao.Processo.Descricao,
                DataFimAtividade = x.Oferta.DataFimAtividade.Value,
                DataInicioAtividade = x.Oferta.DataInicioAtividade.Value,
                SituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                DescricaoSituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                x.DataCheckin,
                x.UsuarioCheckin,
                x.TipoCheckin,
                x.Inscricao.Processo.HoraAberturaCheckin
            }).FirstOrDefault();

            if (!ValidacoesRegrasCheckin(inscricaoOferta, retorno, dados))
            {
                return retorno;
            }

            //Efetuar o checkin
            var checkin = new InscricaoOferta()
            {
                Seq = inscricaoOferta.Seq,
                TipoCheckin = dados.TipoCheckin,
                DataCheckin = DateTime.Now,
                UsuarioCheckin = SMCContext.User.SMCGetNome()
            };

            UpdateFields(checkin, x => x.DataCheckin, x => x.UsuarioCheckin, x => x.TipoCheckin);

            retorno.Mensagem = inscricaoOferta.Nome;
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;
        }

        /// <summary>
        /// Efeturar o checkin manual do inscrito
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        public RespostaCheckinVO EfetuarCheckinManual(CheckinVO dados)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            Guid guidInscricao = Guid.Empty;
            //Valida se é um guid que veio do QRCode
            if (!Guid.TryParse(dados.Guid, out guidInscricao))
            {
                retorno.Mensagem = "QRcode não é valido";
                retorno.StatusCode = HttpStatusCode.BadRequest;
                return retorno;
            }

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification() { Guid = guidInscricao };
            var inscricaoOferta = this.SearchProjectionBySpecification(spec, x => new
            {
                x.Seq,
                x.SeqInscricao,
                x.SeqOferta,
                x.Inscricao.SeqProcesso,
                x.Inscricao.SeqGrupoOferta,
                DescricaoProcesso = x.Inscricao.Processo.Descricao,
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                DescricaoOferta = x.Oferta.DescricaoCompleta,
                DataFimAtividade = x.Oferta.DataFimAtividade.Value,
                DataInicioAtividade = x.Oferta.DataInicioAtividade.Value,
                SituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                DescricaoSituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                x.DataCheckin,
                x.UsuarioCheckin,
                x.TipoCheckin
            }).FirstOrDefault();

            if (!ValidacoesRegrasCheckin(inscricaoOferta, retorno, dados))
            {
                return retorno;
            }

            //Efetuar o checkin
            var checkin = new InscricaoOferta()
            {
                Seq = inscricaoOferta.Seq,
                TipoCheckin = TipoCheckin.QRCode,
                DataCheckin = DateTime.Now,
                UsuarioCheckin = SMCContext.User.SMCGetNome()
            };

            UpdateFields(checkin, x => x.DataCheckin, x => x.UsuarioCheckin, x => x.TipoCheckin);

            retorno.Mensagem = inscricaoOferta.Nome;
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;
        }

        /// <summary>
        /// Efeturar o checkout do inscrito
        /// </summary>
        /// <param name="guid">Guid inscrição oferta</param>
        /// <returns></returns>
        public RespostaCheckinVO EfetuarCheckout(string guid)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            Guid guidInscricao = Guid.Empty;
            //Valida se é um guid que veio do QRCode
            if (!Guid.TryParse(guid, out guidInscricao))
            {
                retorno.Mensagem = "QRcode não é valido";
                retorno.StatusCode = HttpStatusCode.BadRequest;
                return retorno;
            }

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification() { Guid = guidInscricao };
            var inscricaoOferta = this.SearchProjectionBySpecification(spec, x => new
            {
                x.Seq,
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome
            }).FirstOrDefault();


            //Efetuar o checkout
            var checkinOut = new InscricaoOferta()
            {
                Seq = inscricaoOferta.Seq,
                TipoCheckin = null,
                DataCheckin = null,
                UsuarioCheckin = null,
            };

            UpdateFields(checkinOut, x => x.DataCheckin, x => x.UsuarioCheckin, x => x.TipoCheckin);

            retorno.Mensagem = "Checkout efetuado com sucesso!";
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;
        }

        /// <summary>
        /// Pesquisar inscrito checkin manual
        /// </summary>
        /// <param name="filtro">Dados do inscrito</param>
        /// <returns>Mensagem com o status code</returns>
        public RespostaCheckinVO PesquisaNomeCheckinManual(FiltroCheckinVO filtro)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();
            //var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(inscricao.Processo.SeqTemplateProcessoSGF);

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification()
            {
                NomeInscrito = filtro.Nome,
                Cpf = filtro.CPF,
                SeqsOfertas = filtro.SeqsOferta,
                TokenHistoricoSituacao = filtro.TokenHistoricoSituacao,
                HistoricoInscricaoAtual = true,
                MaxResults = 10
            };

            var Inscrito = this.SearchProjectionBySpecification(spec, x => new
            {
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
            }).ToList();

            retorno.Mensagem = string.Join("|", Inscrito.Select(x => x.Nome).Distinct().ToList());
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;
        }

        /// <summary>
        /// Pesquisar inscrito checkin manual
        /// </summary>
        /// <param name="filtro">Dados do inscrito</param>
        /// <returns>Mensagem com o status code</returns>
        public List<RespostaPesquiaOfertaCheckinVO> PesquisaOfertaCheckinManual(FiltroCheckinVO filtro)
        {
            List<RespostaPesquiaOfertaCheckinVO> retorno = new List<RespostaPesquiaOfertaCheckinVO>();

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification()
            {
                NomeInscrito = filtro.Nome,
                Cpf = filtro.CPF,
                SeqsOfertas = filtro.SeqsOferta,
                TokenHistoricoSituacao = filtro.TokenHistoricoSituacao,
                HistoricoInscricaoAtual = true
            };

            var Inscritos = this.SearchProjectionBySpecification(spec, x => new
            {
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                x.DataCheckin,
                x.TipoCheckin,
                x.UsuarioCheckin,
                x.Oferta.DescricaoCompleta,
                x.SeqOferta,
                x.UidInscricaoOferta
            }).OrderBy(o => o.Nome).ToList();

            retorno = Inscritos.GroupBy(g => new { g.DescricaoCompleta, g.SeqOferta }).Select(x => new RespostaPesquiaOfertaCheckinVO
            {
                DescricaoOferta = x.Key.DescricaoCompleta,
                SeqOferta = x.Key.SeqOferta,
                Inscritos = x.Select(i => new RespostaPesquiaOfertaInscritoCheckinVO
                {
                    Nome = i.DataCheckin.HasValue ? "<p>" + i.Nome + "</p>" + "<span class='smc-gpi-detalhe-checkin'>" + " Check-in realizado em " + i.DataCheckin + " - " + i.TipoCheckin?.SMCGetDescription() : i.Nome + "</span>",
                    GuidInscricao = i.UidInscricaoOferta.ToString(),
                    CheckinEfetuado = i.DataCheckin.HasValue,
                }).ToList()
            }).ToList();

            return retorno;
        }

        /// <summary>
        /// Verifica se o Inscrito possui checkin
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool VerificaPossuiCkeckin(long seqInscricao)
        {

            var spec = new InscricaoOfertaFilterSpecification()
            {
                SeqInscricao = seqInscricao,
                VerificaPossuiCheckin = true
            };

            var possuiCheckin = this.SearchProjectionBySpecification(spec, x => new { }).Any();

            return possuiCheckin;
        }

        private bool ValidacoesRegrasCheckin(dynamic inscricaoOferta, RespostaCheckinVO retorno, CheckinVO dados, bool checkinLote = false)
        {
            bool result = true;

            //Valida se a inscrição foi encontrada
            if (inscricaoOferta == null)
            {
                retorno.Mensagem = "Inscrição não encontrada";
                retorno.StatusCode = HttpStatusCode.NotFound;
                result = false;
            }

            //Valida se o Atendente pode fazer o checkin da oferta
            if (result && dados.SeqsOferta != null && !dados.SeqsOferta.Contains(inscricaoOferta.SeqOferta))
            {
                retorno.Mensagem = inscricaoOferta.Nome.ToUpper() + "<br />";
                retorno.Mensagem += "Esta inscrição não pertence à(s) atividade(s) selecionada(s).";

                var of = OfertaDomainService.SearchByKey((long)inscricaoOferta.SeqOferta, p => p.Processo);

                if (!string.IsNullOrEmpty(of.HierarquiaCompleta))
                {
                    retorno.Mensagem += "<p>" + FormataHierarquia(of.DescricaoCompleta) + "</p>";
                }
                else
                {
                    OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, true);
                    retorno.Mensagem += "<p>" + inscricaoOferta.DescricaoProcesso + " " + of.DescricaoCompleta + "</p>";
                }

                retorno.StatusCode = HttpStatusCode.Forbidden;
                result = false;
            }

            //Busca a data atual do sistema e cria uma nova data com os segundos zero
            DateTime dataAtual = DateTime.Now;
            DateTime dataAtividadeAtual = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day, dataAtual.Hour, dataAtual.Minute, 0);

            if (!checkinLote)
            {
                if (result && !(inscricaoOferta.DataInicioAtividade.AddHours(-inscricaoOferta.HoraAberturaCheckin.TotalHours) <= dataAtividadeAtual && inscricaoOferta.DataFimAtividade >= dataAtividadeAtual))
                {
                    retorno.Mensagem = inscricaoOferta.Nome.ToUpper() + "<br />";
                    retorno.Mensagem += "Não é permitido realizar o check-in fora do horário da atividade.";
                    retorno.StatusCode = HttpStatusCode.BadRequest;
                    result = false;
                }
            }

            //Validar se o checkin já foi efetuado
            if (result && inscricaoOferta.DataCheckin != null)
            {
                retorno.Mensagem = inscricaoOferta.Nome.ToUpper() + "<br />";
                retorno.Mensagem += "Check-in realizado previamente em " + inscricaoOferta.DataCheckin + " por " + inscricaoOferta.UsuarioCheckin;
                retorno.StatusCode = HttpStatusCode.BadRequest;
                result = false;
            }

            //Validar situação da inscrição
            if (result &&
                (dados.TokenHistoricoSituacao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA &&
                inscricaoOferta.SituacaoInscricao != TOKENS.SITUACAO_INSCRICAO_CONFIRMADA) ||
                (dados.TokenHistoricoSituacao == TOKENS.SITUACAO_INSCRICAO_DEFERIDA &&
                inscricaoOferta.SituacaoInscricao != TOKENS.SITUACAO_INSCRICAO_DEFERIDA))
            {
                retorno.Mensagem = inscricaoOferta.Nome.ToUpper() + "<br />";
                retorno.Mensagem += $"O check-in não pode ser realizado para {inscricaoOferta.DescricaoSituacaoInscricao}.";
                retorno.StatusCode = HttpStatusCode.BadRequest;
                result = false;
            }

            return result;
        }

        private string FormataHierarquia(string descricaoOfertaCompleta)
        {
            var descOfertaSplit = descricaoOfertaCompleta.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
            //mensagem += "<br />";
            string retorno = string.Empty;
            if (descOfertaSplit.Length == 1)
            {
                retorno += descOfertaSplit[0];
            }
            else if (descOfertaSplit.Length == 2)
            {
                retorno += descOfertaSplit[0] + "<br />";
                retorno += descOfertaSplit[1];
            }
            else if (descOfertaSplit.Length == 3)
            {
                retorno += descOfertaSplit[0] + "<br />";
                retorno += descOfertaSplit[1] + "<br />";
                retorno += descOfertaSplit[2];
            }
            else
            {
                for (int i = 0; i < descOfertaSplit.Length; i++)
                {
                    if (i == 0)
                    {
                        retorno += descOfertaSplit[i] + "<br />";
                    }
                    else if (i == 1)
                    {
                        retorno += descOfertaSplit[i] + "<br />";
                    }
                    else
                    {
                        retorno += descOfertaSplit[i] + " => ";
                    }
                    retorno.Remove(retorno.Length - 4);
                }
            }
            return retorno;
        }


        public List<InscricaoCheckinLoteVO> BuscarInscricoesOfertaPorSeqOferta(long seqOferta)
        {
            var spec = new InscricaoOfertaFilterSpecification() { SeqOferta = seqOferta };

            var inscritos = this.SearchProjectionBySpecification(spec, s => new InscricaoCheckinLoteVO()
            {
                SeqInscricao = s.SeqInscricao,
                NomeInscrito = s.Inscricao.Inscrito.Nome,
                DataHoraCheckin = s.DataCheckin,
                PossuiCheckin = s.TipoCheckin != null && s.TipoCheckin != TipoCheckin.Nenhum,
                Responsavel = s.UsuarioCheckin
            }).ToList();

            return inscritos;
        }

        public CabecalhoCheckinLoteVO BuscarCabecalhoCheckinLote(long seqOferta, long seqProcesso)
        {
            var result = OfertaDomainService.BuscarPosicaoConsolidadaCheckin(new PosicaoConsolidadaCheckinFilterSpecification { SeqProcesso = seqProcesso, SeqOferta = seqOferta }, out int total).FirstOrDefault();

            //Cria o objeto final do cabecalho
            var cabecalho = new CabecalhoCheckinLoteVO()
            {
                TotalInscritos = result.NumeroInscrito,
                DescricaoProcesso = result.DescricaoProcesso,
                DescricaoOferta = result.DescricaoOferta,
                TotalChekinsRealizados = result.NumeroChecinRealizado,
                TotalRestantes = result.RestanteVagas,
            };

            return cabecalho;

        }
        public SMCPagerData<ListarCheckinLoteVO> BuscarInscritosCheckinLote(CheckinLoteFiltroData filtro)
        {
            //caso faça uma pesquisa sem setar se o checkin foi realizado ou não
            if (!filtro.CheckinRealizado.HasValue)
            {
                var listaVazia = new List<ListarCheckinLoteVO>();
                return new SMCPagerData<ListarCheckinLoteVO>(listaVazia);
            }

            var processo = this.ProcessoDomainService.SearchProjectionByKey(filtro.SeqProcesso.Value, p => new { p.SeqTemplateProcessoSGF, p.Descricao });
            var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(processo.SeqTemplateProcessoSGF);
            var tokenSituacao = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;

            var spec = new InscricaoOfertaFilterSpecification()
            {
                SeqOferta = filtro.SeqOferta,
                SeqInscricao = filtro.SeqInscricao,
                NomeInscrito = filtro.NomeInscrito,
                CheckinRealizado = filtro.CheckinRealizado,
                Token = tokenSituacao
            };

            spec.SetOrderBy(s => s.Inscricao.Inscrito.Nome);

            //Busca os inscritos e os detalhes necessarios
            var inscricoes = this.SearchProjectionBySpecification(spec, s => new ListarCheckinLoteVO()
            {
                SeqInscricao = s.SeqInscricao,
                Responsavel = s.UsuarioCheckin,
                DataHoraCheckin = s.DataCheckin,
                NomeInscrito = s.Inscricao.Inscrito.Nome,
                PossuiCheckin = s.TipoCheckin != null && s.TipoCheckin != TipoCheckin.Nenhum
            }).ToList();

            return new SMCPagerData<ListarCheckinLoteVO>(inscricoes);
        }

        /// <summary>
        /// Efeturar o checkin manual do inscrito
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        public RespostaCheckinVO EfetuarCheckinLote(CheckinVO dados)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification() { SeqsInscricao = dados.SeqsInscricao, SeqOferta = dados.SeqOferta };

            var inscricoesOfertaVo = this.SearchProjectionBySpecification(spec, x => new
            {
                x.Seq,
                x.SeqInscricao,
                x.SeqOferta,
                x.Inscricao.SeqProcesso,
                x.Inscricao.SeqGrupoOferta,
                DescricaoProcesso = x.Inscricao.Processo.Descricao,
                Nome = x.Inscricao.Inscrito.NomeSocial ?? x.Inscricao.Inscrito.Nome,
                DescricaoOferta = x.Oferta.DescricaoCompleta,
                DataFimAtividade = x.Oferta.DataFimAtividade.Value,
                DataInicioAtividade = x.Oferta.DataInicioAtividade.Value,
                SituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                DescricaoSituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                x.DataCheckin,
                x.UsuarioCheckin,
                x.TipoCheckin
            }).ToList();

            foreach (var item in inscricoesOfertaVo)
            {

                if (!ValidacoesRegrasCheckin(item, retorno, dados, true))
                {
                    return retorno;
                }

                //Efetuar o checkin
                var checkin = new InscricaoOferta()
                {
                    Seq = item.Seq,
                    TipoCheckin = TipoCheckin.EmLote,
                    DataCheckin = DateTime.Now,
                    UsuarioCheckin = SMCContext.User.SMCGetNome()
                };

                this.UpdateFields(checkin, x => x.DataCheckin, x => x.UsuarioCheckin, x => x.TipoCheckin);
            }

            retorno.Mensagem = "Checkin em lote realizado com sucesso.";
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;

        }

        public RespostaCheckinVO DesfazerCheckinLote(CheckinVO dados)
        {
            RespostaCheckinVO retorno = new RespostaCheckinVO();

            //Buscar dados da inscrição e oferta
            var spec = new InscricaoOfertaFilterSpecification() { SeqsInscricao = dados.SeqsInscricao, SeqOferta = dados.SeqOferta, CheckinRealizado = true };

            var inscricoesOferta = this.SearchProjectionBySpecification(spec, x => x).ToList();


            foreach (var item in inscricoesOferta)
            {
                item.TipoCheckin = TipoCheckin.Nenhum;
                item.DataCheckin = null;
                item.UsuarioCheckin = null;
            }

            this.BulkSaveEntity(inscricoesOferta);

            retorno.Mensagem = "Desfazer checkin em lote realizado com sucesso.";
            retorno.StatusCode = HttpStatusCode.OK;

            return retorno;

        }

        /// <summary>
        /// Buscar dados da inscrição oferta por guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Dados da inscricao oferta</returns>
        public InscricaoOfertaVO BuscarInscricaoOfertaPorGuid(Guid guid)
        {
            var spec = new InscricaoOfertaFilterSpecification() { Guid = guid };

            var retorno = this.SearchProjectionBySpecification(spec, x => new InscricaoOfertaVO
            {
                Seq = x.Seq,
                SeqInscricao = x.SeqInscricao,
                SeqInscrito = x.Inscricao.SeqInscrito,
                UidInscricaoOferta = x.UidInscricaoOferta,
                UidProcesso = x.Inscricao.Processo.UidProcesso,
                NomeInscrito = x.Inscricao.Inscrito.Nome,
                SeqUsuario = x.Inscricao.Inscrito.SeqUsuarioSas,
            }).FirstOrDefault();

            return retorno;
        }


        #region[Metodos de Busca oferta atual e oferta original]
        private OfertaVO BuscarDados(InscricaoOfertaFilterSpecification filtro)
        {

            return this.SearchProjectionByKey(filtro,
                                x => new OfertaVO
                                {
                                    Candidato = (x.Inscricao.Inscrito.NomeSocial != null) ?
                                                    x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" :
                                                    x.Inscricao.Inscrito.Nome,
                                    SeqInscricaoOferta = x.Seq,
                                    SeqInscricao = x.SeqInscricao,
                                    DescricaoOferta = x.Oferta.DescricaoCompleta,
                                    OfertaAtual = x.Oferta.DescricaoCompleta,
                                    SeqGrupoOferta = x.Oferta.SeqGrupoOferta,
                                    DataAlteracaoOferta = x.DataAlteracaoOferta,
                                    JustificativaAlteracaoOferta = x.JustificativaAlteracaoOferta,
                                    OfertaOriginal = x.OfertaOriginal.DescricaoCompleta,
                                    SeqOfertaOriginal = x.SeqOfertaOriginal,
                                    JustificativaInscricao = x.JustificativaInscricao,
                                    UsuarioAlteracaoOferta = x.UsuarioAlteracaoOferta,
                                    OfertasIguais = x.SeqOferta == x.SeqOfertaOriginal,
                                    ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                                    DataInicioAtividade = x.Oferta.DataInicioAtividade,
                                    DataFimAtividade = x.Oferta.DataFimAtividade,
                                    CargaHorariaAtividade = x.Oferta.CargaHorariaAtividade,
                                    Nome = x.Oferta.Nome

                                });
        }



        private string PreencheDadosCompletos(OfertaVO oferta)
        {
            var of = new Oferta()
            {
                Nome = oferta.Nome,
                DescricaoCompleta = oferta.DescricaoOferta,
                DataInicioAtividade = oferta.DataInicioAtividade,
                DataFimAtividade = oferta.DataFimAtividade,
                CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                Processo = new Processo()
                {
                    ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOferta
                }
            };

            OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, false);

            return of.DescricaoCompleta;

        }
        #endregion



        public List<InscricaoOferta> BuscarOfertasInscrito(InscricaoOfertaFilterSpecification filter)
        {


            var ofertas = this.SearchBySpecification(filter, IncludesInscricaoOferta.Oferta_GrupoOferta_Processo_Idiomas).ToList();

            foreach (var item in ofertas)
            {
                OfertaDomainService.AdicionarDescricaoCompleta(item.Oferta, item.Oferta.GrupoOferta.Processo.ExibirPeriodoAtividadeOferta, false);
            }

            return ofertas;

        }
    }
}