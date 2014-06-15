using System;
using System.Linq;
using System.Web.Mvc;
using TeachMe.Helpers;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class TeacherController : Controller
    {
        public TeachMeDBContext Db { get; private set; }

        public TeacherController()
        {
            Db = new TeachMeDBContext();
        }

        // GET: Teacher
        [Route("Teacher/{id?}")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Result", new { Message = ResultMessage.Error });
            }

            TeacherViewModel model = new TeacherViewModel();
            model.Teacher = Db.Teachers.FirstOrDefault(t => t.Id == id.Value);
            model.Comment = new Comment() { TeacherId = model.Teacher.Id };
            //Encrease views
            model.Teacher.Views++;
            Db.SaveChanges();
            return View(model);
        }

        // GET: /Profile/Delete
        [Route("Teacher/SendProfile/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendProfile(int? id, string from, string email, string comment)
        {
            Teacher teacher = Db.Teachers.FirstOrDefault(t => t.Id == id.Value);
            Email.Send(email, "", teacher.GetFullName(), " פרופיל של" + teacher.GetFullName(), EmailTemplate.Feedback);
            return RedirectToAction("Index", new { @id = id });
        }

        [Route("Teacher/CreateComment/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,TeacherId,AuthorName,CommentText")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    comment.Date = DateTime.Now;
                    Teacher teacher = Db.Teachers.Find(comment.TeacherId);
                    teacher.Comments.Add(comment);
                    Db.SaveChanges();
                    string to = teacher.User.UserName;
                    Email.Send(to, comment.AuthorName, "", comment.CommentText, EmailTemplate.Comment);
                }
                catch (Exception)
                {

                }
                return RedirectToAction("Index", new { id = comment.TeacherId });
            }
            //ModelState.AddModelError("", "Some Error.");
            var tvm = new TeacherViewModel();
            tvm.Teacher = Db.Teachers.Find(comment.TeacherId);
            tvm.Comment = comment;
            return View("Index", tvm);


        }

        [Route("Teacher/RateItem/")]
        public ActionResult RateItem(int id, int rate)
        {
            bool success = false;
            string error = "";
            double totalRaters = 0;
            try
            {
                if (Request.Cookies["rating" + id] != null)
                    return Json(new { error = error, success = success, pid = id, total = totalRaters }, JsonRequestBehavior.AllowGet);
                Response.Cookies["rating" + id].Value = DateTime.Now.ToString();
                Response.Cookies["rating" + id].Expires = DateTime.Now.AddYears(1);

                totalRaters = IncrementRating(rate, id);
                success = true;
            }
            catch (Exception ex)
            {
                // get last error
                if (ex.InnerException != null)
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                error = ex.Message;
            }
            if (totalRaters != 0)
                success = true;
            return Json(new { error = error, success = success, pid = id, total = totalRaters }, JsonRequestBehavior.AllowGet);
        }

        private int IncrementRating(int rate, int id)
        {
            var teach = Db.Teachers.Where(a => a.Id == id).First();
            try
            {
                teach.Rating += rate;
                teach.Raters += 1;
                Db.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
            return teach.Raters;
        }
    }
}