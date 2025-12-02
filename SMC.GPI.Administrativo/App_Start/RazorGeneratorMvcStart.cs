using RazorGenerator.Mvc;
using SMC.Formularios.UI.Mvc;
using SMC.Framework.Logging;
using SMC.Framework.UI.Mvc.Global;
using SMC.Inscricoes.UI.Mvc;
using SMC.Notificacoes.UI.Mvc;
using SMC.Seguranca.UI.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SMC.GPI.Administrativo.RazorGeneratorMvcStart), "Start")]

namespace SMC.GPI.Administrativo
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

            var uiNot = new PrecompiledMvcEngine(typeof(NotificacaoExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            var uiSeguranca = new PrecompiledMvcEngine(typeof(SegurancaExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            var uiSGG = new PrecompiledMvcEngine(typeof(FormulariosExternalViews).Assembly)
            {
                UsePhysicalViewsIfNewer = view
            };

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(framework);
            ViewEngines.Engines.Add(sistema);
            ViewEngines.Engines.Add(uiMVC);
            ViewEngines.Engines.Add(uiSGG);
            ViewEngines.Engines.Add(uiNot);
            ViewEngines.Engines.Add(uiSeguranca);
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(framework);
        }
    }
}
