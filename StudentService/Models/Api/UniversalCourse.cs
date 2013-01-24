using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models
{
    public class UniversalCourse
    {
        [Key]
        public int Id { get; set; }
        
        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }
        
        [DataMember][StringLength(255)][Required]
        public string Name { get; set; }

        [DataMember][StringLength(255)][Required]
        public string Description { get; set; }
    }
}