using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce___DEPI.Controllers
{
    public class OrderController : Controller
    {
        DbIntities context = new DbIntities();
        public IActionResult ListOrder()
        {
            List<Order> orders = context.Orders.ToList();
            ViewData["orders"] = orders;
            return View();
        }

        public IActionResult OrderDetails(int orderId)
        {
            Order order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            ViewData["order"] = order;
            return View();
        }

    }
}
