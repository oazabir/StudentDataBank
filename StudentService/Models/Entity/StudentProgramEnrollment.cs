using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    public enum ProgramStatusEnum
    {
        InProgress = 0,
        Terminated = 1,
        Completed = 2,
        Timedout = 3,
        BeingProcessed = 4
    }
    [DataContract(Namespace = "http://universalaward.org")]
    public class StudentProgramEnrollment
    {
        [Key]
        public int Id { get; set; }

        public UniversityStudent Student { get; set; }

        [DataMember][Required][StringLength(50)]
        public string ProgramCode { get; set; }

        [DataMember(IsRequired=true)][Required]
        public DateTime StartDate { get; set; }

        [DataMember(IsRequired = true)][Required]
        public DateTime EndDate { get; set; }

        [DataMember(IsRequired = true)][Required]
        public ProgramStatusEnum Status { get; set; }

        [DataMember(IsRequired = true)][Required]
        public DateTime LastRefreshedAt { get; set; }

        [DataMember(IsRequired = true)][Required]
        public float CGPA { get; set; }

        [DataMember]
        public List<CourseCreditedTowardsProgram> CoursesCredited { get; set; }
    }
}