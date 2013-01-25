using StudentService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentService.Models.Entity;

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

        public ActionResult StudentProgram(string universityCode, string studentId, string programCode)
        {
            return View(new StudentProgramViewModel
            {
                Student = db.Students.First(s => s.StudentId == studentId && s.University.Code == universityCode),
                Program = db.Programs.First(p => p.Code == programCode &&  p.University.Code == universityCode),
                ProgramCourses = db.ProgramCourses.Where(pc => pc.Program.Code == programCode && pc.Program.University.Code == universityCode),
                CourseCredited = db.CourseCrediteds.Where(cc => cc.StudentProgram.ProgramCode == programCode && cc.StudentProgram.Student.StudentId == studentId && cc.StudentProgram.Student.University.Code == universityCode),
                Universities = db.Universities.ToList()
            });
        }

        public ActionResult ChangeCreditedCourse(string universityCode, string studentId, string programCode, string creditedUniversityCode, string creditedCourseCode, string newStatus)
        {
            CourseCreditedStatusEnum status = (CourseCreditedStatusEnum)Enum.Parse(typeof(CourseCreditedStatusEnum), newStatus);
            var creditedCourse = db.CourseCrediteds.First(cc => cc.StudentProgram.ProgramCode == programCode
                && cc.StudentProgram.Student.StudentId == studentId
                && cc.StudentProgram.Student.University.Code == universityCode
                && cc.CreditedCourseUniversityCode == creditedUniversityCode
                && cc.CreditedCourseCode == creditedCourseCode);
            creditedCourse.Status = status;
            db.SaveChanges();

            return RedirectToAction("StudentProgram", new
            {
                universityCode = universityCode,
                studentId = studentId,
                programCode = programCode                            
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
