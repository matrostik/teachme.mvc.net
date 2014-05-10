using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class Teacher
    {

        #region Fields
        
        public int Id { get; set; }

        //switch on/off for profile
        public bool isActive { get; set; } 

        public string PictureUrl { get; set; }

        public int Age{ get; set; }
        
        public string City{ get; set; }

        public string Street{ get; set; }

        public int HomeNum { get; set; }

        public int LessonPrice{ get; set; }

        public int LessonTime { get; set; }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", HtmlEncode = true, ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        public string UserId { get; set; }

        public string SubjectToTeachId { get; set; }

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