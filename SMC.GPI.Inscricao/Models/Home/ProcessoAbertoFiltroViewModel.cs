using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Inscricao.Models
{
    public class ProcessoAbertoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCSize(SMCSize.Grid23_24, SMCSize.Grid23_24, SMCSize.Grid23_24, SMCSize.Grid23_24)]
        public string DescricaoProcesso { get; set; }

        [SMCHidden]
        public long? SeqProcesso { get; set; }

        [SMCFilter]
        [SMCHidden]
        public SMCLanguage Idioma { get; set; }

    }
}

