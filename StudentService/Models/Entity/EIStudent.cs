using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace = "http://studentdatabank.org", Name="Student")]
    public class EIStudent
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string Firstname { get; set; }
            
        [DataMember][Required][StringLength(50)]
        public string Lastname { get; set; }
        
        [DataMember][Required][StringLength(50)]
        public string StudentId { get; set; }

        public EducationalInstitute EducationalInstitute { get; set; }

        [DataMember]
        public List<StudentLinkToOtherEI> LinksToOtherEI { get; set; }

        [DataMember]
        public List<EIStudentCourseTaken> CoursesTaken { get; set; }

        [DataMember]
        public List<EIStudentEnrolledProgram> Programs { get; set; }

    }
}