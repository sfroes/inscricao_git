using SMC.Framework.Security;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AcompanhamentoInscritoDynamicModel : SMCDynamicViewModel
    {
        [SMCKey]
        [SMCHidden]
        public override long Seq { get; set; }

        public override void ConfigureDynamic(ref SMCDynamicOptions options)
        {
            options
                .DisableInitialListing(true)
                .Javascript("reloadPagerAjaxDiv")
                .CssClass("smc-list-sem-rolagem")
                .Detail<AcompanhamentoInscritoListarDynamicModel>("_DetailList")
                .Tokens(tokenInsert: SMCSecurityConsts.SMC_DENY_AUTHORIZATION,
                         tokenList: UC_INS_004_01_01.PESQUISAR_INSCRITO)
                .Service<IAcompanhamentoInscritoService>(index: nameof(IAcompanhamentoInscritoService.BuscarInscrito));
        }
    }
}