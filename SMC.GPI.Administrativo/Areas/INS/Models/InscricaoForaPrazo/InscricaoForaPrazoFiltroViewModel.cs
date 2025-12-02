using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoForaPrazoFiltroViewModel : SMCPagerViewModel
    {
        [SMCHidden]
        [SMCFilterKey]
        public long SeqProcesso { get; set; }        

        [SMCFilter]
        [SMCSize(Framework.SMCSize.Grid3_24)]
        [SMCConditionalRequired("DataFim", SMCConditionalOperation.NotEqual, "", null)]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]    
        public DateTime? DataInicio { get; set; }

        [SMCFilter]
        [SMCSize(Framework.SMCSize.Grid3_24)]
        [SMCMinDate("DataInicio")]
        [SMCConditionalRequired("DataInicio", SMCConditionalOperation.NotEqual, "", null)]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]        
        public DateTime? DataFim { get; set; }

        [SMCFilter]
        [SMCSize(Framework.SMCSize.Grid11_24)]
        [SMCFocus]
        public string Inscrito { get; set; }
    }
}