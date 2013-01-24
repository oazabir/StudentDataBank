using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models
{
    [DataContract(Namespace = "http://universalaward.org", Name="Course")]
    public class ProgramCourse
    {
        [Key]
        public int Id { get; set; }

        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }

        public Program Program { get; set; }        
    }
}