using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace StudentService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CoursesOfProgramsOfUniversity",
                routeTemplate: "api/universities/{universityCode}/programs/{programCode}/courses/{courseCode}",
                defaults: new
                {
                    controller = "ProgramCourse",
                    courseCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "ProgramsOfUniversity",
                routeTemplate: "api/universities/{universityCode}/programs/{programCode}",
                defaults: new { 
                    controller = "program", 
                    programCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "StudentsOfUniversity",
                routeTemplate: "api/universities/{universityCode}/students/{studentId}",
                defaults: new
                {
                    controller = "student",
                    studentId = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "ProgramsOfStudentOfUniversity",
                routeTemplate: "api/universities/{universityCode}/students/{studentId}/programs/{programCode}",
                defaults: new
                {
                    controller = "studentprogram",
                    programCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "RefreshProgramOfStudentOfUniversity",
                routeTemplate: "api/universities/{universityCode}/students/{studentId}/programs/{programCode}/refresh",
                defaults: new
                {
                    controller = "studentprogram",
                    action = "Refresh"
                });

            config.Routes.MapHttpRoute(
                name: "CourseCreditedOfProgramOfStudentOfUniversity",
                routeTemplate: "api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/{courseCode}",
                defaults: new
                {
                    controller = "coursecredited",
                    courseCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "Universities",
                routeTemplate: "api/universities/{code}",                
                defaults: new
                {
                    controller = "university",
                    code = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "UniversalCourses",
                routeTemplate: "api/universal_courses/{code}",
                defaults: new
                {
                    controller = "UniversalCourse",
                    code = RouteParameter.Optional
                });

            //config.Formatters.XmlFormatter.UseXmlSerializer = true;

            // Make XML default formatter
            var xmlFormatter = config.Formatters.XmlFormatter;
            config.Formatters.Remove(xmlFormatter);
            config.Formatters.Insert(0, xmlFormatter);            
        }
    }
}
