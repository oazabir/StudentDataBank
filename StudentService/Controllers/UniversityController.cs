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
    public class UniversityController : Controller
    {
        //
        // GET: /UniversityHome/{id}

        private StudentServiceContext db = new StudentServiceContext();

        public ActionResult Index(string universityCode)
        {
            return View(new UniversityViewModel
                {
                    University = db.Universities.First(u => u.Code == universityCode),
                    Courses = db.UniversityCourses.Where(uc => uc.University.Code == universityCode),
                    Programs = db.Programs.Where(p => p.University.Code == universityCode).Include("ProgramCourses").AsQueryable(),
                    Students = db.Students.Where(s => s.University.Code == universityCode).Include("LinksToOtherUniversity")
                });
        }

        public ActionResult Student(string universityCode, string studentId)
        {
            return View(new StudentViewModel
            {
                Student = db.Students.First(s => s.StudentId == studentId && s.University.Code == universityCode),
                Programs = db.StudentPrograms.Where(sp => sp.Student.StudentId == studentId && sp.Student.University.Code == universityCode),
                Courses = db.StudentCourses.Where(sc => sc.Student.StudentId == studentId && sc.Student.University.Code == universityCode),
                Links = db.StudentLinks.Where(sl => sl.Student.StudentId == studentId && sl.Student.University.Code ==  universityCode)
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
