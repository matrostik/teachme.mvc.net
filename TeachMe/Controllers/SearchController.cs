using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index(int? id)
        {
            ViewBag.Count = 0;
            ViewBag.Result = new List<Teacher>();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string category, string city, string firstName, string lastName)
        {
            bool flag = firstName == null;
            List<Teacher> list = new List<Teacher>();
            //search by category and city
            if (flag)
            {
                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(city))
                    list = FakeDB.Teachers.Where(t => t.Category.Contains(category) && t.City.Contains(city)).ToList();
                else if (string.IsNullOrEmpty(category))
                    list = FakeDB.Teachers.Where(t => t.City.Contains(city)).ToList();
                else
                    list = FakeDB.Teachers.Where(t => t.Category.Contains(category)).ToList();
            }
            //search by firstname and lastname
            else
            {
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                    list = FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName) && t.LastName.Contains(lastName)).ToList();
                else if (string.IsNullOrEmpty(firstName))
                    list = FakeDB.Teachers.Where(t => t.LastName.Contains(lastName)).ToList();
                else
                    list = FakeDB.Teachers.Where(t => t.FirstName.Contains(firstName)).ToList();
            }

            ViewBag.Count = list.Count;
            ViewBag.Result = list;
            return View();
        }
    }
}