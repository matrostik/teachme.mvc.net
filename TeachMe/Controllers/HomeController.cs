using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;

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

        [HttpPost]
        public ActionResult Contact([Bind(Include = "From,Subject,Body")] MailModel mail)
        {
            if (ModelState.IsValid)
            {
                MailMessage email = new MailMessage();
                email.To.Add("matrostik@gmail.com,kobieliasi@gmail.com,anttross@gmail.com,ahuvabloy@gmail.com");
                email.From = new MailAddress(mail.From, "TechMe");
                email.Subject = mail.Subject;
                string Body = mail.Body;
                email.Body = Body;
                email.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                try
                {
                    smtp.Send(email);
                    ViewBag.Result = "Success";
                }
                catch (Exception)
                {
                    ViewBag.Result = "Error";
                }
            }

            return View(new MailModel());
        }
    }
}