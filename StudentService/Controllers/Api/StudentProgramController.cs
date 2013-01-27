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
            return new StudentPrograms(db.StudentPrograms.Where(sp => sp.Student.EducationalInstitute.Code == universityCode && sp.Student.StudentId == studentId).AsEnumerable());
        }

        // GET api/universities/{universityCode}/students/{studentId}/programs/{programCode}
        public EIStudentEnrolledProgram GetStudentProgram(string universityCode, string studentId, string programCode)
        {
            EIStudentEnrolledProgram studentProgram = db.StudentPrograms.FirstOrDefault(sp => sp.Student.EducationalInstitute.Code == universityCode && sp.Student.StudentId == studentId && sp.ProgramCode == programCode);
            if (studentProgram == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return studentProgram;
        }

        // PUT api/universities/{universityCode}/students/{studentId}/programs/{programCode}
        public HttpResponseMessage PutStudentProgram(string universityCode, string studentId, string programCode, EIStudentEnrolledProgram studentProgram)
        {
            if (ModelState.IsValid && programCode == studentProgram.ProgramCode)
            {
                EIStudentEnrolledProgram existingStudentProgram = GetStudentProgram(universityCode, studentId, programCode);
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
        public HttpResponseMessage PostStudentProgram(string universityCode, string studentId, EIStudentEnrolledProgram studentProgram)
        {
            if (ModelState.IsValid)
            {
                studentProgram.Student = db.Students.First(s => s.EducationalInstitute.Code == universityCode && s.StudentId == studentId);
                db.StudentPrograms.Add(studentProgram);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, studentProgram);
                response.Headers.Location = new Uri(Url.Link("ProgramsOfStudentOfEducationalInstitute", new { universityCode = universityCode, studentId = studentProgram.Id, programCode = studentProgram.ProgramCode }));
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
            EIStudentEnrolledProgram studentProgram = GetStudentProgram(universityCode, studentId, programCode);
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
            var studentProgram = GetStudentProgram(universityCode, studentId, programCode);
            studentProgram.Status = ProgramStatusEnum.BeingProcessed;
            db.SaveChanges();

            RefreshStudentProgramCourses(universityCode, studentId, programCode);

            var response = Request.CreateResponse(HttpStatusCode.Accepted);
            response.Headers.Location = new Uri(Url.Link("ProgramsOfStudentOfEducationalInstitute", new { universityCode = universityCode, studentId = studentId, programCode = programCode }));
            return response;
        }

        private static void RefreshStudentProgramCourses(string universityCode, string studentId, string programCode)
        {
            Task.Run(() =>
            {
                try
                {
                    var newDb = new StudentServiceContext();
                    var student = newDb.Students.Where(s => s.StudentId == studentId && s.EducationalInstitute.Code == universityCode).First();
                    var linksToOtherEI = newDb.StudentLinks.Where(s => s.Student.StudentId == studentId && s.Student.EducationalInstitute.Code == universityCode && (s.Status == LinkApprovalStatusEnum.Accepted || s.Status == LinkApprovalStatusEnum.RecordsReceived));
                    var studentProgram = newDb.StudentPrograms.FirstOrDefault(sp => sp.Student.EducationalInstitute.Code == universityCode && sp.Student.StudentId == studentId && sp.ProgramCode == programCode);

                    // Get all the courses of the program in order to check which of them
                    // are done within the university and which are done in another university
                    var coursesInProgram = newDb.ProgramCourses.Where(pc => pc.Program.Code == programCode && pc.Program.EducationalInstitute.Code == universityCode).ToList();
                    var coursesTaken = newDb.StudentCourses.Where(sc => sc.Student.StudentId == studentId && sc.Student.EducationalInstitute.Code == universityCode).ToList();
                    var coursesOfAwardingUniversity = newDb.EducationalInstituteCourses.Where(uc => uc.EducationalInstitute.Code == universityCode).ToList();

                    // Get the courses already credited
                    var coursesCredited = newDb.CourseCrediteds.Where(cc => cc.StudentProgram.ProgramCode == programCode
                        && cc.StudentProgram.Student.StudentId == studentId
                        && cc.StudentProgram.Student.EducationalInstitute.Code == universityCode)
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
                                courseCredited.CreditedCourseEICode = universityCode;
                            }
                            else
                            {
                                // Course not credited, let's count this course towards the program
                                var newCourseCredited = new CourseCreditedTowardsProgram
                                {
                                    CreditedCourseCode = courseInProgram.Code,
                                    CreditedCourseEICode = universityCode,
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

                            foreach (var studentLink in linksToOtherEI)
                            {
                                var otherUniversity = newDb.EducationalInstitutes.First(u => u.Code == studentLink.EICode);
                                var coursesTakenInOtherUniversity = newDb.StudentCourses.Where(
                                    sc => sc.Student.StudentId == studentLink.StudentId
                                        && sc.Student.EducationalInstitute.Code == studentLink.EICode)
                                        .ToList();
                                var coursesOfOtherUniversity = newDb.EducationalInstituteCourses.Where(uc => uc.EducationalInstitute.Code == studentLink.EICode);

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
                                        courseCredited.CreditedCourseEICode = otherUniversity.Code;
                                    }
                                    else
                                    {
                                        // Course not credited, let's count this course towards the program
                                        var newCourseCredited = new CourseCreditedTowardsProgram
                                        {
                                            CreditedCourseCode = matchedCourseTakenInOtherUniversity.CourseCode,
                                            CreditedCourseEICode = otherUniversity.Code,
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

                    // Check if student has completed all the required
                    // courses of this program. If student has done them all,
                    // then accept the Program as completed.
                    var refreshedCoursesCredited = newDb.CourseCrediteds.Where(
                        cc => cc.StudentProgram.ProgramCode == programCode
                            && cc.StudentProgram.Student.StudentId == studentId
                            && cc.StudentProgram.Student.EducationalInstitute.Code == universityCode
                            && cc.Status == CourseCreditedStatusEnum.Accepted)
                            .ToList();

                    if (refreshedCoursesCredited.Count == coursesInProgram.Count)
                    {
                        studentProgram.Status = ProgramStatusEnum.Completed;
                    }
                    else
                    {
                        studentProgram.Status = ProgramStatusEnum.InProgress;
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

    [CollectionDataContract(Namespace = "http://studentdatabank.org")]
    public class StudentPrograms : Collection<EIStudentEnrolledProgram>
    {
        private IEnumerable<EIStudentEnrolledProgram> enu;
        public StudentPrograms(IEnumerable<EIStudentEnrolledProgram> e)
        {
            this.enu = e;
        }

        public StudentPrograms() { }

        public new IEnumerator<EIStudentEnrolledProgram> GetEnumerator()
        {
            return (this.enu ?? this).GetEnumerator();
        }
    }
}