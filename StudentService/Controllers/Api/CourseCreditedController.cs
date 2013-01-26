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
    public class CourseCreditedController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/
        public CoursesCredited GetCourseCrediteds(string universityCode, string studentId, string programCode)
        {
            return new CoursesCredited(db.CourseCrediteds.Where(
                cc => cc.StudentProgram.Student.University.Code == universityCode &&
                    cc.StudentProgram.Student.StudentId == studentId &&
                    cc.StudentProgram.ProgramCode == programCode).AsEnumerable());
        }

        // GET api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/{courseCode}
        public CourseCreditedTowardsProgram GetCourseCredited(string universityCode, string studentId, string programCode, string courseCode)
        {
            CourseCreditedTowardsProgram coursecredited = db.CourseCrediteds.FirstOrDefault(
                cc => cc.StudentProgram.Student.University.Code == universityCode &&
                    cc.StudentProgram.Student.StudentId == studentId &&
                    cc.StudentProgram.ProgramCode == programCode &&
                    cc.RequiredCourseCode == courseCode);
            if (coursecredited == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return coursecredited;
        }

        // PUT api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/{courseCode}
        public HttpResponseMessage PutCourseCredited(string universityCode, string studentId, string programCode, string courseCode, CourseCreditedTowardsProgram coursecredited)
        {
            if (ModelState.IsValid && courseCode == coursecredited.RequiredCourseCode)
            {
                var cc = GetCourseCredited(universityCode, studentId, programCode, courseCode);
                cc.Score = coursecredited.Score;
                cc.Status = coursecredited.Status;
                cc.Grade = coursecredited.Grade;
                cc.CreditedCourseCode = coursecredited.CreditedCourseCode;

                //db.Entry(coursecredited).State = EntityState.Modified;

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
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/
        public HttpResponseMessage PostCourseCredited(string universityCode, string studentId, string programCode, CourseCreditedTowardsProgram coursecredited)
        {
            if (ModelState.IsValid)
            {
                var studentProgram = db.StudentPrograms.First(sp => sp.Student.University.Code == universityCode &&
                    sp.Student.StudentId == studentId &&
                    sp.ProgramCode == programCode);
                coursecredited.StudentProgram = studentProgram;
                db.CourseCrediteds.Add(coursecredited);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, coursecredited);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = coursecredited.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/universities/{universityCode}/students/{studentId}/programs/{programCode}/coursescredited/{courseCode}
        public HttpResponseMessage DeleteCourseCredited(string universityCode, string studentId, string programCode, string courseCode)
        {
            CourseCreditedTowardsProgram coursecredited = GetCourseCredited(universityCode, studentId, programCode, courseCode);
            if (coursecredited == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CourseCrediteds.Remove(coursecredited);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, coursecredited);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://universalaward.org")]
    public class CoursesCredited : Collection<CourseCreditedTowardsProgram>
    {
        private IEnumerable<CourseCreditedTowardsProgram> enu;
        public CoursesCredited(IEnumerable<CourseCreditedTowardsProgram> e)
        {
            this.enu = e;
        }

        public CoursesCredited() { }

        public new IEnumerator<CourseCreditedTowardsProgram> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}