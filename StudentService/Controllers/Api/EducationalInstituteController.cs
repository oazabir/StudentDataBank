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
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using StudentService.Models.Entity;

namespace StudentService.Controllers.Api
{
    public class EducationalInstituteController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/University
        public Universities GetUniversities()
        {
            return new Universities(db.EducationalInstitutes.AsEnumerable());
        }

        // GET api/University/5
        public EducationalInstitute GetUniversity(string code)
        {
            EducationalInstitute university = db.EducationalInstitutes.First(u => u.Code == code);
            if (university == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return university;
        }

        // PUT api/University/5
        public HttpResponseMessage PutUniversity(string code, EducationalInstitute university)
        {
            if (ModelState.IsValid && code == university.Code)
            {
                var existingUni = GetUniversity(code);
                existingUni.Name = university.Name;
                existingUni.Address = university.Address;
                
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, 
                    GetUniversity(code));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/University
        public HttpResponseMessage PostUniversity(EducationalInstitute university)
        {
            if (ModelState.IsValid)
            {
                db.EducationalInstitutes.Add(university);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, university);
                response.Headers.Location = new Uri(Url.Link("EducationalInstitutes", new { code = university.Code }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values);
            }
        }

        // DELETE api/University/5
        public HttpResponseMessage DeleteUniversity(string code)
        {
            EducationalInstitute university = GetUniversity(code);
            
            db.EducationalInstitutes.Remove(university);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, university);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://studentdatabank.org")]
    public class Universities : Collection<EducationalInstitute>
    {
        private IEnumerable<EducationalInstitute> enu;
        public Universities(IEnumerable<EducationalInstitute> e)
        {
            this.enu = e;
        }

        public Universities() {}

        public new IEnumerator<EducationalInstitute> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}