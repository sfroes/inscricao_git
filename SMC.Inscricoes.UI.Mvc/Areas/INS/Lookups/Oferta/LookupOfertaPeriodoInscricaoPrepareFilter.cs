using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.Oferta
{
    public class LookupOfertaPeriodoInscricaoPrepareFilter : ISMCFilter<LookupOfertaPeriodoInscricaoFiltroViewModel>
    {
        public LookupOfertaPeriodoInscricaoFiltroViewModel Filter(SMCControllerBase controllerBase, LookupOfertaPeriodoInscricaoFiltroViewModel filter)
        {
            var processoService = controllerBase.Create<IProcessoService>();
            var expanded = processoService.ExibeArvoreFechada(filter.SeqProcesso);

            controllerBase.ControllerContext.HttpContext.Session["TreeExpanded"] = expanded;
            return filter;
        }
    }
}
