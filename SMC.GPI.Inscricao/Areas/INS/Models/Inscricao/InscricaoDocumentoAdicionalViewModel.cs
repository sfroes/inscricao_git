using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoDocumentoAdicionalViewModel : SMCViewModelBase, ISMCMappable
	{
		[SMCHidden]
		public long Seq { get; set; }

        [SMCSelect("DocumentosAdicionaisUpload")]
		[SMCSize(SMCSize.Grid8_24)]
        //[SMCCssClass("col-40-mob col-40-xs col-40-ms", true)]
        [SMCRequired]
		public long? SeqDocumentoRequerido { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        //[SMCCssClass("col-32-mob col-32-xs col-28-ms", true)]
        [SMCMaxLength(100)]        
        public string DescricaoArquivoAnexado { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

		[SMCSize(SMCSize.Grid4_24)] 
        //[SMCCssClass("col-16-mob col-16-xs col-18-ms", true)]
        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880,
            ActionDownload = "DownloadArquivo", ControllerDownload = "Home",
            AreaDownload = "")]
        [SMCRequired]
        public SMCUploadFile ArquivoAnexado { get; set; } 
	}
}