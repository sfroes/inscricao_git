using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using SMC.GPI.Administrativo.Areas.INS.Views.AcompanhamentoProcesso.App_LocalResources;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class DocumentoEntregueViewModel : SMCViewModelBase, ISMCMappable
    {
        #region [ DataSouces ]

        //[SMCDataSource]
        //[SMCIgnoreProp]
        //[SMCHidden]
        //public List<SMCDatasourceItem> SolicitacoesEntregaDocumento { get; set; }

        #endregion [ DataSouces ]

        public bool Opcional { get; set; }

        [SMCHidden]
        [SMCKey]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqDocumentoRequerido { get; set; }

        [SMCSize(SMCSize.Grid22_24)]
        public string DescricaoTipoDocumento { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        //[SMCSelect(nameof(SolicitacoesEntregaDocumento))]
        [SMCSelect(AjaxLoadAction = "BuscarSituacoesEntregaDocumento",
            AjaxLoadArea = "INS",
            AjaxLoadController = "AcompanhamentoProcesso",
            AjaxLoadProperties = new string[] { nameof(SeqDocumentoRequerido), nameof(SeqInscricao), nameof(SituacaoEntregaDocumento) })]

        public SituacaoEntregaDocumento SituacaoEntregaDocumentos { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        private SituacaoEntregaDocumento? situacaoEntregaDocumentoAntiga;

        [SMCHidden]
        public SituacaoEntregaDocumento SituacaoEntregaDocumentoAntiga
        {
            get
            {
                if (!situacaoEntregaDocumentoAntiga.HasValue)
                {
                    situacaoEntregaDocumentoAntiga = SituacaoEntregaDocumento;
                }
                return situacaoEntregaDocumentoAntiga.Value;
            }
            set
            {
                situacaoEntregaDocumentoAntiga = value;
            }
        }

        [SMCHidden]
        public bool TipoDocumentoPermiteVariosArquivos { get; set; }

        private DateTime? _dataEntrega;

        [SMCSize(SMCSize.Grid3_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCRequired]
        [SMCMapForceFromTo]
        public DateTime? DataEntrega { get; set; }
        
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        [SMCRequired]
        public VersaoDocumento VersaoDocumento { get; set; }

        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        public FormaEntregaDocumento? FormaEntregaDocumento { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCSelect]
        [SMCRequired]
        public FormaEntregaDocumento FormaEntrega { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCLink("CarregarDocumento", "AcompanhamentoProcesso", SMCLinkTarget.NewWindow, "seqArquivo")]
        public string LinkArquivo { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public string DescricaoArquivoAnexado { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        public string SituacaoInscricao { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCFile(ActionDownload = "DownloadDocumento", ControllerDownload = "AcompanhamentoProcesso",
            AreaDownload = "INS", DisplayFilesInContextWindow = true, MaxFileSize = 5242880)]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [SMCHidden]
        public bool ExibirInformacaoExibirObservacaoParaInscrito { get; set; }

        [SMCDisplay(DisplayAsInstructions = true)]
        [SMCSize(SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24)]
        [SMCCssClass("smc-gpi-documento-entregue")]
        [SMCHideLabel]
        [SMCConditionalDisplay(nameof(ExibirInformacaoExibirObservacaoParaInscrito), SMCConditionalOperation.Equals, true)]
        public string InformacaoExibirObservacaoParaInscrito { get { return UIResource.MSG_ExibirObservacaoParaInscrito; } }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxLength(100)]
        public string Observacao { get; set; }

        [SMCHidden]
        public bool ExibirExibirObservacaoParaInscrito { get; set; }



        [SMCSize(SMCSize.Grid3_24)]
        [SMCConditionalDisplay(nameof(ExibirExibirObservacaoParaInscrito), SMCConditionalOperation.Equals,true)]
        public bool? ExibirObservacaoParaInscrito { get; set; }

        [SMCHidden]
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24)]
        public bool EntregaRegistrada
        {
            get
            {
                return this.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido;
            }
            set
            {
                if (value)
                {
                    this.SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido;
                }
                else
                {
                    if (FormaEntrega == SMC.DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Upload)
                    {
                        this.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoValidacao;
                    }
                    else
                    {
                        this.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega;
                    }
                }
            }
        }

        [SMCHidden]
        public bool DocumentacaoEntregue { get; set; }

        public string ActionForm
        {
            get
            {
                if (this.EntregaRegistrada)
                    return "DesfazerEntrega";
                else
                    return "RealizarEntrega";
            }
        }

        public string TextoBotao
        {
            get
            {
                if (this.EntregaRegistrada)
                    return "Botao_Desfazerentrega";
                else
                    return "Botao_Registrarentrega";
            }
        }

        [SMCHidden]
        public bool PermiteVarios { get; set; }

        [SMCHidden]
        public long SeqTipoDocumento { get; set; }

        [SMCHidden]
        public Sexo Sexo { get; set; }

        [SMCDetail(SMCDetailType.Modal)]
        public SMCMasterDetailList<InscricaoDocumentoViewModel> InscricaoDocumentos { get; set; }

        [SMCHidden]
        public bool PermiteEntregaPosterior { get; set; }

        [SMCHidden]
        public bool Obrigatorio { get; set; }

        [SMCHidden]
        public bool ObrigatorioUpload { get; set; }

        [SMCReadOnly]
        [SMCConditionalDisplay(nameof(PermiteEntregaPosterior), SMCConditionalOperation.Equals,true)]
        public string EntregaPosterior { get; set; }


    }
}