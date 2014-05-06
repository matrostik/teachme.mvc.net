using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class AdminController : Controller
    {
        public ApplicationDbContext Db { get; private set; }

        public AdminController()
        {
            Db = new ApplicationDbContext();
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

            if (string.IsNullOrEmpty(filter) || filter.Equals("all"))
                model.Users = Db.Users.ToList();
            else
                model.Users = Db.Users.Where(x => !x.IsConfirmed).ToList();

            if (!string.IsNullOrEmpty(sortOrder))
            {
                model.SortParm = sortOrder;
                switch (sortOrder)
                {
                    case "firstName":
                        model.Users = model.Users.OrderBy(t => t.FirstName).ToList();
                        break;
                    case "lastName":
                        model.Users = model.Users.OrderBy(t => t.LastName).ToList();
                        break;
                    default:
                        break;
                }
            }
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            //ViewBag.Count = list.Count;
            //ViewBag.Result = list.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        //
        // GET: /Admin/UserDetails/5
        public ActionResult UserDetails(string id)
        {
            ApplicationUser u = Db.Users.Include(r => r.Roles).FirstOrDefault(x => x.Id == id);
            AdminUserDetailsViewModel model = new AdminUserDetailsViewModel();
            model.User = u;

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
