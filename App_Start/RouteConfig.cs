using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AssetApp
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
                "Contract",                                              // Route name
                "{controller}/{action}/{id}/{appname}",                           // URL with parameters
                new { controller = "Contract", action = "Index", id= "", appname = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "Version",                                              // Route name
                "{controller}/{action}/{id}/{appname}",                           // URL with parameters
                new { controller = "Info", action = "Version", id = "", appname = "" }  // Parameter defaults
            );
        }
    }
}
