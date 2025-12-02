using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AssertCobrarOfertaTaxasViewModel : SMCViewModelBase
    {
        public string Resource { get; set; }

        public List<string> ListaTaxas { get; set; }

        public string Taxas
        {
            get
            {
                if (ListaTaxas != null)
                    return string.Join(", ", ListaTaxas);
                return null;
            }
        }
    }
}