using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupItemGrupoOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }

        [SMCDescription]
        public string Nome { get; set; }

        [SMCMapProperty("SeqHierarquiaOfertaPai")]
        public long? SeqPai { get; set; }

        public bool EOferta { get; set; }

        [SMCHidden]
        public bool PossuiGrupo { get; set; }

        public string NomeGrupoOferta { get; set; }
    }
}
