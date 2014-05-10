using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace TeachMe.Controllers
{
    public class AdminController : Controller
    {
        public TeachMeDBContext Db { get; private set; }

        public AdminController()
        {
            Db = new TeachMeDBContext();
        }
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            AdminIndexViewModel model = new AdminIndexViewModel();
            model.Users = Db.Users.ToList();

            return View(model);
        }

        //
        // GET: /Admin/Users/
        public ActionResult Users(string filter, string sortOrder, int? page)
        {
            AdminUsersViewModel model = new AdminUsersViewModel();
            var list = new List<ApplicationUser>();
            model.Filter = filter;
            if (string.IsNullOrEmpty(filter) || filter.Equals("all"))
                list = Db.Users.ToList();
            else
                list = Db.Users.Where(x => !x.IsConfirmed).ToList();

            if (!string.IsNullOrEmpty(sortOrder))
            {
                model.SortParm = sortOrder;
                switch (sortOrder)
                {
                    case "firstName":
                        list = list.OrderBy(t => t.FirstName).ToList();
                        break;
                    case "lastName":
                        list = list.OrderBy(t => t.LastName).ToList();
                        break;
                    case "email":
                        list = list.OrderBy(t => t.UserName).ToList();
                        break;
                    default:
                        break;
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            model.Users = list.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        //
        // GET: /Admin/UserDetails/5
        public ActionResult UserDetails(string id, int? edit)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Users");
            var user = Db.Users.Include(r => r.Roles).FirstOrDefault(x => x.Id == id);
            var teacher = Db.Teachers.FirstOrDefault(x => x.UserId == id);
            AdminUserDetailsViewModel model = new AdminUserDetailsViewModel();
            model.User = user;
            model.Teacher = teacher;
            if (edit != null && edit == 1)
                model.InEditMode = true;
            return View(model);
        }

         //
        // POST: /Admin/UserDetails/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetails(AdminUserDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Db.Users.SingleOrDefault(u => u.Id == model.User.Id);
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.UserName = model.User.UserName;
                user.IsConfirmed = model.User.IsConfirmed;

                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();

                return RedirectToAction("UserDetails", new { id  = user.Id });
            }
            model.InEditMode = true;
            return View(model);
        }


        //
        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
