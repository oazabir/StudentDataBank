using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace = "http://universalaward.org")]
    public class StudentUserMap
    {
        [Key]
        public int Id { get; set; }

        public StudentUser StudentUser { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string StudentId { get; set; }

        [DataMember][Required][StringLength(50)]
        public string UniversityCode { get; set; }

        [ForeignKey("RegisteredUniversity")]
        public int RegisteredUniversity_Id { get; set; }

        public University RegisteredUniversity { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public bool Approved { get; set; }
    }
}