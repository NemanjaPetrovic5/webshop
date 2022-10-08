using Microsoft.AspNet.Identity;
using Shop.Models;
using Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;
        public OrderController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Order
        public ActionResult Index()
        {
            CreateOrderViewModel orderVM = new CreateOrderViewModel();
            //ModelState.AddModelError("ShippingPhone", "neki error");

            List<Cart> cartItems = (List<Cart>)Session["cart"];
            if (cartItems == null || cartItems.Count == 0)
                return RedirectToAction("Index", "Home");

            if (TempData["Validation"] == null)
            {
                // if user logged in
                if (User.Identity.IsAuthenticated)
                {
                    var x = User.Identity.GetUserId();
                    ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == x);
                    if (user != null)
                    {
                        orderVM.City = user.City;
                        orderVM.Address = user.Address;
                        orderVM.FirstName = user.FirstName;
                        orderVM.LastName = user.LastName;
                        orderVM.Phone = user.PhoneNumber;
                        orderVM.PostCode = user.PostCode;
                    }
                }
            }
            else
            {
                ViewData = (ViewDataDictionary)TempData["Validation"];
            }

            return View(orderVM);
        }
        //Post Order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(CreateOrderViewModel orderVM)
        {
           
                List<Cart> cartItems = (List<Cart>)Session["cart"];
                if (cartItems == null || cartItems.Count == 0)
                    //throw new ValidationException("Cart is empty");
                    RedirectToAction("Index");
            if (!ModelState.IsValid)
                {
                    TempData["Validation"] = ViewData;
                    return RedirectToAction("Index");
                }

                // create order
                Order newOrder = new Order
                {
                    OrderStatusId = 1,
                    FirstName = orderVM.FirstName,
                    LastName = orderVM.LastName,
                    City = orderVM.City,
                    Address = orderVM.Address,
                    PostCode = orderVM.PostCode,
                    Phone = orderVM.Phone,
                    TotalPrice = cartItems.Count,
                    Created = DateTime.Now
                };

            if (User.Identity.IsAuthenticated)
            {
                newOrder.UserId = User.Identity.GetUserId();

                _context.Orders.Add(newOrder);
                _context.SaveChanges();


                // add products to order
                List<ProductOrder> orderProducts = new List<ProductOrder>();
                foreach (Cart item in cartItems)
                {
                    orderProducts.Add(new ProductOrder
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.Product.Id,
                        Count = item.Quantity,
                        Price = item.Product.Price,
                        Subtotal = item.Product.Price * item.Quantity,
                        Name = item.Product.Name
                    });
                }

                _context.ProductOrders.AddRange(orderProducts);
                _context.SaveChanges();

                // empty cart
                Session["cart"] = null;

                return RedirectToAction("OrderCreated", new { id = newOrder.Id });
            }
            return View("Index");
        }
        public ActionResult OrderCreated(int id)
        {
                Order order = _context.Orders.Single(u => u.Id == id);

                return View(order);
        }
    }
}