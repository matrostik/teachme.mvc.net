using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using TeachMe.Helpers;
using Microsoft.AspNet.Identity;
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
        public ActionResult Index()
        {
            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            if (t == null)
                return RedirectToAction("Create");
            return View(t);
        }

        //
        // GET: /Profile/Create/
        public ActionResult Create()
        {
            var model = new CreateProfileViewModel();
            model.Subjects = GetSubjectsSelectedList();
            model.Cities = GetCitiesSelectedList();
            model.Time = GetLessonTimeSelectedList();
            model.Institutions = GetInstitutionsGroupDropList();

            var id = User.Identity.GetUserId();
            model.User = Db.Users.FirstOrDefault(x => x.Id == id);

            return View(model);
        }

        //
        // Post: /Profile/Create/
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
                t.isActive = model.isActive;
                // take care of subjects
                foreach (var idx in model.SubjectsId)
                {
                    int i = int.Parse(idx);
                    var subj = Db.Subjects.FirstOrDefault(x => x.Id == i);
                    SubjectToTeach stt = new SubjectToTeach();
                    stt.SubjectId = int.Parse(idx);
                    stt.Name = subj.Name;
                    t.SubjectsToTeach.Add(stt);
                }

                // get user geolocation by address
                t.GeoLocation = new GeoLocation(t.GetAddressForMap());
                // set relation to user
                if (User.Identity.IsAuthenticated)
                    t.ApplicationUserId = User.Identity.GetUserId();

                // add teacher to collection
                Db.Teachers.Add(t);
                // save changes to db
                Db.SaveChanges();

                return RedirectToAction("Index", "Profile");

            }
            else
            {
                // ovverride not number message
                if (ModelState["HomeNum"].Errors.Count > 0
                    && ModelState["HomeNum"].Errors.FirstOrDefault().ErrorMessage.Contains("is not valid"))
                {
                    ModelState["HomeNum"].Errors.Clear();
                    ModelState.AddModelError("HomeNum", "* רק מספרים");
                }
            }
            // model not valid get data again and return to page
            model.Subjects = GetSubjectsSelectedList();
            model.Cities = GetCitiesSelectedList();
            model.Time = GetLessonTimeSelectedList();
            model.Institutions = GetInstitutionsGroupDropList();

            var id = User.Identity.GetUserId();
            model.User = Db.Users.FirstOrDefault(x => x.Id == id);

            return View(model);
        }

        //
        // GET: /Profile/AutocompleteStreet/
        public ActionResult AutocompleteStreet(string term)
        {
            var filteredItems = Db.Streets.Where(item => item.Name.StartsWith(term)).Distinct().ToList();
            List<string> res = filteredItems.Select(x => x.Name).Take(20).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Profile/Edit/
        public ActionResult Edit(int? id)
        {
            EditProfileViewModel model = new EditProfileViewModel();
            model.Subjects = GetSubjectsSelectedList();
            model.Cities = GetCitiesSelectedList();
            model.Time = GetLessonTimeSelectedList();
            model.Institutions = GetInstitutionsGroupDropList();

            /*************************************************************/
            //if (id.HasValue)
            //{
            //    Teacher tempT = Db.Teachers.FirstOrDefault(x => x.Id == id);
            //    if (tempT == null)
            //        return RedirectToAction("Index", "Home");
            //    model.Teacher = tempT;
            //    model.SubjectsId = model.Teacher.SubjectsToTeach.Select(x => x.SubjectId.ToString()).ToList();
            //    return View(model);
            //}
            /*************************************************************/

            var idx = User.Identity.GetUserId();
            var teacher = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == idx);
            model.Teacher = teacher;
            model.SubjectsId = model.Teacher.SubjectsToTeach.Select(x => x.SubjectId.ToString()).ToList();

            return View(model);
        }

        //
        // POST: /Profile/Edit/
        [HttpPost]
        public ActionResult Edit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                //save logic
                var teacher = Db.Teachers.FirstOrDefault(x => x.Id == model.Teacher.Id);
                teacher.About = model.Teacher.About;
                teacher.Age = model.Teacher.Age;
                teacher.Category = model.Teacher.Category;
                teacher.City = model.Teacher.City;
                teacher.Education = model.Teacher.Education;
                teacher.HomeNum = model.Teacher.HomeNum;
                teacher.Institution = model.Teacher.Institution;
                teacher.isActive = model.Teacher.isActive;
                teacher.LessonPrice = model.Teacher.LessonPrice;
                teacher.LessonTime = model.Teacher.LessonTime;
                teacher.Phone = model.Teacher.Phone;
                teacher.PictureUrl = model.Teacher.PictureUrl;
                teacher.Street = model.Teacher.Street;

                // remove old subjects to teach
                // bad logic !!!
                Db.SubjectsToTeach.RemoveRange(teacher.SubjectsToTeach);
                // add new subjects to teach
                foreach (var idx in model.SubjectsId)
                {
                    int i = int.Parse(idx);
                    var subj = Db.Subjects.FirstOrDefault(x => x.Id == i);
                    SubjectToTeach stt = new SubjectToTeach();
                    stt.SubjectId = subj.Id;
                    stt.Name = subj.Name;
                    teacher.SubjectsToTeach.Add(stt);
                }
                // update geolocation
                teacher.UpdateGeoLocation(new GeoLocation(model.Teacher.GetAddressForMap()));

                Db.SaveChanges();

                return RedirectToAction("Index");
            }
            // model not valid get data again and return to page
            model.Subjects = GetSubjectsSelectedList();
            model.Cities = GetCitiesSelectedList();
            model.Time = GetLessonTimeSelectedList();
            model.Institutions = GetInstitutionsGroupDropList();

            return View(model);
        }

        //
        // POST: /Profile/UploadImage/
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
                    // upload image to imgur.com
                    imgurl = ImageUtils.UploadImageToImgur(imageByte);
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

        // GET: /Profile/ChangeActive
        public ActionResult ChangeActive()
        {
            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            if (t == null)
                return RedirectToAction("Create");
            t.isActive = !t.isActive;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Profile/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            if (t == null)
                return RedirectToAction("Create");
            Db.Teachers.Remove(t);
            Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Build Subjects selected list
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetSubjectsSelectedList()
        {
            return Db.Subjects.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }

        /// <summary>
        ///  Build Cities selected list
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetCitiesSelectedList()
        {
            return Db.Cities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
        }

        /// <summary>
        /// Build Institutions GroupDropList
        /// </summary>
        /// <returns></returns>
        private List<GroupDropListItem> GetInstitutionsGroupDropList()
        {
            return Db.Institutions.OrderBy(x => x.Id).GroupBy(x => x.Type)
                .Select(g => new GroupDropListItem { Name = g.Key, Items = g.Select(x => new OptionItem { Text = x.Name, Value = x.Name }).ToList() }).ToList();
        }

        /// <summary>
        /// Build LessonTime selected list
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetLessonTimeSelectedList()
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "30 דקות", Value = "30" });
            items.Add(new SelectListItem() { Text = "45 דקות", Value = "45" });
            items.Add(new SelectListItem() { Text = "60 דקות", Value = "60" });
            items.Add(new SelectListItem() { Text = "90 דקות", Value = "90" });
            return items;
        }

    }
}