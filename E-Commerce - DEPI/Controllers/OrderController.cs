using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult ListArchivedOrders()
        {
            // Get today's date
            DateTime currentDate = DateTime.Now;

            // Find orders archived more than 14 days ago
            var ordersToDelete = context.OrderArchives
                                        .Where(oa => oa.ArchiveDate < currentDate.AddDays(-14))
                                        .ToList();

            if (ordersToDelete.Any())
            {
                // First, delete the archived orders from the OrdersArchive table
                context.OrderArchives.RemoveRange(ordersToDelete);
                context.SaveChanges(); // Save changes after removing the archived orders

                // Get the IDs of the related orders to delete from the Orders table
                var relatedOrderIdsToDelete = ordersToDelete.Select(oa => oa.OrderId).ToList();

                // Find the related orders along with their order items
                var relatedOrdersToDelete = context.Orders
                                                    .Include(o => o.OrderdItems) // Load order items
                                                    .Where(o => relatedOrderIdsToDelete.Contains(o.Id))
                                                    .ToList();

                // Delete the order items related to the orders
                foreach (var order in relatedOrdersToDelete)
                {
                    if (order.OrderdItems != null && order.OrderdItems.Any())
                    {
                        context.OrderdItems.RemoveRange(order.OrderdItems); // Remove all items related to the order
                    }
                }

                // Now delete the related orders
                context.Orders.RemoveRange(relatedOrdersToDelete);
                context.SaveChanges(); // Persist the changes to delete the original orders and their items
            }

            // Get remaining orders that are not older than 14 days
            List<OrderArchive> remainingArchivedOrders = context.OrderArchives
                                                                 .Where(oa => oa.ArchiveDate >= currentDate.AddDays(-14))
                                                                 .ToList();

            ViewData["archivedOrders"] = remainingArchivedOrders;

            // Pass the remaining orders to the view
            return View(remainingArchivedOrders);
        }

        public IActionResult OrderDetails(int orderId)
        {
            Order order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            ViewData["order"] = order;
            return View();
        }

        [HttpPost]

        public IActionResult ChangeOrderStatus(int orderId, int orderState, bool isList)
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
                        Order = order,
                        ArchiveDate = DateTime.Today
                    };

                    // Add the OrdersArchive entry to the database
                    context.OrderArchives.Add(OrderArchives);
                    ViewData["isOrderArchived"] = true;
                }

                // Check if the status is Canceled
                if (newOrderState == E_Commerce___DEPI.Models.OrderState.Canceled)
                {
                    // remove the related order items
                    if (order.OrderdItems != null && order.OrderdItems.Any())
                    {
                        // Delete the related order items
                        context.OrderdItems.RemoveRange(order.OrderdItems); // Remove all items related to the order
                    }
                    // Remove the order from the database
                    context.Orders.Remove(order);
                    ViewData["isOrderDeleted"] = true;
                }

                // Save the changes to the database
                context.SaveChanges();
            }
            if (isList)
            {
                return RedirectToAction("ListOrder");
            }
            else
            {
                return RedirectToAction("OrderDetails", new{ orderId= orderId });
            }
            
        }

        public IActionResult DeleteArchivedOrder(int orderId)
        {
            var arrchivedOrder = context.OrderArchives.FirstOrDefault(o => o.Id == orderId);
            if (arrchivedOrder != null)
            {
                // Get the related order from the orders database
                var relatedOrder = context.Orders.FirstOrDefault(o => o.Id == arrchivedOrder.OrderId);
                if (relatedOrder != null) {
                    // Get the related order items from the items database
                    if (relatedOrder.OrderdItems != null && relatedOrder.OrderdItems.Any())
                    {
                        // Delete the related order items
                        context.OrderdItems.RemoveRange(relatedOrder.OrderdItems); // Remove all items related to the order
                    }
                    context.Orders.Remove(relatedOrder);
                }
                // Remove the order from the database
                context.OrderArchives.Remove(arrchivedOrder);
                context.SaveChanges();

            }
                return RedirectToAction("ListArchivedOrders");
        }

    }
}