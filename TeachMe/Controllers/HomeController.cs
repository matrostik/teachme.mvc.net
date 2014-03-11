using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeachMe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "TeachMe";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "TeachMe contact page.";

            return View();
        }
    }
}