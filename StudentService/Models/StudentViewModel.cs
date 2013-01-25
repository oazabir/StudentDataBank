using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class StudentViewModel
    {
        public UniversityStudent Student { get; set; }
        public IQueryable<StudentCourse> Courses { get; set; }
        public IQueryable<StudentProgram> Programs { get; set; }
        public IQueryable<UniversityStudentLink> Links { get; set; }
    }
}