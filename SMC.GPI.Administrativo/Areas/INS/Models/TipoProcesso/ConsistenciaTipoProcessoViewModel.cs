using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsistenciaTipoProcessoViewModel : SMCViewModelBase
    {
        [SMCSelect]
        [SMCUnique]
        [SMCRequired]
        [SMCSize(SMCSize.Grid22_24)]
        public TipoConsistencia TipoConsistencia { get; set; }
    }
}