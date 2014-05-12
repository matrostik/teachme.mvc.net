using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using TeachMe.Helpers;
using Microsoft.AspNet.Identity;

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
        public ActionResult Index(int? id)
        {

            // temporary logic must be current user
            Teacher t;
            if (User.Identity.IsAuthenticated)
            {
                var id1 = User.Identity.GetUserId();
                t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            }
            else
                t = Db.Teachers.FirstOrDefault(x => x.Id == id);
            if (t == null)
                t = Db.Teachers.FirstOrDefault();

            return View(t);
        }


        //
        // GET: /Create/
        public ActionResult Create()
        {
            var model = new CreateProfileViewModel();
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
            model.Cats = items;

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
            model.Cities = items;

            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            items.Add(new SelectListItem() { Text = "100 דקות", Value = "100" });
            items.Add(new SelectListItem() { Text = "120 דקות", Value = "120" });
            model.Time = items;


            //model.Institutions = Db.Institutions.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Name
            //}).ToList();

            List<GroupDropListItem> l = Db.Institutions.OrderBy(x=>x.Id).GroupBy(x => x.Type)
                .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList()}).ToList();
            model.Institutions = l;

            return View(model);
        }

        //
        // Post: /Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProfileViewModel model)
        {

            if (ModelState.IsValid)
            {
                Teacher t = new Teacher();
                t.Age = model.Age.Value;
                t.City = model.City;
                t.Street = model.Street;
                t.HomeNum = model.HomeNum.Value;
                t.Category = model.Category;
                t.LessonPrice = model.LessonPrice.Value;
                t.LessonTime = int.Parse(model.LessonTime);
                t.Education = model.Education;
                t.Institution = "";
                t.About = model.About;
                t.Phone = model.Phone;

                t.GeoLocation = new GeoLocation(t.GetAddressForMap());
                if (User.Identity.IsAuthenticated)
                    t.ApplicationUserId = User.Identity.GetUserId();

                Db.Teachers.Add(t);
                Db.SaveChanges();

                return RedirectToAction("Index", "Profile", new { id = t.Id });

            }
            else
            {
                if (ModelState["HomeNum"].Errors.Count > 0 && ModelState["HomeNum"].Errors.FirstOrDefault().ErrorMessage.Contains("is not valid"))
                {
                    ModelState["HomeNum"].Errors.Clear();
                    ModelState.AddModelError("HomeNum", "* רק מספרים");
                }
            }

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
            model.Cats = items;

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
            model.Cities = items;

            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            items.Add(new SelectListItem() { Text = "100 דקות", Value = "100" });
            items.Add(new SelectListItem() { Text = "120 דקות", Value = "120" });
            model.Time = items;

            List<GroupDropListItem> l = Db.Institutions.OrderBy(x => x.Id).GroupBy(x => x.Type)
                 .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList() }).ToList();
            model.Institutions = l;

            return View(model);
        }
    }
}