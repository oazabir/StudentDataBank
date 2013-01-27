using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace = "http://studentdatabank.org", Name="RequiredCourse")]
    public class EIProgramRequiredCourse
    {
        [Key]
        public int Id { get; set; }

        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }

        public EIProgram Program { get; set; }        
    }
}