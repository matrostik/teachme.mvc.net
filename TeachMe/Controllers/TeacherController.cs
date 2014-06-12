using System;
using System.Linq;
using System.Web.Mvc;
using TeachMe.Helpers;
using TeachMe.Models;
using System.Collections.Generic;


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
            return View(model);
        }

        // GET: /Profile/Delete
        [Route("Teacher/SendProfile/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendProfile(int? id, string from, string email, string comment)
        {
            Teacher teacher = Db.Teachers.FirstOrDefault(t => t.Id == id.Value);
            // Send reset password email
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

    }
}