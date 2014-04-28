using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class Geo
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("lat: {0} , lng: {1}", Latitude, Longitude);
        }
    }
}