using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.DataAnnotations;

namespace SMC.GPI.Inscricao.Models
{
    public class InscricoesFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {        
        public long SeqInscrito { get; set; }

        [SMCFilterKey]
        public long? SeqProcesso { get; set; }

        public string TokenResource { get; set; }
        //public bool? Ingresso { get; set; }

    }
}

