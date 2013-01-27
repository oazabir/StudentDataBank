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
                "EducationalInstitute",
                url: "Institutes/{universityCode}",
                defaults: new { controller = "EducationalInstitute", action = "Index" }
                );
            routes.MapRoute(
                "EducationalInstituteStudent",
                url: "Institutes/{universityCode}/Students/{studentId}",
                defaults: new { controller = "EducationalInstitute", action = "Student" }
                );
            routes.MapRoute(
                "EducationalInstituteStudentProgram",
                url: "Institutes/{universityCode}/Students/{studentId}/Programs/{programCode}",
                defaults: new { controller = "EducationalInstitute", action = "StudentProgram" }
                );
            routes.MapRoute(
                "RecalculateProgram",
                url: "Institutes/{universityCode}/Students/{studentId}/Programs/{programCode}/recalculate",
                defaults: new { controller = "EducationalInstitute", action = "RecalculateProgram" }
                );
            routes.MapRoute(
                "ChangeCreditedCourse",
                url: "Institutes/{universityCode}/Students/{studentId}/Programs/{programCode}/{creditedUniversityCode}/{creditedCourseCode}/{newStatus}",
                defaults: new { controller = "EducationalInstitute", action = "ChangeCreditedCourse" }
                );

            routes.MapRoute(
                "NewLink",
                url: "Institutes/{universityCode}/Students/{studentId}/NewLink",
                defaults: new { controller = "EducationalInstitute", action = "NewLink" }
                );

            routes.MapRoute(
                "FetchCourses",
                url: "Institutes/{universityCode}/Students/{studentId}/links/{otherUniversityCode}/{otherStudentId}/fetch",
                defaults: new { controller = "EducationalInstitute", action = "FetchCourses" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}