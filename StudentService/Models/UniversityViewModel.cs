using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class UniversityViewModel
    {
        public University University { get; set; }
        public IQueryable<UniversityCourse> Courses { get; set; }
        public IQueryable<Program> Programs { get; set; }
        public IQueryable<UniversityStudent> Students { get; set; }
    }
}