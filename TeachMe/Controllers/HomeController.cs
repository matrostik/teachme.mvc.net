using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeachMe.Helpers;
using TeachMe.Models;

namespace TeachMe.Controllers
{

    public class HomeController : Controller
    {
        public TeachMeDBContext Db { get; private set; }

        public HomeController()
        {
            Db = new TeachMeDBContext();
        }

        public ActionResult Index()
        {
            var model = new HomeViewModel();
            model.Cities = Db.Cities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
            model.Subjects = Db.Subjects.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
            var distances = new List<SelectListItem>();
            for (double i = 2.5; i < 50; i += 2.5)
            {
                distances.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            model.Distances = distances;

            //model.NewTeachers = Db.Teachers.OrderByDescending(x => x.User.RegistrationDate).Take(5).ToList();
            //model.MostRatedTeachers = Db.Teachers.OrderByDescending(x => x.Rating).Take(5).ToList();
            //model.MostCommentedTeachers = Db.Teachers.OrderByDescending(x => x.Comments.Count).Take(5).ToList();
            return View(model);
        }

        public ActionResult GetNewTeachers()
        {
            var teachers = Db.Teachers.OrderByDescending(x => x.User.RegistrationDate).Take(5).ToList()
                .Select(x => new TeacherSimple
                {
                    Id = x.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    City = x.City,
                    LessonPrice = x.LessonPrice,
                    Subjects = string.Join(", ", x.SubjectsToTeach.Select(s => s.Name)),
                    PictureUrl = x.PictureUrl
                }).ToList();
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMostRatedTeachers()
        {
            var teachers = Db.Teachers.OrderByDescending(x => x.Rating).Take(5).ToList()
                .Select(x => new TeacherSimple
                {
                    Id = x.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    City = x.City,
                    LessonPrice = x.LessonPrice,
                    Subjects = string.Join(", ", x.SubjectsToTeach.Select(s => s.Name)),
                    PictureUrl = x.PictureUrl,
                    Rating = x.Raters==0 ? 0 : Convert.ToDouble(x.Rating)/Convert.ToDouble(x.Raters)

                }).ToList();
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMostViewedTeachers()
        {
            var teachers = Db.Teachers.OrderByDescending(x => x.Views).Take(5).ToList()
                .Select(x => new TeacherSimple
                {
                    Id = x.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    City = x.City,
                    LessonPrice = x.LessonPrice,
                    Views = x.Views,
                    Subjects = string.Join(", ", x.SubjectsToTeach.Select(s => s.Name)),
                    PictureUrl = x.PictureUrl
                }).ToList();
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompleteStreet(string term)
        {
            var filteredItems = Db.Streets.Where(item => item.Name.StartsWith(term)).Distinct().OrderBy(x => x.Name).ToList();
            List<string> res = filteredItems.Select(x => x.Name).Take(20).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompleteFirstName(string term)
        {
            var filteredItems = Db.Users.Where(item => item.FirstName.StartsWith(term)).Distinct().OrderBy(x => x.FirstName).ToList();
            List<string> res = filteredItems.Select(x => x.FirstName).Take(20).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompleteLastName(string term)
        {
            var filteredItems = Db.Users.Where(item => item.LastName.StartsWith(term)).Distinct().OrderBy(x => x.LastName).ToList();
            List<string> res = filteredItems.Select(x => x.LastName).Take(20).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact([Bind(Include = "From,Subject,Body")] MailModel mail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string to = "matrostik@gmail.com,kobieliasi@gmail.com,anttross@gmail.com,ahuvabloy@gmail.com";
                    Email.Send(to, mail.From, "Feedback", mail.Body, EmailTemplate.Feedback);
                    ViewBag.Result = "Success";
                    return RedirectToAction("Index", "Result", new { Message = ResultMessage.FeedbackSend });
                }
                catch (Exception)
                {
                    ViewBag.Result = "Error";
                }
            }
            return View(mail);
        }
    }
}