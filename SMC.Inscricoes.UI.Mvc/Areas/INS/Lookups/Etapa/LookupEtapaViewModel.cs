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
    public class LookupEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCDescription]
        public string Processo { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Etapa { get; set; }

        public string Configuracao { get; set; }
    }
}
