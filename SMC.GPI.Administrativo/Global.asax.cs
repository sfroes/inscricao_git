using RazorGenerator.Mvc;
using SMC.Framework.Fake;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Global;
using SMC.Framework.UI.Mvc.Resources;
using SMC.Inscricoes.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace SMC.GPI.Administrativo
{
    public class MvcApplication : SMCHttpApplication
    {
        protected override void OnStart()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Estragégias do fake 
            FakeConfig.RegisterStrategies(SMCFakeStrategyConfiguration.Strategies);

            // Configurações para dynamic 
            ControllerBuilder.Current.SMCRegisterFactory("SMC.Inscricoes");

            SMCEmbeddedVirtualPathProvider.RegisterAllAssemblies();


            //var engine = new PrecompiledMvcEngine(this.GetType().Assembly);
            //var engine = new PrecompiledMvcEngine(typeof(CSharpRazorViewEngine).Assembly);

            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(engine);
            //ViewEngines.Engines.Add(new RazorViewEngine());

            //// StartPage lookups are done by WebPages. 
            //VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            //var engine = new PrecompiledMvcEngine(typeof(CSharpRazorViewEngine).Assembly);
            ////var smcDynamic = new PrecompiledMvcEngine(this.GetType().Assembly);
            //// Se utilizar mais de uma Engine de View então retire as duas linhas abaixo
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(engine);
            //ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            //VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            // var engine = new PrecompiledMvcEngine(this.GetType().Assembly);
            //// var engine = new PrecompiledMvcEngine(typeof(CSharpRazorViewEngine).Assembly);

            // // Se utilizar mais de uma Engine de View então retire as duas linhas abaixo
            // ViewEngines.Engines.Clear();
            // ViewEngines.Engines.Add(engine);
            // ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            // // StartPage lookups are done by WebPages. 
            // VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);

            //var smcDynamic = new CSharpCompiledViewEngine(this.GetType().Assembly);
            //// Se utilizar mais de uma Engine de View então retire as duas linhas abaixo
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(smcDynamic);
            //ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            // StartPage lookups are done by WebPages. 
            //VirtualPathFactoryManager.RegisterVirtualPathFactory(smcDynamic);

        }
    }
}