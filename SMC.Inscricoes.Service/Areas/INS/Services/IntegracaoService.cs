using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Logging;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class IntegracaoService : SMCServiceBase, IIntegracaoService
    {
        #region Domain Services

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService
        {
            get { return Create<ArquivoAnexadoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return Create<InscricaoDomainService>(); }
        }

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return Create<InscricaoOfertaDomainService>(); }
        }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService
        {
            get { return Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }
        }

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return Create<TipoProcessoSituacaoDomainService>(); }
        }

        private InscricaoHistoricoSituacaoDomainService InscricaoHistoricoSituacaoDomainService { get => Create<InscricaoHistoricoSituacaoDomainService>(); }

        #endregion Domain Services

        #region Services

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        #endregion Services

        public List<PessoaIntegracaoData> BuscarDadosInscricoes(List<long> seqOfertas)
        {
            if (seqOfertas == null)
                return null;

            return InscricaoDomainService.BuscarDadosInscricoes(seqOfertas).TransformList<PessoaIntegracaoData>();
        }

        public bool ExisteInscricoesConvocadas(List<long> seqOfertas)
        {
            try
            {
                if (seqOfertas == null)
                    return false;

                // Busca por todos os inscritos que possuem alguma situação CONVOCADA que ainda não foram exportados.
                var spec = new InscritoConvocadoSGASpecification()
                {
                    SeqOfertas = seqOfertas
                };
                return InscricaoDomainService.Count(spec) > 0;
            }
            catch (System.Exception ex)
            {
                SMCLogger.Error(ex.SMCLastInnerException().Message);
                throw;
            }
        }

        public SMCPagerData<InscritoProcessoIntegracaoData> BuscarInscritosProcesso(InscritoProcessoIntegracaoFiltroData filtro)
        {
            var result = InscricaoOfertaDomainService.SearchProjectionBySpecification(filtro.Transform<InscricaoOfertasPorProcessoSpecification>(new { TokenEtapa = TOKENS.ETAPA_CONVOCACAO }),
                                                x => new InscritoProcessoIntegracaoData
                                                {
                                                    SeqInscricao = x.SeqInscricao,
                                                    Nome = string.IsNullOrEmpty(x.Inscricao.Inscrito.NomeSocial) ? x.Inscricao.Inscrito.Nome : x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")",
                                                    SeqInscricaoOferta = x.Seq,
                                                    Oferta = x.Oferta.DescricaoCompleta,
                                                    Processo = x.Inscricao.Processo.Descricao,
                                                    SeqHierarquiaOferta = x.Oferta.Seq
                                                }, out int total).ToList();
            return new SMCPagerData<InscritoProcessoIntegracaoData>(result, total);
        }

        public SMCUploadFile BuscarArquivoAnexado(long seqArquivoAnexado)
        {
            return ArquivoAnexadoDomainService.SearchByKey(new SMCSeqSpecification<ArquivoAnexado>(seqArquivoAnexado)).Transform<SMCUploadFile>();
        }

        public void AtualizarExportacaoInscricao(List<long> inscricaoOfertas)
        {
            InscricaoOfertaDomainService.AtualizarExportacaoInscricao(inscricaoOfertas);
        }

        public void AlterarSituacaoConvocadoParaDesistente(long seqInscricaoOferta)
        {
            var inscricaoOferta = InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta),
                                                                        x => new
                                                                        {
                                                                            x.Inscricao.SeqProcesso,
                                                                            x.Inscricao.Processo.TipoProcesso.SeqTipoTemplateProcessoSGF,
                                                                            x.Inscricao.Processo.SeqTipoProcesso,
                                                                        });
            var specTipoProcessoSituacao = new TipoProcessoSituacaoFilterSpecification() { Token = TOKENS.SITUACAO_CONVOCADO_DESISTENTE, SeqTipoProcesso = inscricaoOferta.SeqTipoProcesso };
            var tipoProcessoSituacao = TipoProcessoSituacaoDomainService.SearchByKey(specTipoProcessoSituacao);

            //if (!seqSituacao.HasValue)
            //    throw new VoceNaoConfigurouSGFDireitoENaoAcheiUmaSituacaoParaEssesParametrosExpcetion();

            var seqMotivo = SituacaoService.BuscarSeqMotivoSituacaoPorToken(tipoProcessoSituacao.SeqSituacao, TOKENS.MOTIVO_CONVOCADO_DESISTENTE_NAO_MATRICULADO);

            //if (!seqMotivo.HasValue)
            //    throw new NaoTemMotivo

            InscricaoOfertaHistoricoSituacaoDomainService.AlterarHistoricoSituacao(new AlterarHistoricoSituacaoVO()
            {
                SeqProcesso = inscricaoOferta.SeqProcesso,
                SeqInscricoesOferta = new List<long>() { seqInscricaoOferta },
                SeqTipoProcessoSituacaoDestino = tipoProcessoSituacao.Seq,
                SeqMotivoSGF = seqMotivo.Value
            });
        }

        /// <summary>
        /// Buscar o historico da inscrição mais atual
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial incrição oferta</param>
        /// <returns>Retorna os dados da incrição</returns>
        public InscricaoOfertaHistoricoSituacaoData BuscarHistoricosSituacaoAtual(long seqInscricaoOferta)
        {
            return this.InscricaoOfertaHistoricoSituacaoDomainService.BuscarHistoricosSituacaoAtual(seqInscricaoOferta)
                            .Transform<InscricaoOfertaHistoricoSituacaoData>();
        }
    }
}