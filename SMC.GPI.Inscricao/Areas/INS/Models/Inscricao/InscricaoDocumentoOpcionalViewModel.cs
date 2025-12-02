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
    public class InscricaoDocumentoOpcionalViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long? SeqDocumentoRequerido { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCHidden]
        public string DescricaoTipoDocumento { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        public string DescricaoTipoDocumentoApresentacao { get { return this.DescricaoTipoDocumento; } }

        [SMCSize(SMCSize.Grid8_24)]        
        [SMCMaxLength(100)]
        public string DescricaoArquivoAnexado { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCSize(SMCSize.Grid4_24)]        
        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880,
            ActionDownload = "DownloadArquivo", ControllerDownload = "Home",
            AreaDownload = "")]
        public SMCUploadFile ArquivoAnexado { get; set; }
    }
}