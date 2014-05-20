using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using TeachMe.Helpers;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace TeachMe.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public TeachMeDBContext Db { get; private set; }

        public ProfileController()
        {
            Db = new TeachMeDBContext();
        }

        //
        // GET: /Profile/Index
        public ActionResult Index(int? id)
        {
            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            if (t == null)
                return RedirectToAction("Create");
            return View(t);
        }

        //
        // GET: /Create/
        public ActionResult Create()
        {
            var model = new CreateProfileViewModel();
            model.Subjects = Db.Subjects.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.Cities = Db.Cities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            model.Time = items;

            model.Institutions = Db.Institutions.OrderBy(x => x.Id).GroupBy(x => x.Type)
                .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList() }).ToList();

            var id = User.Identity.GetUserId();
            model.User = Db.Users.FirstOrDefault(x => x.Id == id);

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
                t.LessonPrice = model.LessonPrice.Value;
                t.LessonTime = int.Parse(model.LessonTime);
                t.Education = model.Education;
                t.Institution = model.Institution;
                t.About = model.About;
                t.Phone = model.Phone;
                t.PictureUrl = model.PictureUrl;

                t.SubjectsToTeach = new List<SubjectToTeach>();
                foreach (var idx in model.SubjectsId)
                {
                    int i = int.Parse(idx);
                    var subj = Db.Subjects.FirstOrDefault(x => x.Id == i);
                    SubjectToTeach stt = new SubjectToTeach();
                    stt.Name = subj.Name;
                    t.SubjectsToTeach.Add(stt);
                }

                // get user geolocation by address
                t.GeoLocation = new GeoLocation(t.GetAddressForMap());
                if (User.Identity.IsAuthenticated)
                    t.ApplicationUserId = User.Identity.GetUserId();

                // add teacher to collection
                Db.Teachers.Add(t);
                // save changes to db
                Db.SaveChanges();

                return RedirectToAction("Index", "Profile", new { id = t.Id });

            }
            else
            {
                if (ModelState["HomeNum"].Errors.Count > 0 
                    && ModelState["HomeNum"].Errors.FirstOrDefault().ErrorMessage.Contains("is not valid"))
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
            model.Subjects = items;

            model.Cities = Db.Cities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            
            model.Time = items;

            model.Institutions = Db.Institutions.OrderBy(x => x.Id).GroupBy(x => x.Type)
                 .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList() }).ToList();

            return View(model);
        }

        public ActionResult AutocompleteStreet(string term)
        {
            var filteredItems = Db.Streets.Where(item => item.Name.StartsWith(term)).Distinct().ToList();
            List<string> res = filteredItems.Select(x => x.Name).Take(20).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Edit()
        {
            EditProfileViewModel model = new EditProfileViewModel();
            model.Subjects = Db.Subjects.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.Cities = Db.Cities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            model.Time = items;

            model.Institutions = Db.Institutions.OrderBy(x => x.Id).GroupBy(x => x.Type)
                .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList() }).ToList();

            var id = User.Identity.GetUserId();

            model.teacher = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {

                //save logic
                Db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ContentResult UploadImage(HttpPostedFileBase file)
        {
            string imgurl = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    // get image and resize it
                    Image image = Image.FromStream(ms);
                    Image resized = ImageUtils.ResizeImageFixedWidth(image, 300);
                    byte[] imageByte = ImageUtils.ImageToByteArraybyMemoryStream(resized);
                    //byte[] array = ms.GetBuffer();
                    // upload image to imgur.com
                    imgurl = Utils.UploadImageToImgur(imageByte);
                }
            }
            var res = new UploadFilesResult()
            {
                name = file.FileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = imgurl
            };

            string json = JsonConvert.SerializeObject(res);
            return Content(json, "application/json");
        }

    }
}