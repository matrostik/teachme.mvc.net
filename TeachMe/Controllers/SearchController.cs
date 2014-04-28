using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeachMe.Models;
using PagedList;
using System.Xml;
using System.Web;

namespace TeachMe.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index(string category, string city, string firstName, string lastName, string sortOrder, int? page)
        {

            ViewBag.Category = category;
            ViewBag.City = city;
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;

            List<Teacher> list = new List<Teacher>();
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(city))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.Category.Contains(category) && t.City.Contains(city)))).ToList();
            else if (!string.IsNullOrEmpty(category))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.Category.Contains(category)))).ToList();
            else if (!string.IsNullOrEmpty(city))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.City.Contains(city)))).ToList();
            else if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName) && t.LastName.Contains(lastName)))).ToList();
            else if (!string.IsNullOrEmpty(firstName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName)))).ToList();
            else if (!string.IsNullOrEmpty(lastName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.LastName.Contains(lastName)))).ToList();

            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(city)
                && string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                list = FakeDB.Teachers;

            //sorting
            if(!string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.SortParm = sortOrder;
                switch (sortOrder)
                {
                    case "firstName":
                        list = list.OrderBy(t => t.FirstName).ToList();
                        break;
                    case "lastName":
                        list = list.OrderBy(t => t.LastName).ToList();
                        break;
                    case "priceUp":
                        list = list.OrderBy(t => t.Price).ToList();
                        break;
                    case "priceDown":
                        list = list.OrderByDescending(t => t.Price).ToList();
                        break;
                    default:
                        break;
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Count = list.Count;
            ViewBag.Result = list.ToPagedList(pageNumber, pageSize);
            return View();
        }

         //
        // GET: /Geo/  test get geolocation
        public ActionResult Geo(string id)
        {
            // get teacher
            var teacher = FakeDB.Teachers.FirstOrDefault(t => t.Id == int.Parse(id));
            //get teacher address
            string adr = "ירושלים, יפו 224";
            // get geolocation
            var res = GetLongitudeAndLatitude(adr);

            ViewBag.Address = adr;
            ViewBag.Geo = res;
            return View();
        }

        public Geo GetLongitudeAndLatitude(string address)
        {
            string urlAddress = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.UrlEncode(address)
                + "&sensor=false&language=he";
            try
            {
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(urlAddress);
                XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");

                Geo geo = new Geo();
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
    }
}