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

        //switch on/off for profile
        public bool isActive { get; set; }

        [Required(ErrorMessage = "* יש להכניס תמונה")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "* יש להכניס גיל")]
        public int Age{ get; set; }

        [Required(ErrorMessage = "* יש להכניס עיר")]
        public string City{ get; set; }

        [Required(ErrorMessage = "* יש להכניס רחוב")]
        public string Street{ get; set; }

        [Required(ErrorMessage = "* יש להכניס מספר בית")]
        public int HomeNum { get; set; }

        [Required(ErrorMessage = "* יש להכניס מחיר")]
        public int LessonPrice{ get; set; }

        [Required(ErrorMessage = "* יש להכניס זמן השיעור")]
        public int LessonTime { get; set; }

        [Required(ErrorMessage = "* יש להכניס טלפון")]
        public string Phone{ get; set; }

        public string Education { get; set; }

        public string Institution { get; set; }

        /*  temporary object for subject to teach (for FakeDB) */
        [NotMapped]
        public string Category { get; set; }
        /*  temporary object for subject to teach */

        public string About { get; set; }

        public int Raters { get; set; }

        public int Rating { get; set; }

        public string ApplicationUserId { get; set; }

        public int GeoLocationId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "* יש להכניס מקצוע לימוד ")]
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