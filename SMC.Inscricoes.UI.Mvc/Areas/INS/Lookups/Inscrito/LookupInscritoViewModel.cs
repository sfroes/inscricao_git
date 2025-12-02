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
    public class LookupInscritoViewModel : SMCViewModelBase, ISMCLookupData
    {        
        [SMCHidden]
        public long? Seq { get; set; }

        [SMCKey]
        [SMCHidden]
        public long SeqInscrito { get; set; }

        [SMCDescription]
        public string NomeDisplay
        {
            get { return string.IsNullOrEmpty(NomeSocial) || Nome.Contains('(') ? Nome : string.Format("{0} ({1})", NomeSocial, Nome ); }
            set { Nome = value; }
        }

        [SMCHidden]
        public string Nome { get; set; }

        [SMCHidden]
        public string NomeSocial { get; set; }

        public DateTime DataNascimento { get; set; }

        public string CPF { get; set; }

        public string NumeroPassaporte { get; set; }
    }
}
