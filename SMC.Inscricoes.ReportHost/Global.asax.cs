using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Resources;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace SMC.Inscricoes.ReportHost
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : SMCHttpApplication
    {
        protected override void OnStart()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            SMCEmbeddedVirtualPathProvider.RegisterAllAssemblies(); ;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
        }
    }
}