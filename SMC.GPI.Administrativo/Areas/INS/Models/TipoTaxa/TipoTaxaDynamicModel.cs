using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Inscricoes.Common.Areas.INS.Constants;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoTaxaDynamicModel : SMCDynamicViewModel
    {
        [SMCFilter(true)]
        [SMCKey]
        [SMCOrder(0)]
        [SMCReadOnly(SMCViewMode.Edit | SMCViewMode.Insert)]
        [SMCRequired]
        [SMCSortable]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxValue(long.MaxValue)]
        public override long Seq { get; set; }

        [SMCRequired]
        [SMCMaxLength(100)]
        [SMCFilter(true)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDescription]
        [SMCSortable(true, true)]
        public string Descricao { get; set; }

        #region [ Configuração ]

        public override void ConfigureDynamic(ref SMCDynamicOptions options)
        {
            options.Tokens(tokenInsert: UC_INS_001_10_01.MANTER_TIPO_TAXA,
                           tokenEdit: UC_INS_001_10_01.MANTER_TIPO_TAXA,
                           tokenRemove: UC_INS_001_10_01.MANTER_TIPO_TAXA,
                           tokenList: UC_INS_001_10_01.MANTER_TIPO_TAXA);
        }

        #endregion [ Configuração ]
    }
}