using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.GPI.Administrativo.Areas.INS.Views.TipoItemHierarquiaOferta.App_LocalResources;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
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

        [SMCRequired]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDescription]
        [SMCSortable(true, true)]
        public string Descricao { get; set; }

        [SMCRegularExpression(REGEX.TOKEN, FormatErrorResourceKey = nameof(MetadataResource.MSG_Token_Expression_Error))]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCRequired]
        public string Token { get; set; }

        #region [ Configuração ]

        public override void ConfigureDynamic(ref SMCDynamicOptions options)
        {
            options.Tokens(tokenInsert: UC_INS_001_06_01.MANTER_TIPO_ITEM_HIERARQUIA_OFERTA,
                           tokenEdit: UC_INS_001_06_01.MANTER_TIPO_ITEM_HIERARQUIA_OFERTA,
                           tokenRemove: UC_INS_001_06_01.MANTER_TIPO_ITEM_HIERARQUIA_OFERTA,
                           tokenList: UC_INS_001_06_01.MANTER_TIPO_ITEM_HIERARQUIA_OFERTA);
        }

        #endregion [ Configuração ]
    }
}