using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class StudentProgramViewModel
    {
        public EIStudent Student { get; set; }
        public EIProgram Program { get; set; }
        public EIStudentEnrolledProgram EnrolledProgram { get; set; }
        public List<EIProgramRequiredCourse> ProgramCourses { get; set; }
        public List<CourseCreditedTowardsProgram> CourseCredited { get; set; }
        public List<EducationalInstitute> EducationalInstitutes { get; set; }
        public List<EICourse> CoursesInEI { get; set; }
    }
}