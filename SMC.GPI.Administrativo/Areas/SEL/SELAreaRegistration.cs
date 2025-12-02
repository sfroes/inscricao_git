using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.SEL
{
    public class SELAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SEL";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SEL_default",
                "SEL/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}