using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    public enum CourseCreditedStatusEnum
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }

    [DataContract(Namespace = "http://studentdatabank.org")]
    public class CourseCreditedTowardsProgram
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string RequiredCourseCode { get; set; }

        [DataMember][Required][StringLength(50)]
        public string CreditedCourseCode { get; set; }

        [DataMember][Required][StringLength(50)]
        public string CreditedCourseEICode { get; set; }

        [DataMember(IsRequired=true)][Required]
        public float Score { get; set; }

        [DataMember][Required]
        public string Grade { get; set; }

        [DataMember(IsRequired=true)][Required]
        public CourseCreditedStatusEnum Status { get; set; }

        public EIStudentEnrolledProgram StudentProgram { get; set; }

    }
}