using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace="http://studentdatabank.org")]
    public class EducationalInstitute
    {
        [Key]
        public int Id { get; set; }

        [DataMember][StringLength(50)][Required]
        public string Code { get; set; }

        [DataMember][StringLength(255)][Required]
        public string Name { get; set; }
        
        [DataMember][StringLength(255)][Required]
        public string Address { get; set; }
        
        [DataMember]
        public List<EIProgram> Programs { get; set; }

        [DataMember]
        public List<EIStudent> Students { get; set; }

        [DataMember]
        public List<EICourse> Courses { get; set; }

        [DataMember]
        public List<StudentLinkToOtherEI> StudentClaims { get; set; }
    }
}