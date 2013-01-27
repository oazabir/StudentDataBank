using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class StudentViewModel
    {
        public EIStudent Student { get; set; }
        public IQueryable<EIStudentCourseTaken> Courses { get; set; }
        public IQueryable<EIStudentEnrolledProgram> Programs { get; set; }
        public IQueryable<StudentLinkToOtherEI> Links { get; set; }
    }
}