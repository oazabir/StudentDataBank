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
using System.Net.Http;
using System.Text;

namespace StudentService.Controllers
{
    public class EducationalInstituteController : Controller
    {
        //
        // GET: /institutes/{id}

        private StudentServiceContext db = new StudentServiceContext();

        public ActionResult Index(string universityCode)
        {
            return View(new EIViewModel
                {
                    EducationalInstitute = db.EducationalInstitutes.First(u => u.Code == universityCode),
                    Courses = db.EducationalInstituteCourses.Where(uc => uc.EducationalInstitute.Code == universityCode),
                    Programs = db.Programs.Where(p => p.EducationalInstitute.Code == universityCode).Include("ProgramCourses").AsQueryable(),
                    Students = db.Students.Where(s => s.EducationalInstitute.Code == universityCode).Include("LinksToOtherEI")
                });
        }

        public ActionResult Student(string universityCode, string studentId)
        {
            return View(new StudentViewModel
            {
                Student = db.Students.First(s => s.StudentId == studentId && s.EducationalInstitute.Code == universityCode),
                Programs = db.StudentPrograms.Where(sp => sp.Student.StudentId == studentId && sp.Student.EducationalInstitute.Code == universityCode),
                Courses = db.StudentCourses.Where(sc => sc.Student.StudentId == studentId && sc.Student.EducationalInstitute.Code == universityCode),
                Links = db.StudentLinks.Where(sl => sl.Student.StudentId == studentId && sl.Student.EducationalInstitute.Code ==  universityCode)
            });
        }

        public ActionResult StudentProgram(string universityCode, string studentId, string programCode)
        {
            return View(new StudentProgramViewModel
            {
                Student = db.Students.First(s => s.StudentId == studentId && s.EducationalInstitute.Code == universityCode),
                Program = db.Programs.First(p => p.Code == programCode &&  p.EducationalInstitute.Code == universityCode),
                ProgramCourses = db.ProgramCourses.Where(pc => pc.Program.Code == programCode && pc.Program.EducationalInstitute.Code == universityCode).ToList(),
                CourseCredited = db.CourseCrediteds.Where(cc => cc.StudentProgram.ProgramCode == programCode && cc.StudentProgram.Student.StudentId == studentId && cc.StudentProgram.Student.EducationalInstitute.Code == universityCode).ToList(),
                EducationalInstitutes = db.EducationalInstitutes.ToList(),
                EnrolledProgram = db.StudentPrograms.First(ep => ep.Student.StudentId == studentId && ep.Student.EducationalInstitute.Code == universityCode && ep.ProgramCode == programCode),
                CoursesInEI = db.EducationalInstituteCourses.Where(eic => eic.EducationalInstitute.Code == universityCode).ToList()
            });
        }

        public ActionResult RecalculateProgram(string universityCode, string studentId, string programCode)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri("http://" + Request.Url.Authority + "/api/institutes/" + universityCode + "/Students/" + studentId + "/Programs/" + programCode + "/refresh");
                var result = client.PostAsync(uri, new StringContent("", Encoding.UTF8, "application/xml"));
                if (result.Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return RedirectToAction("StudentProgram");
                else
                    throw new ApplicationException("Refresh failed: " + result.Result.StatusCode);
            }

            return RedirectToAction("StudentProgram");
        }

        

        public ActionResult ChangeCreditedCourse(string universityCode, string studentId, string programCode, string creditedUniversityCode, string creditedCourseCode, string newStatus)
        {
            CourseCreditedStatusEnum status = (CourseCreditedStatusEnum)Enum.Parse(typeof(CourseCreditedStatusEnum), newStatus);
            var creditedCourse = db.CourseCrediteds.First(cc => cc.StudentProgram.ProgramCode == programCode
                && cc.StudentProgram.Student.StudentId == studentId
                && cc.StudentProgram.Student.EducationalInstitute.Code == universityCode
                && cc.CreditedCourseEICode == creditedUniversityCode
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

        [HttpGet]
        public ActionResult NewLink(string universityCode, string studentId)
        {
            return View(new NewLinkViewModel
            {
                InstitutionStudentID = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                DateOfBirth = DateTime.Now,
                Student = db.Students.First(s => s.EducationalInstitute.Code == universityCode && s.StudentId == studentId),
                Institutes = db.EducationalInstitutes.Select(ei => new SelectListItem
                {
                    Text = ei.Name,
                    Value = ei.Code
                }).ToArray()
            });
        }

        [HttpPost]
        public ActionResult NewLink(string universityCode, string studentId, NewLinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.StudentLinks.Add(new StudentLinkToOtherEI
                {
                    StudentId = model.InstitutionStudentID,
                    EICode = model.SelectedInstitute,
                    Status = LinkApprovalStatusEnum.PendingApproval
                });
                db.SaveChanges();

                return Student(universityCode, studentId);
            }
            else
            {

                model.Student = db.Students.First(s => s.EducationalInstitute.Code == universityCode && s.StudentId == studentId);
                model.Institutes = db.EducationalInstitutes.Select(ei => new SelectListItem
                    {
                        Text = ei.Name,
                        Value = ei.Code
                    }).ToArray();

                return View(model);
            }
        }

        public ActionResult FetchCourses(string universityCode, string studentId, string otherUniversityCode, string otherStudentId)
        {
            var link = db.StudentLinks.First(sl => sl.Student.EducationalInstitute.Code == universityCode
                && sl.Student.StudentId == studentId
                && sl.EICode == otherUniversityCode
                && sl.StudentId == otherStudentId);

            link.Status = LinkApprovalStatusEnum.FetchingRecords;
            db.SaveChanges();

            //TODO: Execute BPS Process to fetch courses

            return RedirectToAction("Student");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
