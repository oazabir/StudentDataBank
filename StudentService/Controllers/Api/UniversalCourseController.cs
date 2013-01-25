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
using StudentService.Models.Entity;

namespace StudentService.Controllers.Api
{
    public class UniversalCourseController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/UniversalCourse
        public IEnumerable<UniversalCourse> GetUniversalCourses()
        {
            return db.UniversalCourses.AsEnumerable();
        }

        // GET api/UniversalCourse/5
        public UniversalCourse GetUniversalCourse(string code)
        {
            UniversalCourse universalcourse = db.UniversalCourses.Where(uc => uc.Code == code).FirstOrDefault();
            if (universalcourse == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return universalcourse;
        }

        // PUT api/UniversalCourse/5
        public HttpResponseMessage PutUniversalCourse(string code, UniversalCourse universalcourse)
        {
            if (ModelState.IsValid && code == universalcourse.Code)
            {
                db.Entry(universalcourse).State = EntityState.Modified;

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

        // POST api/UniversalCourse
        public HttpResponseMessage PostUniversalCourse(UniversalCourse universalcourse)
        {
            if (ModelState.IsValid)
            {
                db.UniversalCourses.Add(universalcourse);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, universalcourse);
                response.Headers.Location = new Uri(Url.Link("UniversalCourses", new { code = universalcourse.Code }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/UniversalCourse/5
        public HttpResponseMessage DeleteUniversalCourse(string code)
        {
            UniversalCourse universalcourse = db.UniversalCourses.Find(code);
            if (universalcourse == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.UniversalCourses.Remove(universalcourse);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, universalcourse);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}