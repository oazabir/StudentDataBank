using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace StudentService.Models
{
    [DataContract(Namespace = "http://universalaward.org")]
    public class Program
    {
        [Key]
        public int Id { get; set; }

        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }
        
        [DataMember][StringLength(255)][Required]
        public string Name { get; set; }

        public University University { get; set; }

        [DataMember]
        public List<ProgramCourse> ProgramCourses { get; set; }
    }
}