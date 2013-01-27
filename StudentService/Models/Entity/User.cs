using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    public enum UserTypeEnum
    {
        Admin,
        University,
        Student
    }
    [DataContract(Namespace = "http://studentdatabank.org")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string Username { get; set; }

        [DataMember][Required][StringLength(50)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)][Required]
        public UserTypeEnum Type { get; set; }
    }
}