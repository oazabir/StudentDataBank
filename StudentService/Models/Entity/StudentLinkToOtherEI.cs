using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    public enum LinkApprovalStatusEnum
    {
        PendingApproval,
        Accepted,
        FetchingRecords,
        RecordsReceived,
        Denied,
        InvalidClaim
    }
    [DataContract(Namespace = "http://studentdatabank.org", Name="LinkToOtherEducationalInstitute")]
    public class StudentLinkToOtherEI
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string EICode { get; set; }

        [DataMember][Required][StringLength(50)]
        public string StudentId { get; set; }

        [DataMember(IsRequired = true)][Required]
        public LinkApprovalStatusEnum Status { get; set; }

        public EIStudent Student { get; set; }        
    }
}