using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    /// <summary>
    /// GeoLocation class that represent 
    /// geolocation coordinates of teachers
    /// address
    /// </summary>
    public class GeoLocation
    {
        public int Id { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }
}