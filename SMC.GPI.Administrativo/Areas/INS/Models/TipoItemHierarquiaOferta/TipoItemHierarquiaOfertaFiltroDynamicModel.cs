using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoItemHierarquiaOfertaFiltroDynamicModel : SMCDynamicFilterViewModel
    {
        [SMCFilter]
        [SMCOrder(0)]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxValue(long.MaxValue)]
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDescription]
        [SMCSortable(true, true)]
        public string Descricao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid9_24)]
        public string Token { get; set; }
    }
}