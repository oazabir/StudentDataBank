using StudentService.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentService.Models
{
    public class NewLinkViewModel
    {
        public EIStudent Student { get; set; }

        public SelectListItem[] Institutes { get; set; }
        public string SelectedInstitute { get; set; }

        [Required][StringLength(50)][Display(Name = "Institution provided Student ID")]        
        public string InstitutionStudentID { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Date of Birth"), DisplayFormat(DataFormatString="dd/mm/yyyy")]
        public DateTime DateOfBirth { get; set; }

    }
}