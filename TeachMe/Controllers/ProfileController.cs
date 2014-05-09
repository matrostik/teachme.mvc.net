using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class ProfileController : Controller
    {
        public TeachMeDBContext Db { get; private set; } 

        public ProfileController()
        {
            Db = new TeachMeDBContext();
        }
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            return View();
        }



        //
        // GET: /Create/
        public ActionResult Create()
        {
            var model = new CreateProfileModel();
            var cats = FakeDB.Cat;
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < cats.Count; i++)
            {
                items.Add(new SelectListItem
                {
                    Text = cats[i],
                    Value = cats[i]
                });
            }
            ViewBag.Cats = items;

            var cities = FakeDB.Cities;
            items = new List<SelectListItem>();
            for (int i = 0; i < cities.Count; i++)
            {
                items.Add(new SelectListItem
                {
                    Text = cities[i],
                    Value = cities[i]
                });
            }
            ViewBag.Cities = items;
            return View(model);
        }

        //
        // Post: /Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProfileModel model)
        {
           

            if (ModelState.IsValid)
            {
                Teacher t = new Teacher();
                t.Age = model.Age;
                t.City = model.City;
                t.Street = model.Street;
                t.HomeNum = model.HomeNum;
                t.Category = model.Category;
                t.LessonPrice = model.LessonPrice;
                t.LessonTime = model.LessonTime;
                t.Education = model.Education;
                t.About = model.About;
                t.Phone = model.Phone;

                Db.Teachers.Add(t);
                Db.SaveChanges();
                return RedirectToAction("Index", "Profile");

            }
            return View(model);
        }
	}
}