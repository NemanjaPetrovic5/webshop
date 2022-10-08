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
        // GET: Admin/Order
        public ActionResult Index()
        {
            IEnumerable<Order> orders = _context.Orders.ToList().OrderByDescending(o => o.Created);
            return View(orders);
        }
        public ActionResult Details(int id)
        {
            Order order = _context.Orders.SingleOrDefault(p => p.Id == id);

            return View(order);
        }
        public ActionResult Edit(int id)
        {
            Order order = _context.Orders.Single(p => p.Id == id);
            var orderStatus = _context.OrderStatus.ToList();

            EditOrderViewModel productViewModel = new EditOrderViewModel()
            {
                Order = order,
                OrderStatuss = orderStatus,

            };
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditOrderViewModel orderVM)
        {
            Order order = _context.Orders.SingleOrDefault(p => p.Id == orderVM.Order.Id);
            order.Id = orderVM.Order.Id;
            order.City = orderVM.Order.City;
            order.Address = orderVM.Order.Address;
            order.TotalPrice = orderVM.Order.TotalPrice;
            order.FirstName = orderVM.Order.FirstName;
            order.LastName = orderVM.Order.LastName;
            order.Phone = orderVM.Order.Phone;
            order.PostCode = orderVM.Order.PostCode;
            order.OrderStatusId = orderVM.Order.OrderStatusId;
            order.Modified = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Details", new { orderVM.Order.Id });
        }
        public ActionResult Delete(int id)
        {
            try
            {
                Order order = _context.Orders.Find(id);

                if (order == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}