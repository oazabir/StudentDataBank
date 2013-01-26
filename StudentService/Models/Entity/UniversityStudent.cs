using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentService.Models.Entity
{
    [DataContract(Namespace = "http://universalaward.org")]
    public class UniversityStudent
    {
        [Key]
        public int Id { get; set; }

        [DataMember][Required][StringLength(50)]
        public string Firstname { get; set; }
            
        [DataMember][Required][StringLength(50)]
        public string Lastname { get; set; }
        
        [DataMember][Required][StringLength(50)]
        public string StudentId { get; set; }

        public University University { get; set; }

        [DataMember]
        public List<StudentLinkToOtherUniversity> LinksToOtherUniversity { get; set; }

        [DataMember]
        public List<StudentCourseTaken> CoursesTaken { get; set; }

        [DataMember]
        public List<StudentProgramEnrollment> Programs { get; set; }

    }
}