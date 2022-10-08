using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Shop.Models.ViewModels;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ViewResult Index(string sortProduct, string currentFilter, string search, int? page)
        {
            //var products = _context.Products.ToList();
            //return View(products);
            ViewBag.CurrentSort = sortProduct;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortProduct) ? "name_desc" : "";


            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var products = from s in _context.Products
                           select s;
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Name.Contains(search));
                                       
            }
            switch (sortProduct)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;

                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
           // return View(products.ToList());
        }
        [Route("Home/Products/{categoryId}")]

        public ActionResult Products(int? categoryId,string sortProduct, string currentFilter, string search, int? page)
        {
          
            //var products = _context.Products.ToList();
            //return View(products);
            ViewBag.CurrentSort = sortProduct;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortProduct) ? "name_desc" : "";
            ViewBag.NameAscSortParm = String.IsNullOrEmpty(sortProduct) ? "name_asc" : "";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortProduct) ? "Date" : "";
            ViewBag.DateSortDescParm = String.IsNullOrEmpty(sortProduct) ? "date_desc" : "";
            ViewBag.PriceAscSort = String.IsNullOrEmpty(sortProduct) ? "price_asc" : "";
            ViewBag.PriceDescSort = String.IsNullOrEmpty(sortProduct) ? "price_desc" : "";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }


            var products = from s in _context.Products
                           select s;
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Name.Contains(search));

            }
            switch (sortProduct)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    products = products.OrderBy(s => s.Name);
                    break;
                case "Date":
                    products = products.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.Created);
                    break;
                case "price_asc":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            if (categoryId != null)
            {
                var products2 = _context.Products
                .OrderByDescending(x => x.Id)
                .Where(x => x.CategoryId == categoryId)
                .ToList();
            return View(products2.ToPagedList(pageNumber, pageSize));
            }
            return View(products.ToPagedList(pageNumber, pageSize));
            // return View(products.ToList());
        }
        public PartialViewResult CategoryPartial()
        {
            var tree = _context.Categories.ToList();
            return PartialView("_CategoryMenu", tree);
        }

        public ActionResult Details(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }
        // user profile (auth access only)
        public ActionResult MyProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ProfileViewModel userVM = new ProfileViewModel();


            if (TempData["Validation"] == null)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);

                //fill form
                userVM.FirstName = user.FirstName;
                userVM.LastName = user.LastName;
                userVM.Address = user.Address;
                userVM.PostCode = user.PostCode;
                userVM.City = user.City;
                userVM.PhoneNumber = user.PhoneNumber;

                ViewBag.Orders = user.Orders.OrderByDescending(o => o.Created).ToList();
            }
            else
            {
                ViewData = (ViewDataDictionary)TempData["Validation"];
            }

            return View(userVM);
        }
        public ActionResult UpdateAccount(ProfileViewModel vm)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                TempData["Validation"] = ViewData;
                return RedirectToAction("MyProfile");
            }

            // get current user
            string userId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);

            // update from form
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Address = vm.Address;
            user.PostCode = vm.PostCode;
            user.City = vm.City;
            user.PhoneNumber = vm.PhoneNumber;

            _context.SaveChanges();

            return RedirectToAction("MyProfile");
        }

      
    }
}