using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupHierarquiaOfertaViewModel : SMCViewModelBase, ISMCLookupData
    {
        [SMCKey]
        [SMCHidden]
        public long? Seq { get; set; }

        public string TipoItemHierarquiaOferta { get; set; }

        public string HierarquiaOferta { get; set; }

        [SMCHidden(SMCViewMode.List)]
        [SMCDescription]
        public string Descricao
        {
            get
            {
                if (TipoItemHierarquiaOferta == null || HierarquiaOferta == null)
                    return null;
                return string.Format("{0}: {1}", TipoItemHierarquiaOferta, HierarquiaOferta);
            }
            set { }
        }
    }
}
