using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class MapSearchViewModel
    {
        /// <summary>
        /// Search result
        /// Collection of teachers
        /// </summary>
        public List<Teacher> Teachers { get; set; }

        /// <summary>
        /// Search result count
        /// </summary>
        public int ResultCount { get; set; }

        /// <summary>
        /// Geolocation to hold
        /// center of map
        /// </summary>
        public GeoLocation MapCenter { get; set; }

        /// <summary>
        /// Map zoom
        /// </summary>
        public int MapZoom { get; set; }

        public string SearchFor { get; set; }
    }
}