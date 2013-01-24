using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models
{
    [DataContract(Namespace = "http://universalaward.org")]
    public class StudentLink
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string UniversityCode { get; set; }

        [DataMember][Required][StringLength(50)]
        public string StudentId { get; set; }

        public Student Student { get; set; }
    }
}