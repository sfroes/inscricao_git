using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS
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
            context.MapRoute(
                "INS_default",
                "INS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
