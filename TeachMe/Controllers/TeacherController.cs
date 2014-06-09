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
            Teacher model = Db.Teachers.FirstOrDefault(t => t.Id == id.Value);
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
            return RedirectToAction("Index", new { @id=id });
        }

    }
}