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
        public ActionResult Index(string category, string city, string firstName, string lastName)
        {

            FakeDB db = new FakeDB();
            var s1 = db.Teachers.Where(t => t.Category.Contains(category) ).ToList();
            var s2 = db.Teachers.Where(t => t.City.Contains(city)).ToList();

            var s3 = db.Teachers.Where(t => t.Category.Contains(category) && t.City.Contains(city)).ToList();

            ViewBag.Count = s2.Count; 
            ViewBag.Result = s2;
            return View();
        }
	}
}