using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Helpers;


namespace TeachMe.Models
{
    public class CreateProfileViewModel
    {
        [Required(ErrorMessage = "* יש להכניס גיל")]
        [Display(Name = "גיל")]
        [Range(15, 120, ErrorMessage = "יש להכניס גיל")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "* יש להכניס תמונה")]
        [Display(Name = "תמונת פרופיל")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "* יש להכניס עיר")]
        [Display(Name = "עיר")]
        public string City { get; set; }

        [Required(ErrorMessage = "* יש להכניס רחוב")]
        [Display(Name = "רחוב")]
        public string Street { get; set; }

        [Required(ErrorMessage = "* יש להכניס מספר בית")]
        [Display(Name = "מספר בית")]
        [Range(1, int.MaxValue, ErrorMessage = "יש להכניס מספר בית")]
        public int? HomeNum { get; set; }

        [Required(ErrorMessage = "* יש להכניס תחום לימוד")]
        [Display(Name = "תחום הלימוד")]
        public List<string> SubjectsId { get; set; }

        [Required(ErrorMessage = "* יש להכניס מחיר לשיעור ")]
        [Display(Name = "מחיר לשיעור")]
        [Range(1, 500, ErrorMessage = "יש להכניס מחיר לשיעור")]
        public int? LessonPrice { get; set; }

        [Range(30, 90, ErrorMessage = "יש לבחור ערך")]
        [Required(ErrorMessage = "* יש להכניס את זמן השיעור  ")]
        [Display(Name = "זמן השיעור (בדקות)")]
        public string LessonTime { get; set; }

        [Required(ErrorMessage = "* יש להכניס טלפון  ")]
        [Display(Name = "טלפון ")]
        public string Phone { get; set; }

        [Display(Name = "השכלה ")]
        public string Education { get; set; }

        [Display(Name = "מוסד לימודים")]
        public string Institution { get; set; }

        [Display(Name = "על עצמי... ")]
        public string About { get; set; }

        [Display(Name = "הפעל פרופיל")]
        public bool isActive { get; set; }

        /// <summary>
        /// List of cities
        /// </summary>
        public List<SelectListItem> Cities { get; set; }

        /// <summary>
        /// List of subjects to teach
        /// </summary>
        public List<SelectListItem> Subjects { get; set; }

        /// <summary>
        /// List of lesson time
        /// </summary>
        public List<SelectListItem> Time { get; set; }

        /// <summary>
        /// List of institutions
        /// </summary>
        public List<GroupDropListItem> Institutions { get; set; }

        public ApplicationUser User { get; set; }

    }

    public class EditProfileViewModel
    {
        public Teacher Teacher { get; set; }

        [Required(ErrorMessage = "* יש להכניס תחום לימוד")]
        [Display(Name = "תחום הלימוד")]
        public List<string> SubjectsId { get; set; }
        /// <summary>
        /// List of cities
        /// </summary>
        public List<SelectListItem> Cities { get; set; }

        /// <summary>
        /// List of subjects to teach
        /// </summary>
        public List<SelectListItem> Subjects { get; set; }

        /// <summary>
        /// List of lesson time
        /// </summary>
        public List<SelectListItem> Time { get; set; }

        /// <summary>
        /// List of institutions
        /// </summary>
        public List<GroupDropListItem> Institutions { get; set; }

       
    }
}