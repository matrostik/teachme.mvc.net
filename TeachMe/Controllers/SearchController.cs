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
        public ActionResult Index(string firstName, string lastName, string sortOrder, int? page)
        {
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;

            List<Teacher> list = new List<Teacher>();
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName) && t.LastName.Contains(lastName)))).ToList();
            else if (!string.IsNullOrEmpty(firstName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName)))).ToList();
            else if (!string.IsNullOrEmpty(lastName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.LastName.Contains(lastName)))).ToList();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                list = FakeDB.Teachers;

            //sorting
            if (!string.IsNullOrEmpty(sortOrder))
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
                        list = list.OrderBy(t => t.LessonPrice).ToList();
                        break;
                    case "priceDown":
                        list = list.OrderByDescending(t => t.LessonPrice).ToList();
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
        // GET: /Map/city/category/
        public ActionResult Map(string city, string category)
        {
            List<Teacher> teachers;
            // search for teachers
            if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(category))
                teachers = FakeDB.Teachers.Where(t => t.City.Contains(city) && t.Category.Contains(category)).ToList();
            else if (!string.IsNullOrEmpty(city))
                teachers = FakeDB.Teachers.Where(t => t.City.Contains(city)).ToList();
            else if (!string.IsNullOrEmpty(category))
                teachers = FakeDB.Teachers.Where(t => t.Category.Contains(category)).ToList();
            else
                teachers = new List<Teacher>();

            foreach (var t in teachers)
            {
                // get geo by address
                var geo = GetLongitudeAndLatitude(t.GetAddressForMap());
                geo.TeacherId = t.Id;
                t.GeoLocation = geo;
            }
            //create and set model
            var model = new MapSearchViewModel();
            // map settings
            if (!string.IsNullOrEmpty(city))
            {
                model.MapCenter = GetLongitudeAndLatitude(city);
                model.MapZoom = 12;
            }
            else
            {
                model.MapCenter = GetLongitudeAndLatitude("");
                model.MapZoom = 8;
            }
            // data
            model.Teachers = teachers;
            model.ResultCount = teachers.Count;
            // pass model to view
            return View(model);
        }


        //
        // GET: /Geo/  test get geolocation
        public ActionResult Geo(string id, string address, List<Geo> geos)
        {
            // get teacher
            var teacher = FakeDB.Teachers.FirstOrDefault(t => t.Id == int.Parse(id));

            //get teacher address
            if (teacher != null)
                address = teacher.GetAddressForMap();
            if (geos == null)
                geos = new List<Geo>();

            // get geolocation
            var res = GetGeo(address);
            geos.Add(res);

            ViewBag.Geo = res;
            ViewBag.Geos = geos;
            return View();
        }

        public GeoLocation GetLongitudeAndLatitude(string address)
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

        public Geo GetGeo(string address)
        {
            string urlAddress = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.UrlEncode("ישראל," + address)
                + "&sensor=false&language=he";
            try
            {
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(urlAddress);
                XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");

                Geo geo = new Geo();
                geo.Address = address;
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