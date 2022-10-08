using Microsoft.AspNet.Identity;
using Shop.Models;
using Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private ApplicationDbContext _context;
        public ProductController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //Get
        public ViewResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        //Get/id
        public ActionResult Details(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }
        //Get/Add
        public ActionResult Add()
        {
            var categories = _context.Categories.ToList();
            var viewModel = new CreateProductViewModel
            {
                Categories = categories,
            };

            if (TempData["Validation"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["Validation"];
            }

            return View(viewModel);
        }
        //Post/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CreateProductViewModel p)
        {
            // upload image

            string fileName = DateTime.Now.ToString("yyyyMMdd_") + Guid.NewGuid().ToString().Substring(0, 7) + ".png";
            string path = Path.Combine(Server.MapPath("~/Upload/images"), fileName); // thumbnail

            p.Thumbnail.SaveAs(path);

            var userId = Guid.Parse(User.Identity.GetUserId());

            // Add product to db
            Product newProduct = new Product()
            {
                Name = p.Name,
                UserId = userId,
                CategoryId = p.Product.CategoryId,
                Price = p.Price,
                Thumbnail = fileName,
                Description = p.Description,
                Created = DateTime.Now
            };

            _context.Products.Add(newProduct); // adding product
            _context.SaveChanges();
            TempData["msg"] = "Product added successfuly.";

            return RedirectToAction("Index", "Product");
        }
        //Get/Edit
        public ActionResult Edit(int id)
        {
            Product product = _context.Products.Single(p => p.Id == id);
            var categories = _context.Categories.ToList();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = product,
                Categories = categories,

            };
            return View(productViewModel);

        }
        //Post/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            var productDb = _context.Products.SingleOrDefault(p => p.Id == product.Id);
            productDb.Name = product.Name;
            productDb.Price = product.Price;
            productDb.Description = product.Description;
            productDb.CategoryId = product.CategoryId;
            productDb.UserId = userId;
            productDb.Modified = DateTime.Now;

            if (product.Thumbnail != null)
            {

                string fileName = DateTime.Now.ToString("yyyyMMdd_") + Guid.NewGuid().ToString().Substring(0, 7) + ".png";
                string path = Path.Combine(Server.MapPath("~/Upload/images"), fileName); // thumbnail
                productDb.Thumbnail = fileName.ToString();

                product.Thumbnail.SaveAs(path);
            }
            // update product
            _context.SaveChanges();
            TempData["msg"] = "Product updated successfully";

            return RedirectToAction("Index");
        }
        // Product/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = _context.Products.Find(id);
                _context.Products.Remove(product);
                _context.SaveChanges();

                // delete thumbnail
                string thumbnail = product.Thumbnail;
                string path = Path.Combine(Server.MapPath("~/Upload/images"), thumbnail);
                System.IO.File.Delete(path);

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["msg"] = "Product deleted successfully.";

                return RedirectToAction("Index");
            }
        }
    }
}
