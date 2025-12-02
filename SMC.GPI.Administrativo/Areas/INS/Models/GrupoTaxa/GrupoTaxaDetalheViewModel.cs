using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoTaxaDetalheViewModel : SMCViewModelBase, ISMCMappable
    {       

        [SMCSelect("Taxas")]
        [SMCRequired]        
        [SMCSize(SMCSize.Grid10_24)]
        public long SeqTaxa { get; set; }

        [SMCHidden]
        public long SeqGrupoTaxa { get; set; }

        [SMCHidden]
        public long Seq { get; set; }

    }
}