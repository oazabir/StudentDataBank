using StudentService.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentService.Controllers
{
    public class UniversityHomeController : Controller
    {
        //
        // GET: /UniversityHome/{id}

        private StudentServiceContext db = new StudentServiceContext();

        public ActionResult Index(string id)
        {
            return View(new UniversityHomeViewModel
                {
                    University = db.Universities.First(u => u.Code == id),
                    Courses = db.UniversityCourses.Where(uc => uc.University.Code == id),
                    Programs = db.Programs.Where(p => p.University.Code == id).Include("ProgramCourses").AsQueryable(),
                    Students = db.Students.Where(s => s.University.Code == id).Include("LinksToOtherUniversity")
                });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
