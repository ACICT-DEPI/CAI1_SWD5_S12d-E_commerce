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

        public IActionResult ChangeOrderStatus(int orderId, int orderState)
        {
            // Fetch the order based on the ID
            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                // Cast the integer to the OrderState enum
                var newOrderState = (OrderState)orderState;

                // Update the order's status
                order.Status = newOrderState;

                // Check if the status is Delivered
                if (newOrderState == E_Commerce___DEPI.Models.OrderState.Delivered)
                {
                    // Create a new OrdersArchive instance
                    var OrderArchives = new OrderArchive
                    {
                        Order = order
                    };

                    // Add the OrdersArchive entry to the database
                    context.OrderArchives.Add(OrderArchives);
                    ViewData["isOrderArchived"] = true;
                }

                // Check if the status is Canceled
                if (newOrderState == E_Commerce___DEPI.Models.OrderState.Canceled)
                {
                    // Remove the order from the database
                    context.Orders.Remove(order);
                    ViewData["isOrderDeleted"] = true;
                }

                // Save the changes to the database
                context.SaveChanges();
            }

            List<Order> orders = context.Orders.ToList();
            ViewData["orders"] = orders;

            // Redirect back to the list or the same page
            return RedirectToAction("ListOrder");
        }

    }
}
