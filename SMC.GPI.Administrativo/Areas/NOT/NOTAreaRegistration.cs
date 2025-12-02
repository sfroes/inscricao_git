using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.NOT
{
    public class NOTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NOT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NOT_default",
                "NOT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
