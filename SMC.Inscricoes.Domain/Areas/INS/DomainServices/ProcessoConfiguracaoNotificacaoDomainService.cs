using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.NOT.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.NOT.DomainServices;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ProcessoConfiguracaoNotificacaoDomainService : InscricaoContextDomain<ProcessoConfiguracaoNotificacao>
    {
        #region [ Domains Services ]

        private ProcessoConfiguracaoNotificacaoIdiomaDomainService ProcessoConfiguracaoNotificacaoIdiomaDomainService => this.Create<ProcessoConfiguracaoNotificacaoIdiomaDomainService>();

        private TipoNotificacaoDomainService TipoNotificacaoDomainService => this.Create<TipoNotificacaoDomainService>();

        private ProcessoDomainService ProcessoDomainService => this.Create<ProcessoDomainService>();

        private InscricaoEnvioNotificacaoDomainService InscricaoEnvioNotificacaoDomainService => this.Create<InscricaoEnvioNotificacaoDomainService>();

        #endregion [ Domains Services ]

        #region [ Services ]

        private INotificacaoService NotificacaoService => this.Create<INotificacaoService>();

        #endregion [ Services ]

        public long SalvarConfiguracaoNotificacao(ConfigurarNotificacaoVO configurarNotificacaoVo)
        {
            var configuracaoNotificacao = configurarNotificacaoVo.Transform<ProcessoConfiguracaoNotificacao>();

            var existeLiberada = this.ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(
                configuracaoNotificacao.SeqProcesso), x => x.EtapasProcesso.Any(e => e.SituacaoEtapa == SituacaoEtapa.Liberada && e.DataFimEtapa > DateTime.Today));
            if (existeLiberada)
            {
                throw new ConfiguracaoNotificacaoEtapaLiberadaException();
                //Para incluir ou alterar uma configuração de notificação, a situação de todas as etapas do processo que ainda não foram encerradas deve estar “Aguardando Liberação“ ou “Em Manutenção”.
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    //Salva o registro no GPI, pois precisamos do Seq para preencher corretamente a descrição da configuracao de notificação
                    this.SaveEntity(configuracaoNotificacao);

                    //
                    var specConfiguracao = new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(configuracaoNotificacao.Seq);
                    var configuracao = this.SearchByKey(specConfiguracao,
                                                IncludesProcessoConfiguracaoNotificacaoIdioma.ConfiguracoesIdioma |
                                                IncludesProcessoConfiguracaoNotificacaoIdioma.ConfiguracoesIdioma_ProcessoIdioma);

                    //Salva o registro no Notificação
                    var specProcesso = new SMCSeqSpecification<Processo>(configurarNotificacaoVo.SeqProcesso);
                    var processo = ProcessoDomainService.SearchByKey(specProcesso, IncludesProcesso.UnidadeResponsavel |
                                                                                   IncludesProcesso.Idiomas);
                    foreach (var item in configurarNotificacaoVo.ConfiguracoesEmail)
                    {
                        item.ConfiguracaoNotificacao.SeqUnidadeResponsavel = processo.UnidadeResponsavel.SeqUnidadeResponsavelNotificacao;
                        item.ConfiguracaoNotificacao.Descricao = string.Format("{0} ({1}) - ({2}) {3}",
                                                                        configurarNotificacaoVo.DescricaoTipoNotificacao,
                                                                        SMCEnumHelper.GetDescription(item.Idioma),
                                                                        configuracaoNotificacao.SeqProcesso,
                                                                        processo.Descricao);

                        //Caso o "E-mail para resposta" não tenha sido informado, gravar o valor do campo "E-mail"
                        if (string.IsNullOrWhiteSpace(item.ConfiguracaoNotificacao.EmailResposta))
                            item.ConfiguracaoNotificacao.EmailResposta = item.ConfiguracaoNotificacao.EmailOrigem;

                        // A validação das tags é feita apenas na tela de manutenção da notificação e na hora de liberar a etapa.
                        item.ConfiguracaoNotificacao.ValidaTags = configurarNotificacaoVo.ValidaTags;
                        //Salva o data de notificação
                        var seqNot = NotificacaoService.SalvarConfiguracaoTipoNotificacao(item.ConfiguracaoNotificacao);

                        var confNotIdioma = configuracao.ConfiguracoesIdioma.Where(f => f.ProcessoIdioma != null && f.ProcessoIdioma.Idioma == item.Idioma).FirstOrDefault();
                        if (confNotIdioma == null)
                        {
                            confNotIdioma = new ProcessoConfiguracaoNotificacaoIdioma
                            {
                                SeqConfiguracaoTipoNotificacao = seqNot,
                                SeqProcessoConfiguracaoNotificacao = configuracao.Seq,
                                SeqProcessoIdioma = processo.Idiomas.Where(f => f.Idioma == item.Idioma).First().Seq
                            };
                        }
                        else
                        {
                            confNotIdioma.SeqConfiguracaoTipoNotificacao = seqNot;
                        }
                        ProcessoConfiguracaoNotificacaoIdiomaDomainService.SaveEntity(confNotIdioma);
                    }

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return configuracaoNotificacao.Seq;
        }

        /// <summary>
        /// Verifica se o processo permite a inclusão/edição de configurações de notificação.
        /// </summary>
        public void VerificaPermissaoAlteracao(long seqProcesso)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcesso);
            var processo = ProcessoDomainService.SearchByKey(spec, IncludesProcesso.EtapasProcesso | IncludesProcesso.UnidadeResponsavel);
            foreach (var etapa in processo.EtapasProcesso)
            {
                if (etapa.SituacaoEtapa == SituacaoEtapa.Liberada && etapa.DataFimEtapa > DateTime.Now)
                {
                    throw new ConfigurarNotificacaoEtapaNaoEncerradaException();
                }
            }
        }

        public void ValidarTipoNotificacao(ConfigurarNotificacaoVO configurarNotificacaoData)
        {
            var specConfiguracao = new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(configurarNotificacaoData.Seq);
            var confNot = this.SearchByKey(specConfiguracao,
                                IncludesProcessoConfiguracaoNotificacao.ParametrosEnvioNotificacao |
                                IncludesProcessoConfiguracaoNotificacao.ConfiguracoesIdioma);

            if (confNot != null)
            {
                if (configurarNotificacaoData.SeqTipoNotificacao != confNot.SeqTipoNotificacao)
                {
                    //Verifica se existem notificações
                    if (NotificacaoService.VerificarConfiguracaoPossuiNotificacoes(confNot.ConfiguracoesIdioma.Select(f => f.SeqConfiguracaoTipoNotificacao).ToArray()))
                    {
                        throw new ConfiguracaoNotificacaoJaEnviadaException();
                    }

                    if (confNot.ParametrosEnvioNotificacao.Count > 0)
                    {
                        var specTipoNot = new SMCSeqSpecification<TipoNotificacao>(configurarNotificacaoData.SeqTipoNotificacao);
                        var tipoNotificacao = this.TipoNotificacaoDomainService.SearchByKey(specTipoNot);
                        if (tipoNotificacao.PermiteAgendamento == false)
                        {
                            throw new ConfigurarNotificacaoSemAgendamentoException();
                        }
                    }
                }
            }
        }

        public void Excluir(long seqConfiguracaoNotificacao)
        {
            var specConfiguracao = new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(seqConfiguracaoNotificacao);
            var confNot = this.SearchByKey(specConfiguracao,
                                            IncludesProcessoConfiguracaoNotificacao.ConfiguracoesIdioma |
                                            IncludesProcessoConfiguracaoNotificacao.Processo |
                                            IncludesProcessoConfiguracaoNotificacao.Processo_EtapasProcesso);

            //Verifica se existe alguma etapa do processo na situação Liberada e não encerrada
            foreach (var etapa in confNot.Processo.EtapasProcesso)
            {
                if (etapa.SituacaoEtapa == SituacaoEtapa.Liberada && etapa.DataFimEtapa > DateTime.Now)
                {
                    throw new ParametroNotificacaoEtapaNaoEncerradaException();
                }
            }

            var configuracoesTipoNotificacao = confNot.ConfiguracoesIdioma.Select(f => f.SeqConfiguracaoTipoNotificacao).ToArray();
            //Verifica se existem notificações
            if (NotificacaoService.VerificarConfiguracaoPossuiNotificacoes(configuracoesTipoNotificacao))
            {
                throw new ExcluirConfiguracaoNotificacaoJaEnviadaException();
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var configToDelete = this.SearchByKey(new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(seqConfiguracaoNotificacao));
                    this.DeleteEntity(configToDelete);

                    NotificacaoService.ExcluirConfiguracaoTipoNotificacao(configuracoesTipoNotificacao);

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public void SalvarParametrosNotificacao(ProcessoConfiguracaoNotificacao processoConfiguracaoNotificacao)
        {
            var parametros = processoConfiguracaoNotificacao.ParametrosEnvioNotificacao;
            var spec = new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(processoConfiguracaoNotificacao.Seq);
            var configuracaoNotificacao = SearchByKey(spec, IncludesProcessoConfiguracaoNotificacao.ParametrosEnvioNotificacao |
                                                            IncludesProcessoConfiguracaoNotificacao.ConfiguracoesIdioma);

            //Verifica se existe alguma etapa do processo na situação Liberada e não encerrada
            var specProcesso = new SMCSeqSpecification<Processo>(configuracaoNotificacao.SeqProcesso);
            var processo = ProcessoDomainService.SearchByKey(specProcesso, IncludesProcesso.EtapasProcesso);
            foreach (var etapa in processo.EtapasProcesso)
            {
                if (etapa.SituacaoEtapa == SituacaoEtapa.Liberada && etapa.DataFimEtapa > DateTime.Now)
                {
                    throw new ParametroNotificacaoEtapaNaoEncerradaException();
                }
            }

            ValidarAlteracaoParametroNotificao(parametros, configuracaoNotificacao);

            configuracaoNotificacao.ParametrosEnvioNotificacao = parametros;
            SaveEntity(configuracaoNotificacao);
        }

        /// <summary>
        /// Valida os parâmetros novos com os parâmetros da base, se foram alterados e possuem alguma notificação
        /// </summary>
        /// <param name="parametros">Novos parâmetros</param>
        /// <param name="processoConfiguracaoNotificacao">Configuração com os parâmetros da Base</param>
        private void ValidarAlteracaoParametroNotificao(IList<ParametroEnvioNotificacao> parametros, ProcessoConfiguracaoNotificacao processoConfiguracaoNotificacao)
        {
            var parametrosAlterados = BuscarParametrosModificados(parametros, processoConfiguracaoNotificacao.ParametrosEnvioNotificacao);

            if (parametrosAlterados.SMCAny())
            {
                var specEnvioNotificacao = new InscricaoEnvioNotificacaoFilterSpecification()
                {
                    SeqProcessoConfiguracaoNotificacao = processoConfiguracaoNotificacao.Seq,
                    SeqsParametroEnvioNotificacao = parametrosAlterados.Select(x => x.Seq).ToList()
                };

                var seqsConfiguracaoTipoNotificacao = InscricaoEnvioNotificacaoDomainService.SearchProjectionBySpecification(specEnvioNotificacao, x => x.ProcessoConfiguracaoNotificacaoIdioma.SeqConfiguracaoTipoNotificacao, true).ToList();

                // Verifico se os parâmetros alterados possuem alguma notificação
                if (NotificacaoService.VerificarConfiguracaoPossuiNotificacoes(seqsConfiguracaoTipoNotificacao.ToArray()))
                {
                    // Caso exista, não poderá ser alterado
                    throw new ParametroComNotificacaoJaEnviadaException();
                }
            }
        }


        /// <summary>
        /// Busca os parâmetros que foram excluídos e/ou modificados da lista original
        /// </summary>
        /// <param name="novos">Todos parametros do modelo</param>
        /// <param name="originais">Parametros originais</param>
        /// <returns>Verdadeiro apenas se nenhum item original foi modificado ou removido</returns>
        private List<ParametroEnvioNotificacao> BuscarParametrosModificados(IList<ParametroEnvioNotificacao> novos, IList<ParametroEnvioNotificacao> originais)
        {
            var modificados = new List<ParametroEnvioNotificacao>();

            var seqsOriginais = originais.Select(s => s.Seq).ToList();
            // Caso algum item seja removido
            var excluidos = seqsOriginais.Except(novos.Select(s => s.Seq)).ToList();

            // Adiciono os excluídos
            if (excluidos.SMCAny()) { modificados.AddRange(originais.Where(x => excluidos.Contains(x.Seq))); }

            //adiciono os modificados
            foreach (var original in originais)
            {
                var novo = novos.FirstOrDefault(f => f.Seq == original.Seq);

                if (novo != null && ValidarParametroAlterado(novo, original))
                {
                    modificados.Add(novo);
                }
            }

            return modificados;
        }

        /// <summary>
        /// Verifica se o parâmetro é diferente de outro
        /// </summary>
        /// <param name="novo"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        private bool ValidarParametroAlterado(ParametroEnvioNotificacao novo, ParametroEnvioNotificacao original)
        {
            return novo.AtributoAgendamento != original.AtributoAgendamento ||
                    novo.QuantidadeDiasInicioEnvio != original.QuantidadeDiasInicioEnvio ||
                    novo.QuantidadeDiasRecorrencia != original.QuantidadeDiasRecorrencia ||
                    novo.ReenviarNotificacaoInscrito != original.ReenviarNotificacaoInscrito ||
                    novo.Temporalidade != original.Temporalidade;
        }

        public ConfigurarNotificacaoVO BuscarConfiguracaoNotificacao(long seqConfiguracaoNotificacao)
        {
            var spec = new SMCSeqSpecification<ProcessoConfiguracaoNotificacao>(seqConfiguracaoNotificacao);
            var configuracaoNotificacao = this.SearchProjectionBySpecification(spec,
                            x => new ConfigurarNotificacaoVO
                            {
                                Seq = x.Seq,
                                SeqProcesso = x.SeqProcesso,
                                SeqTipoNotificacao = x.SeqTipoNotificacao,
                                EnvioAutomatico = x.EnvioAutomatico,
                                ConfiguracoesEmail = x.ConfiguracoesIdioma.Select(f => new ConfiguracaoNotificacaoIdiomaVO
                                {
                                    Idioma = f.ProcessoIdioma.Idioma,
                                    ConfiguracaoNotificacao = new ConfiguracaoNotificacaoEmailData()
                                    {
                                        Seq = f.SeqConfiguracaoTipoNotificacao
                                    }
                                }).ToList()
                            }).FirstOrDefault();

            configuracaoNotificacao.DescricaoTipoNotificacao = NotificacaoService.BuscarTipoNotificacao(configuracaoNotificacao.SeqTipoNotificacao).Descricao;

            return configuracaoNotificacao;
        }


    }
}
