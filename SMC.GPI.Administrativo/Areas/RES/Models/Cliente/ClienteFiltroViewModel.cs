using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    /// <summary>
    /// Classe para filtro do cliente
    /// </summary>
    public class ClienteFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(long.MaxValue)]        
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)] 
        public string Nome { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public string Sigla { get; set; }
    }
}