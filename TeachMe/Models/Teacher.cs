using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class Teacher
    {

        #region Fields
        
        public int Id { get; set; }

        //switch on/off for profile
        public bool isActivated { get; set; } 

        public string FirstName { get; set; }
       
        public string LastName{ get; set; }

        public string PictureUrl { get; set; }

        public int Age{ get; set; }
        
        public string City{ get; set; }

        public string Street{ get; set; }

        public int HomeNum { get; set; }

        // subject to teach
        public string Category { get; set; }

        public int LessonPrice{ get; set; }

        public string Phone{ get; set; }

        public string Education { get; set; }

        public string About { get; set; }

        public string Raters { get; set; }

        public decimal Rating { get; set; }




        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", HtmlEncode = true, ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        public GeoLocation GeoLocation { get; set; }

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
            return FirstName + " " + LastName;
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