using System;
using System.Collections.Generic;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.GPI.Inscricao.Models
{
    public class InscricaoProcessoItemViewModel : SMCViewModelBase
    {
        public InscricaoProcessoItemViewModel()
        {
            DescricaoOfertas = new List<string>();
        }

        public long SeqInscricao { get; set; }
        public long SeqProcesso { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqGrupoOferta { get; set; }

        public DateTime DataInscricao { get; set; }

        public string DescricaoSituacaoAtual { get; set; }

        public string TokenSituacaoAtual { get; set; }

        [SMCMapForceFromTo]
        public List<string> DescricaoOfertas { get; set; }

        public SMCLanguage IdiomaInscricao { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public string DescricaoLabelOferta { get; set; }

        public bool ConfiguracaoEtapaVigente { get; set; }

        public bool GrupoPossuiOfertaVigente { get; set; }

        public bool ProcessoCancelado { get; set; }

        public Guid UidProcesso { get; set; }

        public bool PermissaoInscricaoForaPrazo { get; set; }

        public bool ExisteDocumentoObrigatorioIndeferidoOuPendente { get; set; }

        public bool ExisteGrupoDocumentoIndeferidoOuPendente { get; set; }

        public bool PermiteNovaEntregaDocumentacao { get; set; }

        public SituacaoEtapa SituacaoEtapa { get; set; }

        public string PaginaAtualInscricao { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        public DateTime? DataEncerramentoProcesso { get; set; }

        public bool ProcessoEncerrado { get; set; }
        
        public bool HabilitaCheckin { get; set; }
        public bool ExibirBotaoEmitirDocumentacao { get; set; }
        public string DescricaoTipoDocumento { get; set; }
        public long SeqTipoDocumento { get; set; }
        public string MensagemInformativaBotaoEmitirDocumentacao
        {
            get
            {
                if (!ExibirBotaoEmitirDocumentacao)
                    return string.Empty;

                return string.Format(Views.Home.App_LocalResources.UIResource.Mensagem_Tooltip_Botao_Emitir_Documentacao, DescricaoTipoDocumento);
            }
        }

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        #region Membros auxiliares

        public bool InscricaoIniciada
        {
            get { return this.TokenSituacaoAtual.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA); }
        }

        public bool InscricaoCancelada => TokenSituacaoAtual.Equals(TOKENS.SITUACAO_INSCRICAO_CANCELADA);

        public bool ContinuarInscricaoDesabilitado
        {
            get
            {
                return (!(this.ConfiguracaoEtapaVigente && this.GrupoPossuiOfertaVigente) && !this.PermissaoInscricaoForaPrazo) ||
                        this.ProcessoCancelado || this.ProcessoEncerrado ||
                        this.SituacaoEtapa == SituacaoEtapa.EmManutencao || this.DataEncerramentoProcesso <= DateTime.Now;
            }
        }

        public bool TokenValidoExibirEntregaDocumentacao
        {
            get
            {
                return TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_FINALIZADA ||
                       TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_DEFERIDA ||
                       TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;
            }
        }

        public bool HabilitarEntregaDocumentacao
        {
            get
            {
                var dataVigente = DataPrazoNovaEntregaDocumentacao.HasValue &&
                                  DataPrazoNovaEntregaDocumentacao.Value >= DateTime.Now.Date;

                return (ExisteDocumentoObrigatorioIndeferidoOuPendente || ExisteGrupoDocumentoIndeferidoOuPendente) && 
                        PermiteNovaEntregaDocumentacao &&  dataVigente &&
                        TokenValidoExibirEntregaDocumentacao;
            }
        }

        public string MensagemInformativa
        {
            get
            {
                if (this.InscricaoIniciada)
                {
                    if ((!(this.ConfiguracaoEtapaVigente && this.GrupoPossuiOfertaVigente) && !this.PermissaoInscricaoForaPrazo) || this.ProcessoCancelado || this.ProcessoEncerrado)
                    {
                        this.classAlternativa = "smc-gpi-mensagem-botao";                        
                        return Views.Home.App_LocalResources.UIResource.ResourceManager.GetString("Mensagem_Informativa_Inscricao_Indisponivel_" + this.TokenResource);
                    }
                    else if (this.SituacaoEtapa == SituacaoEtapa.EmManutencao)
                    {
                        this.classAlternativa = "smc-gpi-mensagem-botao";
                        return Views.Home.App_LocalResources.UIResource.ResourceManager.GetString("Mensagem_Informativa_Inscricoes_Em_Manutencao_" + this.TokenResource);
                    }
                    else if (this.DescricaoOfertas.Count <= 0)
                    {
                        this.classAlternativa = "smc-gpi-lista-inscricoes-mensagem";
                        return string.Format(Views.Home.App_LocalResources.UIResource.ResourceManager.GetString("Mensagem_Informativa_Oferta_Nao_Selecionada"), this.DescricaoLabelOferta);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string MensagemInformativaEntregaDocumentos
        {
            get
            {
                var dataVigente = DataPrazoNovaEntregaDocumentacao.HasValue &&
                  DataPrazoNovaEntregaDocumentacao.Value >= DateTime.Now.Date;

                if ((ExisteDocumentoObrigatorioIndeferidoOuPendente || ExisteGrupoDocumentoIndeferidoOuPendente) && 
                    PermiteNovaEntregaDocumentacao && dataVigente &&
                    TokenValidoExibirEntregaDocumentacao)
                {
                    if (this.DataPrazoNovaEntregaDocumentacao.HasValue)
                    {
                        string dataLimite;
                        switch (this.IdiomaInscricao)
                        {
                            case SMCLanguage.Portuguese:
                            case SMCLanguage.French:
                            case SMCLanguage.German:
                            case SMCLanguage.Italian:
                            case SMCLanguage.Spanish:
                                dataLimite = this.DataPrazoNovaEntregaDocumentacao.Value.ToString("dd/MM/yyyy");
                                break;
                            case SMCLanguage.English:
                                dataLimite = this.DataPrazoNovaEntregaDocumentacao.Value.ToString("MM/dd/yyyy");
                                break;
                            default:
                                dataLimite = this.DataPrazoNovaEntregaDocumentacao.Value.ToString("dd/MM/yyyy");
                                break;
                        }

                        return string.Format(Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Entrega_Documentacao, dataLimite);
                    }
                    else
                    {
                        return string.Format(Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Entrega_Documentacao, "-");
                    }
                }
                else
                    return string.Empty;
            }
        }

        public string classAlternativa { get; set; }

        public bool HabilitarBotaoCheckin {  get; set; }

        public string MensagemInformativaCheckin { get; set; }

        public bool PosssuiTaxas { get; set; }

        public bool PermiteEmitirComprovante { get; set; }

        public long SeqArquivoComprovante { get; set; }

        public bool GestaoEventos { get; set; }

        #endregion Membros auxiliares

        #region Botões dinamicos
        public string BotaoContiuar { get; set; }
        public string BotaoContiuarTootip { get; set; }
        public bool HabilitarBotaoContinuar { get; set; }
        public string MensagemBotaoContinuar { get; set; }
        public string TokenResource { get; set; }

        #endregion
    }
}