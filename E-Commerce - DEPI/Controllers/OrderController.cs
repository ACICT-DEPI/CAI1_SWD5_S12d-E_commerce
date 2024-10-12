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

        [HttpPost]
        public IActionResult ChangeOrderStatus(int orderId, int OrderState)
        {
            // Fetch the order based on the ID
            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                // Update the order's status
                order.Status = (OrderState)OrderState;

                // Save the changes to the database
                context.SaveChanges();
            }

            // Redirect back to the list or the same page
            return RedirectToAction("ListOrder");
        }


    }
}
