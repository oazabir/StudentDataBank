using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using StudentService.Models;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using StudentService.Models.Entity;

namespace StudentService.Controllers.Api
{
    public class ProgramCourseController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/universities/{universityCode}/programs/{programCode}/courses
        public ProgramCourses GetProgramCourses(string universityCode, string programCode)
        {
            return new ProgramCourses(db.ProgramCourses.Where(pc => pc.Program.EducationalInstitute.Code == universityCode && pc.Program.Code == programCode).AsEnumerable());
        }

        // GET api/universities/{universityCode}/programs/{programCode}/courses/{courseCode}
        public EIProgramRequiredCourse GetProgramCourse(string universityCode, string programCode, string courseCode)
        {
            EIProgramRequiredCourse programcourse = db.ProgramCourses.FirstOrDefault(pc => pc.Program.EducationalInstitute.Code == universityCode && pc.Program.Code == programCode && pc.Code == courseCode);
            if (programcourse == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return programcourse;
        }

        // PUT api/universities/{universityCode}/programs/{programCode}/courses/{courseCode}
        public HttpResponseMessage PutProgramCourse(string universityCode, string programCode, string courseCode, [FromBody]EIProgramRequiredCourse programcourse)
        {
            if (ModelState.IsValid && courseCode == programcourse.Code)
            {
                var pc = GetProgramCourse(universityCode, programCode, courseCode);
                //db.Entry(programcourse).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values);
            }
        }

        // POST api/universities/{universityCode}/programs/{programCode}/courses
        public HttpResponseMessage PostProgramCourse(string universityCode, string programCode, [FromBody]EIProgramRequiredCourse programcourse)
        {
            if (ModelState.IsValid)
            {
                programcourse.Program = db.Programs.First(p => p.EducationalInstitute.Code == universityCode && p.Code == programCode);
                db.ProgramCourses.Add(programcourse);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, programcourse);
                response.Headers.Location = new Uri(Url.Link("CoursesOfProgramsOfEducationalInstitute", 
                    new { universityCode = universityCode, programCode = programCode, courseCode = programcourse.Code }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values);
            }
        }

        // DELETE api/universities/{universityCode}/programs/{programCode}/courses/{courseCode}
        public HttpResponseMessage DeleteProgramCourse(string universityCode, string programCode, string courseCode)
        {
            EIProgramRequiredCourse programcourse = GetProgramCourse(universityCode, programCode, courseCode);
            if (programcourse == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ProgramCourses.Remove(programcourse);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, programcourse);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://studentdatabank.org", Name="Courses")]
    public class ProgramCourses : Collection<EIProgramRequiredCourse>
    {
        private IEnumerable<EIProgramRequiredCourse> enu;
        public ProgramCourses(IEnumerable<EIProgramRequiredCourse> e)
        {
            this.enu = e;
        }

        public ProgramCourses() { }

        public new IEnumerator<EIProgramRequiredCourse> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}