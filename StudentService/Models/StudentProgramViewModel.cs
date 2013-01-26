using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class StudentProgramViewModel
    {
        public UniversityStudent Student { get; set; }
        public UniversityProgram Program { get; set; }
        public IQueryable<UniversityProgramCourse> ProgramCourses { get; set; }
        public IQueryable<CourseCreditedTowardsProgram> CourseCredited { get; set; }
        public List<University> Universities { get; set; }
    }
}