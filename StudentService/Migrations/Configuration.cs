namespace StudentService.Migrations
{
    using StudentService.Models;
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
            
            context.Universities.AddOrUpdate(
                u => u.Id,
                new University
                {
                    Code = "OX",
                    Name = "Oxford University",
                    Address = "Oxfordshire, UK",
                    Courses = new System.Collections.Generic.List<UniversityCourse> {
                        new UniversityCourse { Code = "MAT101", Name = "Mathematics 101", UniversalCourseCode="MAT101_BASIC" },
                        new UniversityCourse { Code = "MAT102", Name = "Mathematics 102", UniversalCourseCode="MAT102_BASIC" },
                        new UniversityCourse { Code = "PHY101", Name = "Physics 101", UniversalCourseCode="PHY101_BASIC" },
                        new UniversityCourse { Code = "PROG101", Name = "Programming 101", UniversalCourseCode="PROG101_BASIC" },
                        new UniversityCourse { Code = "PROG102", Name = "Programming 102", UniversalCourseCode="PROG102_BASIC" }
                    },
                    Programs = new System.Collections.Generic.List<Program> {
                        new Program { Code = "MSC_SE", Name = "Masters in Software Engineering", 
                            ProgramCourses = new System.Collections.Generic.List<ProgramCourse> {
                                new ProgramCourse { Code = "MAT101" },
                                new ProgramCourse { Code = "MAT102" },
                                new ProgramCourse { Code = "PHY101" },
                                new ProgramCourse { Code = "PROG101" },
                                new ProgramCourse { Code = "PROG102" }
                            }
                        },
                        new Program { Code = "MSC_PHY", Name = "Masters in Physics",
                            ProgramCourses = new System.Collections.Generic.List<ProgramCourse> {
                                new ProgramCourse { Code = "MAT101" },
                                new ProgramCourse { Code = "MAT102" },
                                new ProgramCourse { Code = "PHY101" },
                                new ProgramCourse { Code = "PHY102" }
                            }
                        }
                    },
                    Students = new System.Collections.Generic.List<Student>
                    {
                        new Student { StudentId = "OX123", Firstname = "Omar", Lastname = "AL Zabir",
                            LinksToOtherUniversity = new System.Collections.Generic.List<StudentLink> {
                                new StudentLink { UniversityCode = "CAM", StudentId="CAM123" }
                            },
                            CoursesTaken = new System.Collections.Generic.List<StudentCourse> {
                                new StudentCourse { CourseCode="PROG101", Score=3.5f, Grade="A", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new StudentCourse { CourseCode="PROG102", Score=3.0f, Grade="B", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                            },
                            Programs = new System.Collections.Generic.List<StudentProgram> {
                                new StudentProgram { 
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
                new University
                {
                    Code = "CAM",
                    Name = "Cambridge University",
                    Address = "Cambridge, UK",
                    Courses = new System.Collections.Generic.List<UniversityCourse> {
                        new UniversityCourse { Code = "MAT101", Name = "Mathematics 101", UniversalCourseCode="MAT101_BASIC" },
                        new UniversityCourse { Code = "MAT102", Name = "Mathematics 102", UniversalCourseCode="MAT102_ADV" },
                        new UniversityCourse { Code = "PHY101", Name = "Physics 101", UniversalCourseCode="PHY101_BASIC" },
                        new UniversityCourse { Code = "PROG101", Name = "Programming 101", UniversalCourseCode="PROG101_BASIC" },
                        new UniversityCourse { Code = "PROG102", Name = "Programming 102", UniversalCourseCode="PROG102_BASIC" }
                    },
                    Programs = new System.Collections.Generic.List<Program> {
                        new Program { Code = "MSC_SE", Name = "Masters in Software Engineering",
                            ProgramCourses = new System.Collections.Generic.List<ProgramCourse> {
                                new ProgramCourse { Code = "MAT101" },
                                new ProgramCourse { Code = "MAT102" },
                                new ProgramCourse { Code = "PROG101" },
                                new ProgramCourse { Code = "PROG103" }
                            }
                        },
                        new Program { Code = "MSC_PHY", Name = "Masters in Physics",
                            ProgramCourses = new System.Collections.Generic.List<ProgramCourse> {
                                new ProgramCourse { Code = "MAT101" },
                                new ProgramCourse { Code = "MAT102" },
                                new ProgramCourse { Code = "PHY101" },
                                new ProgramCourse { Code = "PHY102" }
                            }
                        }
                    },
                    Students = new System.Collections.Generic.List<Student> {
                        new Student { StudentId = "CAM123", Firstname = "Omar", Lastname = "AL Zabir",
                            CoursesTaken = new System.Collections.Generic.List<StudentCourse> {
                                new StudentCourse { CourseCode = "MAT101", Score=3.5f, Grade="A", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new StudentCourse { CourseCode = "MAT102", Score=3.0f, Grade="B", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                                new StudentCourse { CourseCode = "PHY101", Score=4.0f, Grade="A+", StartDate=DateTime.Parse("1/1/2001"), EndDate=DateTime.Parse("3/3/2001"),Status= CourseStatusEnum.Completed},
                            }
                        }
                    }
                });
        }
    }
}