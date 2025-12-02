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
    public class ConfigurarTextoSecaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }        

        [SMCSize(SMCSize.Grid8_24)]
        public string Pagina { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        public string Idioma { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        public string Secao { get; set; }
                
        [SMCHtml]
        [SMCMultiline]
        [SMCSize(SMCSize.Grid24_24)]
        public string Texto { get; set; }
    }
}