using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    /// <summary>
    /// Nao existe no Inscricao somente no Administrativo.
    /// Utilizado como ponto de entrada no HealthCheck. 
    /// </summary>
    public class TipoItemHierarquiaOfertaDynamicModel : SMCDynamicViewModel
    {
        [SMCKey]
        [SMCOrder(0)]
        [SMCReadOnly(SMCViewMode.Edit | SMCViewMode.Insert)]
        [SMCRequired]
        [SMCSortable]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxValue(long.MaxValue)]
        public override long Seq { get; set; }

    }
}