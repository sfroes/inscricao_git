using SMC.EstruturaOrganizacional.UI.Mvc.Areas.ESO.Lookups;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelCentroCustoViewModel : SMCViewModelBase, ISMCMappable
    {
        public UnidadeResponsavelCentroCustoViewModel()
        {
            Ativo = true;
        }

        [SMCRequired]
        [CentroCustoLookup]
        [SMCSize(SMCSize.Grid18_24)]
        public CentroCustoViewModel CentroCusto { get; set; }

        [SMCRequired]
        [SMCSelect]
        [SMCSize(SMCSize.Grid3_24)]
        public bool Ativo { get; set; }
    }
}