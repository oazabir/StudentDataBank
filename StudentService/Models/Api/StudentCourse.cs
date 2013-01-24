using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models
{
    public enum CourseStatusEnum
    {
        InProgress = 0,
        Dropped = 1,
        Completed = 2,
        Retake = 3
    }
    [DataContract(Namespace = "http://universalaward.org")]
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string CourseCode { get; set; }

        [DataMember(IsRequired=true)][Required]
        public float Score { get; set; }

        [DataMember(IsRequired=true)][Required]
        public string Grade { get; set; }

        [DataMember(IsRequired=true)][Required]
        public CourseStatusEnum Status { get; set; }

        [DataMember(IsRequired=true)][Required]
        public DateTime StartDate { get; set; }
        
        [DataMember(IsRequired=true)][Required]
        public DateTime EndDate { get; set; }
        
        
        public Student Student { get; set; }        
    }
}