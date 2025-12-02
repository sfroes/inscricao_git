using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupInscritoFiltroViewModel : SMCLookupFilterViewModel, ISMCLookupData
    {
        [SMCDescription]
        [SMCMaxLength(100)]
        [SMCSize(Framework.SMCSize.Grid13_24)]
        public string Inscrito { get; set; }

        [SMCSize(Framework.SMCSize.Grid5_24)]
        [SMCCpf]
        public string CPF { get; set; }

        [SMCMaxLength(30)]
        [SMCSize(Framework.SMCSize.Grid6_24)]
        public string Passaporte { get; set; }

        [SMCSize(Framework.SMCSize.Grid12_24)]
        [LookupProcesso]
        public GPILookupViewModel Processo { get; set; }

        [SMCSelect()]
        [SMCDependency("Processo", "BuscarSituacoes", "LookupInscrito", true, Remote = true)]
        [SMCSize(Framework.SMCSize.Grid6_24)]
        public long? Situacao { get; set; }
    }
}
