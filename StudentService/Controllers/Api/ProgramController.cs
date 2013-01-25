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
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using StudentService.Models.Entity;

namespace StudentService.Controllers.Api
{
    public class ProgramController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/universities/{universityCode}/programs
        public Programs GetPrograms(string universityCode)
        {
            var programs = db.Programs.Where(p => p.University.Code == universityCode);
            return new Programs(programs.AsEnumerable());
        }

        // GET api/universities/{universityCode}/programs/{programCode}
        public Program GetProgram(string universityCode, string programCode)
        {
            Program program = db.Programs.SingleOrDefault(p => p.University.Code == universityCode && p.Code == programCode);
            if (program == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return program;
        }

        // PUT api/universities/{universityCode}/programs/{programCode}
        public HttpResponseMessage PutProgram(string universityCode, Program program)
        {
            if (ModelState.IsValid)
            {
                program.University = db.Universities.Single(u => u.Code == universityCode);
                db.Entry(program).State = EntityState.Modified;

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

        // POST api/universities/{universityCode}/programs/
        public HttpResponseMessage PostProgram(string universityCode, Program program)
        {
            if (ModelState.IsValid)
            {
                program.University = db.Universities.Single(u => u.Code == universityCode);
                db.Programs.Add(program);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, program);
                response.Headers.Location = new Uri(Url.Link("ProgramsOfUniversity", new { universityCode = universityCode, programCode = program.Code }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/universities/{universityCode}/programs/{programCode}
        public HttpResponseMessage DeleteProgram(string universityCode, string programCode)
        {
            Program program = GetProgram(universityCode, programCode);
            if (program == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Programs.Remove(program);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, program);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://universalaward.org")]
    public class Programs : Collection<Program>
    {
        private IEnumerable<Program> enu;
        public Programs(IEnumerable<Program> e)
        {
            this.enu = e;
        }

        public Programs() { }

        public new IEnumerator<Program> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}