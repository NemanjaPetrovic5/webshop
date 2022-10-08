using Shop.Models;
using Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);

        }
        public ActionResult CreateNewCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewCategory(CreateCategoryViewModel p)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("CreateNewProduct");
            }

            // Add category to db
            Category newCategory = new Category()
            {
                Name = p.Name,
                Created = DateTime.Now
            };

            _context.Categories.Add(newCategory); // adding category
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        public ActionResult Edit(int id)
        {
            Category category = _context.Categories.Single(p => p.Id == id);
           
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            // update category

            var productDb = _context.Categories.Single(p => p.Id == category.Id);
            productDb.Name = category.Name;
            productDb.Modified = DateTime.Now;

            // update category

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try { 
                Category productDetail = _context.Categories.Find(id);

                _context.Categories.Remove(productDetail);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}