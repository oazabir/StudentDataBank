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
using System.Threading.Tasks;
using System.Diagnostics;
using StudentService.Models.Entity;

namespace StudentService.Controllers.Api
{
    public class StudentProgramController : ApiController
    {
        private StudentServiceContext db = new StudentServiceContext();

        // GET api/universities/{universityCode}/students/{studentId}/programs
        public StudentPrograms GetStudentPrograms(string universityCode, string studentId)
        {
            return new StudentPrograms(db.StudentPrograms.Where(sp => sp.Student.University.Code == universityCode && sp.Student.StudentId == studentId).AsEnumerable());
        }

        // GET api/universities/{universityCode}/students/{studentId}/programs/{programCode}
        public StudentProgram GetStudentProgram(string universityCode, string studentId, string programCode)
        {
            StudentProgram studentProgram = db.StudentPrograms.FirstOrDefault(sp => sp.Student.University.Code == universityCode && sp.Student.StudentId == studentId && sp.ProgramCode == programCode);
            if (studentProgram == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return studentProgram;
        }

        // PUT api/universities/{universityCode}/students/{studentId}/programs/{programCode}
        public HttpResponseMessage PutStudentProgram(string universityCode, string studentId, string programCode, StudentProgram studentProgram)
        {
            if (ModelState.IsValid && programCode == studentProgram.ProgramCode)
            {
                StudentProgram existingStudentProgram = GetStudentProgram(universityCode, studentId, programCode);
                existingStudentProgram.EndDate = studentProgram.EndDate;
                existingStudentProgram.StartDate = studentProgram.StartDate;
                existingStudentProgram.Status = studentProgram.Status;
                existingStudentProgram.CGPA = studentProgram.CGPA;
                //db.Entry(existingStudent).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, existingStudentProgram);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values);
            }
        }

        // POST api/universities/{universityCode}/students/
        public HttpResponseMessage PostStudentProgram(string universityCode, string studentId, StudentProgram studentProgram)
        {
            if (ModelState.IsValid)
            {
                studentProgram.Student = db.Students.First(s => s.University.Code == universityCode && s.StudentId == studentId);
                db.StudentPrograms.Add(studentProgram);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, studentProgram);
                response.Headers.Location = new Uri(Url.Link("ProgramsOfStudentOfUniversity", new { universityCode = universityCode, studentId = studentProgram.Id, programCode = studentProgram.ProgramCode }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values);
            }
        }

        // DELETE api/universities/{universityCode}/students/{studentId}/programs/{programCode}
        public HttpResponseMessage DeleteStudentProgram(string universityCode, string studentId, string programCode)
        {
            StudentProgram studentProgram = GetStudentProgram(universityCode, studentId, programCode);
            if (studentProgram == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.StudentPrograms.Remove(studentProgram);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, studentProgram);
        }

        [HttpPost]
        public HttpResponseMessage Refresh(string universityCode, string studentId, string programCode)
        {
            var student = db.Students.Where(s => s.StudentId == studentId && s.University.Code == universityCode).Include("LinksToOtherUniversity").Include("CoursesTaken").First();
            var studentProgram = GetStudentProgram(universityCode, studentId, programCode);
            studentProgram.Status = ProgramStatusEnum.BeingProcessed;
            db.SaveChanges();

            RefreshStudentProgramCourses(universityCode, studentId, programCode);

            var response = Request.CreateResponse(HttpStatusCode.Accepted);
            response.Headers.Location = new Uri(Url.Link("ProgramsOfStudentOfUniversity", new { universityCode = universityCode, studentId = studentId, programCode = programCode }));
            return response;
        }

        private static void RefreshStudentProgramCourses(string universityCode, string studentId, string programCode)
        {
            Task.Run(() =>
            {
                try
                {
                    var newDb = new StudentServiceContext();
                    var student = newDb.Students.Where(s => s.StudentId == studentId && s.University.Code == universityCode).Include("LinksToOtherUniversity").Include("CoursesTaken").First();
                    var studentProgram = newDb.StudentPrograms.FirstOrDefault(sp => sp.Student.University.Code == universityCode && sp.Student.StudentId == studentId && sp.ProgramCode == programCode);

                    // Get all the courses of the program in order to check which of them
                    // are done within the university and which are done in another university
                    var coursesInProgram = newDb.ProgramCourses.Where(pc => pc.Program.Code == programCode && pc.Program.University.Code == universityCode).ToList();
                    var coursesTaken = newDb.StudentCourses.Where(sc => sc.Student.StudentId == studentId && sc.Student.University.Code == universityCode).ToList();
                    var coursesOfAwardingUniversity = newDb.UniversityCourses.Where(uc => uc.University.Code == universityCode).ToList();

                    // Get the courses already credited
                    var coursesCredited = newDb.CourseCrediteds.Where(cc => cc.StudentProgram.ProgramCode == programCode
                        && cc.StudentProgram.Student.StudentId == studentId
                        && cc.StudentProgram.Student.University.Code == universityCode)
                        .ToList();
                    foreach (var courseInProgram in coursesInProgram)
                    {
                        // If student has done the course within this university, then credit it, if not already credited
                        var courseTaken = coursesTaken.FirstOrDefault(ct => ct.CourseCode == courseInProgram.Code);
                        if (courseTaken != null)
                        {
                            // If already credited, updated
                            var courseCredited = coursesCredited.FirstOrDefault(cc => cc.RequiredCourseCode == courseTaken.CourseCode);
                            if (courseCredited != null)
                            {
                                // Course has been already credited. Update information from latest course taken status.
                                courseCredited.Score = courseTaken.Score;
                                courseCredited.Status = CourseCreditedStatusEnum.Accepted;
                                courseCredited.CreditedCourseCode = courseTaken.CourseCode;
                                courseCredited.CreditedCourseUniversityCode = universityCode;
                            }
                            else
                            {
                                // Course not credited, let's count this course towards the program
                                var newCourseCredited = new CourseCredited
                                {
                                    CreditedCourseCode = courseInProgram.Code,
                                    CreditedCourseUniversityCode = universityCode,
                                    Grade = courseTaken.Grade,
                                    Score = courseTaken.Score,
                                    Status = CourseCreditedStatusEnum.Accepted,
                                    RequiredCourseCode = courseInProgram.Code,
                                    StudentProgram = studentProgram
                                };
                                newDb.CourseCrediteds.Add(newCourseCredited);
                            }
                        }
                        else
                        {
                            // Student hasn't done this course in this university.
                            // See if this course is done in any other linked university.
                            var univeralCourseCode = coursesOfAwardingUniversity.Find(uc => uc.Code == courseInProgram.Code).UniversalCourseCode;

                            foreach (var studentLink in student.LinksToOtherUniversity)
                            {
                                var otherUniversity = newDb.Universities.First(u => u.Code == studentLink.UniversityCode);
                                var coursesTakenInOtherUniversity = newDb.StudentCourses.Where(
                                    sc => sc.Student.StudentId == studentLink.StudentId
                                        && sc.Student.University.Code == studentLink.UniversityCode)
                                        .ToList();
                                var coursesOfOtherUniversity = newDb.UniversityCourses.Where(uc => uc.University.Code == studentLink.UniversityCode);

                                // If student has done a course which has the same 
                                // universal course code as the one we are looking for
                                // then that's the course we need.
                                var matchedCourseTakenInOtherUniversity = coursesTakenInOtherUniversity.FirstOrDefault(ctou =>
                                    coursesOfOtherUniversity.First(cou => cou.Code == ctou.CourseCode).UniversalCourseCode
                                    == univeralCourseCode && ctou.Status == CourseStatusEnum.Completed);

                                if (matchedCourseTakenInOtherUniversity != null)
                                {
                                    // If already credited, updated
                                    var courseCredited = coursesCredited.FirstOrDefault(cc => cc.RequiredCourseCode == courseInProgram.Code);
                                    if (courseCredited != null)
                                    {
                                        // Course has been already credited. Update information from latest course taken status.
                                        courseCredited.Score = matchedCourseTakenInOtherUniversity.Score;
                                        courseCredited.Status = CourseCreditedStatusEnum.Accepted;
                                        courseCredited.CreditedCourseCode = matchedCourseTakenInOtherUniversity.CourseCode;
                                        courseCredited.CreditedCourseUniversityCode = otherUniversity.Code;
                                    }
                                    else
                                    {
                                        // Course not credited, let's count this course towards the program
                                        var newCourseCredited = new CourseCredited
                                        {
                                            CreditedCourseCode = matchedCourseTakenInOtherUniversity.CourseCode,
                                            CreditedCourseUniversityCode = otherUniversity.Code,
                                            Grade = matchedCourseTakenInOtherUniversity.Grade,
                                            Score = matchedCourseTakenInOtherUniversity.Score,
                                            Status = CourseCreditedStatusEnum.Accepted,
                                            RequiredCourseCode = courseInProgram.Code,
                                            StudentProgram = studentProgram
                                        };
                                        newDb.CourseCrediteds.Add(newCourseCredited);
                                    }
                                }

                            }
                        }
                    }

                    var refreshedCoursesCredited = newDb.CourseCrediteds.Where(
                        cc => cc.StudentProgram.ProgramCode == programCode
                            && cc.StudentProgram.Student.StudentId == studentId
                            && cc.StudentProgram.Student.University.Code == universityCode
                            && cc.Status == CourseCreditedStatusEnum.Accepted)
                            .ToList();

                    if (refreshedCoursesCredited.Count == coursesInProgram.Count)
                    {
                        studentProgram.Status = ProgramStatusEnum.Completed;
                    }
                    else
                    {
                        studentProgram.Status = ProgramStatusEnum.Completed;
                    }
                    studentProgram.LastRefreshedAt = DateTime.Now;
                    newDb.SaveChanges();
                }
                catch (Exception x)
                {
                    Trace.Fail(x.Message, x.ToString());
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    [CollectionDataContract(Namespace = "http://universalaward.org")]
    public class StudentPrograms : Collection<StudentProgram>
    {
        private IEnumerable<StudentProgram> enu;
        public StudentPrograms(IEnumerable<StudentProgram> e)
        {
            this.enu = e;
        }

        public StudentPrograms() { }

        public new IEnumerator<StudentProgram> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}