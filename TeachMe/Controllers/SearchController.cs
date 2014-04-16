using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeachMe.Models;
using PagedList;

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
            if (!string.IsNullOrEmpty(category))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.Category.Contains(category)))).ToList();
            if (!string.IsNullOrEmpty(city))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.City.Contains(city)))).ToList();
            if (!string.IsNullOrEmpty(firstName))
                list = (list.Union<Teacher>(FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName)))).ToList();
            if (!string.IsNullOrEmpty(lastName))
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
    }
}