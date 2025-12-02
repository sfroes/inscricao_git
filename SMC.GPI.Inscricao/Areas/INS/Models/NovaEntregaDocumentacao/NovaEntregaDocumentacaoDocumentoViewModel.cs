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
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.DataAnnotations;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class NovaEntregaDocumentacaoDocumentoViewModel : SMCViewModelBase, ISMCMappable
	{
		[SMCHidden]
		public long Seq { get; set; }

        [SMCHidden]
		public long SeqDocumentoRequerido { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexadoAnterior { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCHideLabel]
        [SMCReadOnly]
        public string DescricaoTipoDocumento { get; set; }

        [SMCMaxLength(30)]        
        public string DescricaoArquivoAnexado { get; set; }

        [SMCLegendItemDisplay(GenerateLabel = false)]
        [SMCGridLegend(DisplayDescription = false)]
        [SMCHideLabel]
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880,
            ActionDownload = "DownloadArquivo", ControllerDownload = "Home",
            AreaDownload = ""),
            ]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [SMCDisplay(DisplayAsInstructions = true)]
        [SMCCssClass("smc-gpi-documento-entregue")]
        [SMCHideLabel]
        public string Observacao { get; set; }
    }
}