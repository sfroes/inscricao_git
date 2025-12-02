using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaFiltroViewModel : SMCLookupFilterViewModel
    {
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCFilter]
        [SMCHidden]
        public long? SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long? SeqItemHierarquiaOferta { get; set; }

        [SMCDescription]
        [SMCHidden]
        public string Nome { get; set; }
    }
}
