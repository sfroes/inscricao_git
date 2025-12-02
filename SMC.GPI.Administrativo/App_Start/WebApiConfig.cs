using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SMC.Framework.UI.Mvc.Html;

namespace SMC.GPI.Administrativo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
