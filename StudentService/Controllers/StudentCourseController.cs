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

namespace StudentService.Controllers
{
    public class StudentCourseController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/StudentCourse
        public IEnumerable<StudentCourse> GetStudentCourses()
        {
            return db.StudentCourses.AsEnumerable();
        }

        // GET api/StudentCourse/5
        public StudentCourse GetStudentCourse(int id)
        {
            StudentCourse studentcourse = db.StudentCourses.Find(id);
            if (studentcourse == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return studentcourse;
        }

        // PUT api/StudentCourse/5
        public HttpResponseMessage PutStudentCourse(int id, StudentCourse studentcourse)
        {
            if (ModelState.IsValid && id == studentcourse.Id)
            {
                db.Entry(studentcourse).State = EntityState.Modified;

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

        // POST api/StudentCourse
        public HttpResponseMessage PostStudentCourse(StudentCourse studentcourse)
        {
            if (ModelState.IsValid)
            {
                db.StudentCourses.Add(studentcourse);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, studentcourse);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = studentcourse.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/StudentCourse/5
        public HttpResponseMessage DeleteStudentCourse(int id)
        {
            StudentCourse studentcourse = db.StudentCourses.Find(id);
            if (studentcourse == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.StudentCourses.Remove(studentcourse);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, studentcourse);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}