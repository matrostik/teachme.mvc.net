using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeachMe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           // routes.MapRoute(
           //name: "SearchMap",
           //url: "Search/Map/{category}/{city}",
           //defaults: new { controller = "Search", action = "Index", category = UrlParameter.Optional, city = UrlParameter.Optional }
           //);
           // routes.MapRoute(
           // name: "Search",
           // url: "Search/{page}",
           // defaults: new { controller = "Search", action = "Index", page = UrlParameter.Optional }
           //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Search1",
            url: "Search/Index/{page}/{category}/{city}/{firstName}/{lastName}/",
            defaults: new { controller = "Search", action = "Index", page = UrlParameter.Optional, category = UrlParameter.Optional, city = UrlParameter.Optional, firstName = UrlParameter.Optional, lastName = UrlParameter.Optional }
           );
        }
    }
}
