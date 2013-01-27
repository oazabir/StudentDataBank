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
                name: "CoursesOfProgramsOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/programs/{programCode}/courses/{courseCode}",
                defaults: new
                {
                    controller = "ProgramCourse",
                    courseCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "ProgramsOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/programs/{programCode}",
                defaults: new { 
                    controller = "Program", 
                    programCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "StudentOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/students/{studentId}",
                defaults: new
                {
                    controller = "Student",
                    studentId = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "ProgramsOfStudentOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/students/{studentId}/programs/{programCode}",
                defaults: new
                {
                    controller = "StudentProgram",
                    programCode = RouteParameter.Optional
                });
            
            config.Routes.MapHttpRoute(
                name: "RefreshProgramOfStudentOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/students/{studentId}/programs/{programCode}/refresh",
                defaults: new
                {
                    controller = "StudentProgram",
                    action = "Refresh"
                });

            config.Routes.MapHttpRoute(
                name: "CourseCreditedOfProgramOfStudentOfEducationalInstitute",
                routeTemplate: "api/institutes/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/{courseCode}",
                defaults: new
                {
                    controller = "CourseCredited",
                    courseCode = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "EducationalInstitutes",
                routeTemplate: "api/institutes/{code}",                
                defaults: new
                {
                    controller = "EducationalInstitute",
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
            xmlFormatter.Indent = true;
            config.Formatters.Remove(xmlFormatter);
            config.Formatters.Insert(0, xmlFormatter);            
            
        }
    }
}
