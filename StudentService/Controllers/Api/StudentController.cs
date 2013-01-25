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
    public class StudentController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/universities/{universityCode}/students/
        public Students GetStudents(string universityCode)
        {
            return new Students(db.Students.Where(s => s.University.Code == universityCode).AsEnumerable());
        }

        // GET api/universities/{universityCode}/students/{studentId}
        public UniversityStudent GetStudent(string universityCode, string studentId)
        {
            UniversityStudent student = db.Students.FirstOrDefault(s => s.University.Code == universityCode && s.StudentId == studentId);
            if (student == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return student;
        }

        // PUT api/universities/{universityCode}/students/{studentId}
        public HttpResponseMessage PutStudent(string universityCode, string studentId, UniversityStudent student)
        {
            if (ModelState.IsValid && studentId == student.StudentId)
            {
                UniversityStudent existingStudent = GetStudent(universityCode, studentId);
                existingStudent.Firstname = student.Firstname;
                existingStudent.Lastname = student.Lastname;
                //db.Entry(existingStudent).State = EntityState.Modified;

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

        // POST api/universities/{universityCode}/students/
        public HttpResponseMessage PostStudent(string universityCode, UniversityStudent student)
        {
            if (ModelState.IsValid)
            {
                student.University = db.Universities.First(u => u.Code == universityCode);
                db.Students.Add(student);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, student);
                response.Headers.Location = new Uri(Url.Link("StudentsOfUniversity", new { universityCode = universityCode, studentId = student.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/universities/{universityCode}/students/{studentId}
        public HttpResponseMessage DeleteStudent(string universityCode, string studentId)
        {
            UniversityStudent student = GetStudent(universityCode, studentId);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Students.Remove(student);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://universalaward.org")]
    public class Students : Collection<UniversityStudent>
    {
        private IEnumerable<UniversityStudent> enu;
        public Students(IEnumerable<UniversityStudent> e)
        {
            this.enu = e;
        }

        public Students() { }

        public new IEnumerator<UniversityStudent> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}