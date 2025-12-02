using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SMC.Framework.UI.Mvc;
using SMC.Formularios.UI.Mvc;
using SMC.Seguranca.UI.Mvc;
using SMC.Localidades.UI.Mvc;
using SMC.Seguranca.UI.Mvc.Controllers;

namespace SMC.GPI.Inscricao
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Recursos/{*pathInfo}");
            routes.IgnoreRoute("Images/{*pathInfo}");         

            // configuração de rota para componente de profile do sas
            routes.RegistrarRotaProfile();

            // configuração de rota para uso de formulários do sgf
            routes.RegistrarRotaSGF();

            // configuração de rota para componentes de endereço e telefone
            routes.RegistrarRotaLocalidade();

            // configuração da rota do FileUpload 
            routes.SMCMapUploadRoute(
                name: "FileUpload",
                url: "FileUpload",
                defaults: new { controller = "FileUpload", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ); 

        }
    }
}