using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "University",
                url: "University/{universityCode}",
                defaults: new { controller = "University", action = "Index" }
                );
            routes.MapRoute(
                "UniversityStudent",
                url: "University/{universityCode}/Students/{studentId}",
                defaults: new { controller = "University", action = "Student" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}