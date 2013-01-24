using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public IQueryable<StudentCourse> Courses { get; set; }
        public IQueryable<StudentProgram> Programs { get; set; }
        public IQueryable<StudentLink> Links { get; set; }
    }
}