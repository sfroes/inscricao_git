using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.RES
{
    public class RESAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RES";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RES_default",
                "RES/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
