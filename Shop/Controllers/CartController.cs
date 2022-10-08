using Shop.Models;
using Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";


        private ApplicationDbContext _context;
        public CartController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Cart
        public ActionResult Index()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];

            ViewBag.CartItems = cart;
            double total = GetTotal(cart);

            ViewBag.Total = total;
            return View();
        }
        public double GetTotal(List<Cart> cartItems)
        {
            double total = 0;
            if (cartItems != null)
            {
                foreach (var item in cartItems)
                    total += item.Product.Price * item.Quantity;
            }
            return total;
        }
        public ActionResult AddToCart(int id,int qnt=1)
        {
            Product productDB = _context.Products.Find(id);

            if (qnt <= 0)
                TempData["false"] = "Product is already in your cart.";

            //ProductList productModel = new ProductList();
            if (Session["cart"] == null)
            {
                List<Cart> cart = new List<Cart>();
                cart.Add(new Cart { 
                    Product = productDB,
                    Quantity = qnt
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Cart> cart = (List<Cart>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    TempData["false"] = "Product is already in your cart.";
                    return Redirect(Request.UrlReferrer.ToString());

                }
                else
                {
                    cart.Add(new Cart {
                        Product = productDB,
                        Quantity = qnt
                    });
                }
                Session["cart"] = cart;
            }
            TempData["msg"] = "Product added successfully.";

            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public JsonResult Update(int id, int qnt)
        {
            List<Cart> cartItems = (List<Cart>)Session["cart"];

            // validate quantity
            if (qnt <= 0)
                return Json(new { Success = false, Message = "Negative count." });

            // if list is empty
            if (cartItems == null || cartItems.Count == 0)
                return Json(new { Success = false, Message = "Cart is empty." });

            // if product not exists
            int index = isExist(id);
            if (index < 0)
                return Json(new { Success = false, Message = "Product not found." });

            // update
            //cartItems.Where(i => i.Product.Id == id).ToList().ForEach(c => c.Quantity = qnt);
            cartItems[index].Quantity = qnt;

            Session["cart"] = cartItems;

            return Json(new { Success = true, Message = "Cart edited." });
        }
        public ActionResult Remove(int id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            if (index >= 0)
            {
                cart.RemoveAt(index);
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }
    }
}