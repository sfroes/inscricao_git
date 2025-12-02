using System.Web;
using System.Web.Mvc;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Filters;
using SMC.Framework.UI.Mvc.Security;
namespace SMC.GPI.Inscricao
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SMCHandleErrorAttribute());

            // Filtro para só permitir acesso as páginas com autorização de segurança.
            filters.Add(new SMCAuthorizeAttribute() { Order = 999 });

            // Filtro para monitorar a navegação pelo site
            filters.Add(new SMCAccessMonitoringAttribute());

            // Filtro para o botão voltar
            filters.Add(new SMCBackButtonFilterAttribute());
        }
    }
}