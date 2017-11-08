using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MechCrud
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Mech",
               url: "mech/{id}",
               defaults: new { controller = "Mech", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "MechEdit",
                url: "Mech/edit/{id}",
                defaults: new { controller = "Mech", action = "Edit", id = UrlParameter.Optional }
            );
        }
    }
}
