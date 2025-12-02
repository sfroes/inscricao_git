using RazorGenerator.Mvc;
using SMC.Formularios.UI.Mvc;
using SMC.Framework.UI.Mvc.Global;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SMC.Inscricoes.ReportHost.RazorGeneratorMvcStart), "Start")]

namespace SMC.Inscricoes.ReportHost
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            Configure();
        }

        public static void Configure()
        {
            bool view = false;
#if DEBUG
            view = HttpContext.Current.Request.IsLocal;
#endif

            var framework = new PrecompiledMvcEngine(typeof(CSharpRazorViewEngine).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            var sistema = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            var formulario = new PrecompiledMvcEngine(typeof(FormulariosExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(framework);
            ViewEngines.Engines.Add(sistema);
            ViewEngines.Engines.Add(formulario);
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            // StartPage lookups are done by WebPages.
            VirtualPathFactoryManager.RegisterVirtualPathFactory(framework);
        }
    }
}