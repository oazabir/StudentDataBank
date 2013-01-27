namespace StudentService.Migrations
{
    using StudentService.Models;
    using StudentService.Models.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentService.Models.StudentServiceContext>
    {
        private const string BaseUrl = "http://universalawards.org/";
        private const string UniversalCoursesUrl = "universal_courses/";
        private const string OxfordCoursesUrl = BaseUrl + "OX/courses/";
        private const string CambridgeCoursesUrl = BaseUrl + "CAM/courses/";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StudentService.Models.StudentServiceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.UniversalCourses.AddOrUpdate(
                uc => uc.Id,
                    new UniversalCourse { Code = "MAT101_BASIC", Name = "Mathematics 101 Basic", Description = "Differential Calculus, Boolean Algebra" },
                    new UniversalCourse { Code = "MAT102_BASIC", Name = "Mathematics 102 Basic", Description = "Limits, derivatives, definite and indefinite integrals" },
                    new UniversalCourse { Code = "MAT102_ADV", Name = "Mathematics 102 Advanced", Description = "Properties of numbers; fundamental operations with algebraic expressions; polynomials; systems of equations; ratio and proportion; factoring; functions; graphs; solutions of linear inequalities; and linear and quadratic equations." },
                    new UniversalCourse { Code = "PRG101_BASIC", Name = "Programming 101 Basic", Description = "Basic C programming" },
                    new UniversalCourse { Code = "PRG102_BASIC", Name = "Programming 102 Basic", Description = "Basic Java programming" },
                    new UniversalCourse { Code = "PRG103_ADV", Name = "Programming 103 Advanced", Description = "Enterprise Application Development" },
                    new UniversalCourse { Code = "PHY101_BASIC", Name = "Physics 101 Basic", Description = "Theory of Relativity" },
                    new UniversalCourse { Code = "PHY102_BASIC", Name = "Physics 102 Basic", Description = "Quantum Mechanics" },
                    new UniversalCourse { Code = "PHY102_ADV", Name = "Physics 102 Advanced", Description = "String Theory" }

                );
            
            context.EducationalInstitutes.AddOrUpdate(
                u => u.Id,
                new EducationalInstitute
                {
                    Code = "OX",
                    Name = "Oxford University",
                    Address = "Oxfordshire, UK",
                    Courses = new System.Collections.Generic.List<EICourse> {
                        new EICourse { Code = "MAT101", Name = "Mathematics 101", UniversalCourseCode="MAT101_BASIC" },
                        new EICourse { Code = "MAT102", Name = "Mathematics 102", UniversalCourseCode="MAT102_BASIC" },
                        new EICourse { Code = "PHY101", Name = "Physics 101", UniversalCourseCode="PHY101_BASIC" },
                        new EICourse { Code = "PROG101", Name = "Programming 101", UniversalCourseCode="PROG101_BASIC" },
                        new EICourse { Code = "PROG102", Name = "Programming 102", UniversalCourseCode="PROG102_BASIC" }
                    },
                    Programs = new System.Collections.Generic.List<EIProgram> {
                        new EIProgram { Code = "MSC_SE", Name = "Masters in Software Engineering", 
                            ProgramCourses = new System.Collections.Generic.List<EIProgramRequiredCourse> {
                                new EIProgramRequiredCourse { Code = "MAT101" },
                                new EIProgramRequiredCourse { Code = "MAT102" },
                                new EIProgramRequiredCourse { Code = "PHY101" },
                                new EIProgramRequiredCourse { Code = "PROG101" },
                                new EIProgramRequiredCourse { Code = "PROG102" }
                            }
                        },
                        new EIProgram { Code = "MSC_PHY", Name = "Masters in Physics",
                            ProgramCourses = new System.Collections.Generic.List<EIProgramRequiredCourse> {
                                new EIProgramRequiredCourse { Code = "MAT101" },
                                new EIProgramRequiredCourse { Code = "MAT102" },
                                new EIProgramRequiredCourse { Code = "PHY101" },
                                new EIProgramRequiredCourse { Code = "PHY102" }
                            }
                        }
                    },
                    Students = new System.Collections.Generic.List<EIStudent>
                    {
                        new EIStudent { StudentId = "OX123", Firstname = "Omar", Lastname = "AL Zabir",
                            LinksToOtherEI = new System.Collections.Generic.List<StudentLinkToOtherEI> {
                                new StudentLinkToOtherEI { EICode = "CAM", StudentId="CAM123", Status=LinkApprovalStatusEnum.Accepted },
                                new StudentLinkToOtherEI { EICode = "UCL", StudentId="UCL123", Status=LinkApprovalStatusEnum.PendingApproval },
                            },
                            CoursesTaken = new System.Collections.Generic.List<EIStudentCourseTaken> {
                                new EIStudentCourseTaken { CourseCode="PROG101", Score=3.5f, Grade="A", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new EIStudentCourseTaken { CourseCode="PROG102", Score=3.0f, Grade="B", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                            },
                            Programs = new System.Collections.Generic.List<EIStudentEnrolledProgram> {
                                new EIStudentEnrolledProgram { 
                                    ProgramCode = "MSC_SE", 
                                    Status = ProgramStatusEnum.InProgress, 
                                    StartDate=DateTime.Parse("1/1/2001"), 
                                    EndDate=DateTime.Parse("3/3/2001"),
                                    CGPA=3.5f,
                                    LastRefreshedAt = DateTime.Now,
                                    /*CoursesCredited = new System.Collections.Generic.List<CourseCredited> {
                                        new CourseCredited { Score=3.5f, RequiredCourseCode="PROG101", Grade="A", CreditedCourseUniversityCode="OX", CreditedCourseCode="PROG101" },
                                        new CourseCredited { Score=3.0f, RequiredCourseCode="PROG102", Grade="B", CreditedCourseUniversityCode="OX", CreditedCourseCode="PROG102" },
                                        new CourseCredited { Score=3.5f, RequiredCourseCode="MAT101", Grade="A", CreditedCourseUniversityCode="CAM", CreditedCourseCode="MAT101" },
                                        new CourseCredited { Score=3.0f, RequiredCourseCode="MAT102", Grade="B", CreditedCourseUniversityCode="CAM", CreditedCourseCode="MAT102" },
                                        new CourseCredited { Score=4.0f, RequiredCourseCode="PHY101", Grade="A+", CreditedCourseUniversityCode="CAM", CreditedCourseCode="PHY101" }
                                    }*/
                                }
                            }
                        }
                    }
                },
                new EducationalInstitute
                {
                    Code = "CAM",
                    Name = "Cambridge University",
                    Address = "Cambridge, UK",
                    Courses = new System.Collections.Generic.List<EICourse> {
                        new EICourse { Code = "MAT101", Name = "Mathematics 101", UniversalCourseCode="MAT101_BASIC" },
                        new EICourse { Code = "MAT102", Name = "Mathematics 102", UniversalCourseCode="MAT102_ADV" },
                        new EICourse { Code = "PHY101", Name = "Physics 101", UniversalCourseCode="PHY101_BASIC" },
                        new EICourse { Code = "PROG101", Name = "Programming 101", UniversalCourseCode="PROG101_BASIC" },
                        new EICourse { Code = "PROG102", Name = "Programming 102", UniversalCourseCode="PROG102_BASIC" }
                    },
                    Programs = new System.Collections.Generic.List<EIProgram> {
                        new EIProgram { Code = "MSC_SE", Name = "Masters in Software Engineering",
                            ProgramCourses = new System.Collections.Generic.List<EIProgramRequiredCourse> {
                                new EIProgramRequiredCourse { Code = "MAT101" },
                                new EIProgramRequiredCourse { Code = "MAT102" },
                                new EIProgramRequiredCourse { Code = "PROG101" },
                                new EIProgramRequiredCourse { Code = "PROG103" }
                            }
                        },
                        new EIProgram { Code = "MSC_PHY", Name = "Masters in Physics",
                            ProgramCourses = new System.Collections.Generic.List<EIProgramRequiredCourse> {
                                new EIProgramRequiredCourse { Code = "MAT101" },
                                new EIProgramRequiredCourse { Code = "MAT102" },
                                new EIProgramRequiredCourse { Code = "PHY101" },
                                new EIProgramRequiredCourse { Code = "PHY102" }
                            }
                        }
                    },
                    Students = new System.Collections.Generic.List<EIStudent> {
                        new EIStudent { StudentId = "CAM123", Firstname = "Omar", Lastname = "AL Zabir",
                            CoursesTaken = new System.Collections.Generic.List<EIStudentCourseTaken> {
                                new EIStudentCourseTaken { CourseCode = "MAT101", Score=3.5f, Grade="A", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new EIStudentCourseTaken { CourseCode = "MAT102", Score=3.0f, Grade="B", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new EIStudentCourseTaken { CourseCode = "PHY101", Score=4.0f, Grade="A+", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                            }
                        }
                    }
                });
        }
    }
}