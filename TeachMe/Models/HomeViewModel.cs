using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeachMe.Models
{
    public class HomeViewModel
    {
        /// <summary>
        /// List of cities
        /// </summary>
        public List<SelectListItem> Cities { get; set; }

        /// <summary>
        /// List of subjects to teach
        /// </summary>
        public List<SelectListItem> Subjects { get; set; }

        /// <summary>
        /// List of distances to search for
        /// </summary>
        public List<SelectListItem> Distances { get; set; }

        /// <summary>
        /// List of new teachers
        /// </summary>
        public List<Teacher> NewTeachers { get; set; }

        /// <summary>
        /// List of most rated teachers
        /// </summary>
        public List<Teacher> MostRatedTeachers { get; set; }

        /// <summary>
        /// List of most commented teachers
        /// </summary>
        public List<Teacher> MostCommentedTeachers { get; set; }
    }
}