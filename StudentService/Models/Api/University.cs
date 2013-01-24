using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace StudentService.Models
{
    [DataContract(Namespace="http://universalaward.org")]
    public class University
    {
        [Key]
        public int Id { get; set; }

        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }

        [DataMember][StringLength(255)][Required]
        public string Name { get; set; }
        
        [DataMember][StringLength(255)][Required]
        public string Address { get; set; }
        
        [DataMember]
        public List<Program> Programs { get; set; }

        [DataMember]
        public List<Student> Students { get; set; }

        [DataMember]
        public List<UniversityCourse> Courses { get; set; }
    }
}