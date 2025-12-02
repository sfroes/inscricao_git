using System.Web.Http;
using System.Web.Mvc;

namespace SMC.Inscricoes.ReportHost.Areas.INS
{
    public class INSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "INS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.Routes.MapHttpRoute(
                "INS_WebApiRoute",
                "INS/api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            context.MapRoute(
                "INS_default",
                "INS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
