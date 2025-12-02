using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoDocumentoObrigatorioViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqDocumentoRequerido { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCHidden]
        public string DescricaoTipoDocumento { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCRequired]
        public string DescricaoTipoDocumentoApresentacao { get { return this.DescricaoTipoDocumento; } }

        [SMCSize(SMCSize.Grid8_24)]
        //[SMCCssClass("col-32-mob col-32-xs col-28-ms", true)]
        [SMCMaxLength(100)]
        public string DescricaoArquivoAnexado { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        //[SMCCssClass("col-16-mob col-16-xs col-18-ms", true)]
        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880,
            ActionDownload = "DownloadArquivo", ControllerDownload = "Home",
            AreaDownload = "")]
        [SMCConditionalReadonly(nameof(EntregaPosterior),true)]
        [SMCConditionalRequired(nameof(EntregaPosterior),false)]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [SMCHidden]
        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCHidden]
        public DateTime? DataLimiteEntrega { get; set; }
                
        [SMCSize(SMCSize.Grid1_24)]        
        public bool EntregaPosterior { get; set; }
    }
}