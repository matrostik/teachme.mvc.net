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
            return View(model);
        }

        public ActionResult AutocompleteStreet(string term)
        {
            var filteredItems = Db.Streets.Where(item => item.Name.StartsWith(term)).Distinct().ToList();
            List<string> res = filteredItems.Select(x => x.Name).Take(20).ToList();
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