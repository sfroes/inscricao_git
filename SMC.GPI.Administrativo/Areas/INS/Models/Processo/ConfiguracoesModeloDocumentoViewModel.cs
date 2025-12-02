using SMC.Framework;
using SMC.Framework.Caching;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.ServiceModel.Configuration;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfiguracoesModeloDocumentoViewModel : SMCViewModelBase, ISMCMappable
    {

        [SMCRequired]
        [SMCSelect("TiposDocumento")]
        [SMCDependency("SeqTipoProcesso", "BuscarTiposDocumentoSelect", "Processo", true)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqTipoDocumento { get; set; }

        [SMCRequired]
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMapForceFromTo]
        public bool? AssinaturaEletronica { get; set; }

        [SMCSelect("ConfiguracoesAssinaturaGad")]
        [SMCDependency("SeqUnidadeResponsavel", "BuscarConfiguracoesAssinaturaGadSelect", "Processo", true)]
        [SMCConditionalRequired(nameof(AssinaturaEletronica), true)]
        [SMCConditionalReadonly(nameof(AssinaturaEletronica), SMCConditionalOperation.Equals, false, null)]
        [SMCDependency(nameof(AssinaturaEletronica), "BuscarConfiguracoesAssinaturaGadSelect", "Processo", false)]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCCssClass("margin-bottom1")]
        public string TokenConfiguracaoDocumentoGad { get; set; }

        [SMCRequired]
        [SMCRadioButtonList()]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMapForceFromTo]
        public bool? RequerCheckin { get; set; }

        [SMCRequired]
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMapForceFromTo]
        public bool? ExibeDocumentoHome { get; set; }


        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly(nameof(ExibeDocumentoHome),SMCConditionalOperation.Equals, false, null)]
        [SMCConditionalRequired(nameof(ExibeDocumentoHome), true)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataInicioDocumentoHome { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly(nameof(ExibeDocumentoHome), SMCConditionalOperation.Equals, false, null)]        
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCMinDate(nameof(DataInicioDocumentoHome))]
        public DateTime? DataFimDocumentoHome { get; set; }

        [SMCHidden]
        public long? SeqArquivoModelo { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCFile(AllowedFileExtensions = new string[] { "dotx" },
            ActionDownload = "DownloadDocumento",
            ControllerDownload = "Processo",
            AreaDownload = "INS")]
        public SMCUploadFile ArquivoModelo { get; set; }
    }
}