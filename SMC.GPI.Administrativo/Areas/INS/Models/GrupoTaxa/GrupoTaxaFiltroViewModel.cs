using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoTaxaFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {

        public CabecalhoProcessoViewModel Cabecalho { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid2_24)]
        public long SeqProcesso { get; set; }        

    }
}