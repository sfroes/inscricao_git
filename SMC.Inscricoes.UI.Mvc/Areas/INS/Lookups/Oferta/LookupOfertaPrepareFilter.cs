using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.Oferta
{
    public class LookupOfertaPrepareFilter : ISMCFilter<LookupOfertaFiltroViewModel>
    {
        public LookupOfertaFiltroViewModel Filter(SMCControllerBase controllerBase, LookupOfertaFiltroViewModel filter)
        {
            var processoService = controllerBase.Create<IProcessoService>();
            var expanded = !processoService.ExibeArvoreFechada(filter.SeqProcesso);

            controllerBase.ControllerContext.HttpContext.Session["TreeExpanded"] = expanded;
            return filter;
        }
    }
}
