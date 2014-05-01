using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TeachMe.Helpers;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

            var numbers = new List<string>();
            var list = new List<SelectListItem>();
            for (double i = 2.5; i < 50; i += 2.5)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            ViewBag.Numbers = list;

            return View();
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
                    Email.Send(to, mail.From, "Feedback",mail.Body, EmailTemplate.Feedback);
                    ViewBag.Result = "Success";
                    return RedirectToAction("Index", "Result", new { Message = ResultMessageId.FeedbackSend });
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