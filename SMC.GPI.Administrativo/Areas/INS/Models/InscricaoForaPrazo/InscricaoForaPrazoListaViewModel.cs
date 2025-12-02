using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoForaPrazoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }        

        [SMCSortable(true)]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        public DateTime DataInicio { get; set; }

        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        [SMCSortable(true)]
        public DateTime DataFim { get; set; }

        public DateTime? DataVencimento { get; set; }
    }
}