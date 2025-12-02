using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupProcessoViewModel : SMCViewModelBase
    {
        [SMCKey]        
        public long Seq { get; set; }

        [SMCDescription]
        public string Descricao { get; set; }

        public string UnidadeResponsavel { get; set; }

        public string TipoProcesso { get; set; }

        public int AnoReferencia { get; set; }

        public int SemestreReferencia { get; set; }
    }
}
