using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace = "http://universalaward.org")]
    public class UniversityCourse
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(255)]
        public string Name { get; set; }
        
        [DataMember][Required][StringLength(50)]
        public string Code { get; set; }
        
        [DataMember][Required][StringLength(50)]
        public string UniversalCourseCode { get; set; }

        public University University { get; set; }
    }
}