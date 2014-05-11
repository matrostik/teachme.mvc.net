﻿
namespace TeachMe.Models
{
    /// <summary>
    /// GeoLocation class that represent 
    /// geolocation coordinates of teachers
    /// address or cities
    /// </summary>
    public class GeoLocation
    {
        public int Id { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }
}