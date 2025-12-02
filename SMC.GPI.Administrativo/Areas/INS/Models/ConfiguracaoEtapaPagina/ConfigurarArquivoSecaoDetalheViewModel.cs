using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfigurarArquivoSecaoDetalheViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqArquivo { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        [SMCHidden]
        public long SeqSecaoPaginaSGF { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid3_24)]
        public string Ordem { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        public string NomeLink { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid9_24)]
        public string Descricao { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCFile(HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 26214400,
            ActionDownload = "DownloadDocumento",ControllerDownload="AcompanhamentoProcesso", AreaDownload = "INS")]
        public SMCUploadFile Arquivo { get; set; }



 
    }
}