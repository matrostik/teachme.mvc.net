using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TeachMe.Models
{
    public class CreateProfileViewModel
    {
        [Required(ErrorMessage = "* יש להכניס גיל")]
        [Display(Name = "גיל")]
        [Range(1, 120, ErrorMessage ="יש להכניס גיל")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "* יש להכניס עיר")]
        [Display(Name = "עיר")]
        public string City { get; set; }

        [Required(ErrorMessage = "* יש להכניס רחוב")]
        [Display(Name = "רחוב")]
        public string Street { get; set; }

        [Required(ErrorMessage = "* יש להכניס מספר בית")]
        [Display(Name = "מספר בית")]
        [Range(1, int.MaxValue, ErrorMessage="יש להכניס מספר בית")]
        public int? HomeNum { get; set; }

        [Required(ErrorMessage = "* יש להכניס תחום לימוד")]
        [Display(Name = "תחום הלימוד")]
        public string Category { get; set; }

        [Required(ErrorMessage = "* יש להכניס מחיר לשיעור ")]
        [Display(Name = "מחיר לשיעור")]
        [Range(1, 500, ErrorMessage="יש להכניס מחיר לשיעור")]
        public int? LessonPrice { get; set; }

        [Range(30, 90, ErrorMessage="יש לבחור ערך")]
        [Required(ErrorMessage = "* יש להכניס את זמן השיעור  ")]
        [Display(Name = "זמן השיעור (בדקות)")]
        public string LessonTime { get; set; }

        [Required(ErrorMessage = "* יש להכניס טלפון  ")]
        [Display(Name = "טלפון ")]
        public string Phone { get; set; }

        [Display(Name = "השכלה ")]
        public string Education { get; set; }

        [Display(Name = "על עצמי... ")]
        public string About { get; set; }


        public List<SelectListItem> Cities { get; set; }


        public List<SelectListItem> Cats { get; set; }

        public List<SelectListItem> Time { get; set; }

    }
}