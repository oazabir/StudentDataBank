using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentService.Models.Entity;

namespace StudentService.Models
{
    public class EIViewModel
    {
        public EducationalInstitute EducationalInstitute { get; set; }
        public IQueryable<EICourse> Courses { get; set; }
        public IQueryable<EIProgram> Programs { get; set; }
        public IQueryable<EIStudent> Students { get; set; }
    }
}