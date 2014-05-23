using System.Linq;
using System.Web.Mvc;
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
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Result", new { Message = ResultMessage.Error });
            }
            Teacher model = Db.Teachers.FirstOrDefault(t => t.Id == id.Value);
            return View(model);
        }
    }
}