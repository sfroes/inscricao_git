using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }

        [SMCDescription]
        [SMCMapProperty("DescricaoCompleta")]
        public string Descricao { get; set; }

        [SMCMapProperty("SeqHierarquiaOfertaPai")]
        public long SeqPai { get; set; }

        public bool IsLeaf { get; set; }

        public string DescricaoComplementar { get; set; }
    }
}
