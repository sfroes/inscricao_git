using SMC.Framework.Fake;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Resources;
using SMC.Inscricoes.Common;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMC.GPI.Inscricao
{
    public class MvcApplication : SMCHttpApplication
    {
        protected override void OnStart()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

			// Estragégias do fake 
			FakeConfig.RegisterStrategies(SMCFakeStrategyConfiguration.Strategies); 

            // Configurações para dynamic 
            ControllerBuilder.Current.SMCRegisterFactory("SMC.Inscricoes");

            SMCEmbeddedVirtualPathProvider.RegisterAllAssemblies();
        }
    }
}