
using System.Web;
using System.Xml;
namespace TeachMe.Models
{
    /// <summary>
    /// GeoLocation class that represent 
    /// geolocation coordinates of teachers
    /// address or cities
    /// </summary>
    public class GeoLocation
    {
        public GeoLocation()
        {
        }

        public GeoLocation(string address)
        {
            var g = GetLongitudeAndLatitude(address);
            if(g!=null)
            {
                Latitude = g.Latitude;
                Longitude = g.Longitude;
            }
        }
        public int Id { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }




        #region Methods

        public bool isExist()
        {
            return (Longitude != null && Latitude != null);
        }

        public static GeoLocation GetLongitudeAndLatitude(string address)
        {
            string urlAddress = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.UrlEncode("ישראל," + address)
                + "&sensor=false&language=he";
            try
            {
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(urlAddress);
                XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");

                GeoLocation geo = new GeoLocation();
                foreach (XmlNode objXmlNode in objXmlNodeList)
                {
                    // GET LATITUDE 
                    geo.Latitude = objXmlNode.ChildNodes.Item(0).InnerText;
                    // GET LONGITUDE 
                    geo.Longitude = objXmlNode.ChildNodes.Item(1).InnerText;
                }
                return geo;
            }
            catch
            {
                return null;
            }

        }

        #endregion

    }
}