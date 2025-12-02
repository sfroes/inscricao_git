using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoDocumentoFiltroViewModel : SMCPagerViewModel
    {
        [SMCFilter]
        [SMCMaxLength(255)]
        [SMCSize(SMCSize.Grid14_24)]
        public string Descricao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCSelect]
        public TipoEmissao? TipoEmissao { get; set; }

    }
}
