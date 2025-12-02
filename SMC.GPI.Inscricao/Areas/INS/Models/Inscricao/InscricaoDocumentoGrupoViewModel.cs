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
    public class InscricaoDocumentoGrupoViewModel : SMCViewModelBase, ISMCMappable
	{
		[SMCHidden]
		public long Seq { get; set; }

        [SMCSize(SMCSize.Grid8_24)]        
        [SMCRequired]
        [SMCSelect("DocumentosRequeridosGrupo")]
		public long? SeqDocumentoRequerido { get; set; }

        //Lista de documentos do grupo 
        public List<SMCDatasourceItem> DocumentosRequeridosGrupo { get; set; }

        [SMCHidden]
        public long SeqGrupoDocumentoRequerido { get; set; }
        
        [SMCSize(SMCSize.Grid4_24)]
        [SMCHidden]
        public string DescricaoGrupoDocumentos { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        //[SMCCssClass("col-32-mob col-32-xs col-28-ms", true)]
        [SMCMaxLength(100)]        
        public string DescricaoArquivoAnexado { get; set; }

		[SMCSize(SMCSize.Grid3_24)] 
        //[SMCCssClass("col-16-mob col-16-xs col-18-ms", true)]
        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880,
            ActionDownload = "DownloadArquivo", ControllerDownload = "Home",
            AreaDownload = "")]
        [SMCConditionalReadonly(nameof(EntregaPosterior), true)]
        [SMCConditionalRequired(nameof(EntregaPosterior), false)]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [SMCHidden]
        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCHidden]
        public DateTime? DataLimiteEntrega { get; set; }

        [SMCSize(SMCSize.Grid1_24)]
        [SMCHideLabel]
        public bool EntregaPosterior { get; set; }
    }
}