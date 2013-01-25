using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models.Entity
{
    public class StudentUser : User
    {
        public List<StudentUserMap> StudentUserMaps { get; set; }
    }
}