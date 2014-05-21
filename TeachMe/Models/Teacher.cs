using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class Teacher
    {

        #region Fields
        
        public int Id { get; set; }

        [Display(Name = "הפעל פרופיל")]
        public bool isActive { get; set; }

        [Required(ErrorMessage = "* יש להכניס תמונה")]
        [Display(Name = "תמונת פרופיל")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "* יש להכניס גיל")]
        [Range(1, 120, ErrorMessage = "יש להכניס גיל")]
        [Display(Name = "גיל")]
        public int Age{ get; set; }

        [Required(ErrorMessage = "* יש להכניס עיר")]
        [Display(Name = "עיר")]
        public string City{ get; set; }

        [Required(ErrorMessage = "* יש להכניס רחוב")]
        [Display(Name = "רחוב")]
        public string Street{ get; set; }

        [Required(ErrorMessage = "* יש להכניס מספר בית")]
        [Range(1, int.MaxValue, ErrorMessage = "יש להכניס מספר בית")]
        [Display(Name = "מספר בית")]
        public int HomeNum { get; set; }

        [Required(ErrorMessage = "* יש להכניס מחיר")]
        [Range(1, 500, ErrorMessage = "יש להכניס מחיר לשיעור")]
        [Display(Name = "מחיר לשיעור")]
        public int LessonPrice{ get; set; }

        [Range(30, 90, ErrorMessage = "יש לבחור ערך")]
        [Required(ErrorMessage = "* יש להכניס את זמן השיעור  ")]
        [Display(Name = "זמן השיעור (בדקות)")]
        public int LessonTime { get; set; }

        [Required(ErrorMessage = "* יש להכניס טלפון  ")]
        [Display(Name = "טלפון ")]
        public string Phone{ get; set; }

        [Display(Name = "השכלה ")]
        public string Education { get; set; }

        [Display(Name = "מוסד לימודים")]
        public string Institution { get; set; }

        /*  temporary object for subject to teach (for FakeDB) */
        [NotMapped]
        public string Category { get; set; }
        /*  temporary object for subject to teach */

        [Display(Name = "על עצמי... ")]
        public string About { get; set; }

        public int Raters { get; set; }

        public int Rating { get; set; }

        public string ApplicationUserId { get; set; }

        public int GeoLocationId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual List<SubjectToTeach> SubjectsToTeach { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }

        public virtual List<Comment> Comments { get; set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Returns full address of teacher
        /// City , Street 10
        /// Needed for google map
        /// </summary>
        /// <returns></returns>
        public string GetAddressForMap()
        {
            return City + "," + Street + " " + HomeNum;
        }

        /// <summary>
        /// Return teacher's full name
        ///  Firstname Lastname
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return User.FirstName + " " + User.LastName;
        }

        public string GetSubjects()
        {
            return string.Join(",",SubjectsToTeach.Select(x=>x.Name));
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}