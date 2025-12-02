using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class OpcoesInscricaoViewModel : SMCViewModelBase
    {
        [SMCSize(Framework.SMCSize.Grid3_24)]
        public string Opcao { get; set; }

        [SMCSize(Framework.SMCSize.Grid14_24)]
        public string Oferta { get; set; }

        [SMCSize(Framework.SMCSize.Grid7_24)]
        public string Situacao { get; set; }
    }
}