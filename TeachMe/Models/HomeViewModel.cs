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


    }
}