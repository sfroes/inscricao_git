using RazorGenerator.Mvc;
using SMC.Framework.Logging;
using SMC.Framework.UI.Mvc.Global;
using SMC.Inscricoes.UI.Mvc;
using SMC.Seguranca.UI.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SMC.GPI.Inscricao.RazorGeneratorMvcStart), "Start")]

namespace SMC.GPI.Inscricao
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
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

            var uiMVC = new PrecompiledMvcEngine(typeof(ExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            var uiSeguranca = new PrecompiledMvcEngine(typeof(SegurancaExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(framework);
            ViewEngines.Engines.Add(sistema);
            ViewEngines.Engines.Add(uiMVC);
            ViewEngines.Engines.Add(uiSeguranca);
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(framework);
        }
    }
}
