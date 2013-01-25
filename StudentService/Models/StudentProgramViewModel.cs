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
        public Program Program { get; set; }
        public IQueryable<ProgramCourse> ProgramCourses { get; set; }
        public IQueryable<CourseCredited> CourseCredited { get; set; }
        public List<University> Universities { get; set; }
    }
}