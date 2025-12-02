using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupInscritoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        [SMCKey]
        public long SeqInscrito { get; set; }

        [SMCDescription]
        public string Inscrito { get; set; }

        public DateTime DataNascimento { get; set; }
        
        [SMCCpf]
        public string Cpf { get; set; }

        public string Passaporte { get; set; }
    }
}
