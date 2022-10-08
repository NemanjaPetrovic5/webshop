using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        public UserController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Admin/User
        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> users = _context.Users.ToList();
            return View(users);
        }
        public ActionResult Edit(string id, string email)
        {
            ApplicationUser user = null;

            if (id != null)
                user = _context.Users.FirstOrDefault(u => u.Id == id);
            else if (email != null)
                user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser appUser)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == appUser.Id);

            if (user == null)
            {
                //TempData["message"] = "User not found.";
                return RedirectToAction("Index");
            }

            user.FirstName = appUser.FirstName;
            user.LastName = appUser.LastName;
            user.Address = appUser.Address;
            user.PostCode = appUser.PostCode;
            user.City = appUser.City;
            user.Email = appUser.Email;
            user.PhoneNumber = appUser.PhoneNumber;

            _context.SaveChanges();

            //TempData["message"] = "User edited.";
            return RedirectToAction("Index");

        }
        public ActionResult Delete(string id)
        {
            try
            {

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    //TempData["message"] = "User not found";
                    return RedirectToAction("Index");
                }
                _context.Users.Remove(user);
                _context.SaveChanges();

                //TempData["message"] = "User deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}