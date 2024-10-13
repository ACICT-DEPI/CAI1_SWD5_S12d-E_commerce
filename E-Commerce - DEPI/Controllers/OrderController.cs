using E_Commerce___DEPI.Models;
using E_Commerce___DEPI.Session;
using GP.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace E_Commerce___DEPI.Controllers
{
    public class OrderController : Controller
    {

        DbIntities context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(DbIntities _context, ILogger<OrderController> logger)
        {
            context = _context;
            _logger = logger;
        }

        public IActionResult ListOrder(string sortOrder, string searchTerm)
        {
            if (!SessionHelper.IsLoggedIn(this, context))
                return View(HomeController.LoggedInView);

            List<Order> orders = context.Orders.ToList();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                orders = orders.Where(oa =>
                    (oa.Customer.Fname + " " + oa.Customer.Lname).Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Sorting logic
            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "date_desc":
                        orders = orders.OrderByDescending(o => o.Date).ToList();
                        break;
                    default:
                        orders = orders.OrderBy(o => o.Date).ToList();
                        break;
                }
            }

            ViewData["orders"] = orders;
            return View();

        }

        public IActionResult ListArchivedOrders(string sortOrder, string searchTerm)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

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

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                remainingArchivedOrders = remainingArchivedOrders.Where(oa =>
                    (oa.Order.Customer.Fname + " " + oa.Order.Customer.Lname).Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sorting logic
            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "date_desc":
                        remainingArchivedOrders = remainingArchivedOrders.OrderByDescending(o => o.ArchiveDate).ToList();
                        break;
                    default:
                        remainingArchivedOrders = remainingArchivedOrders.OrderBy(o => o.ArchiveDate).ToList();
                        break;
                }
            }
            ViewData["archivedOrders"] = remainingArchivedOrders;

            // Pass the remaining orders to the view
            return View(remainingArchivedOrders);
        }

        public IActionResult OrderDetails(int orderId)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			Order order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            ViewData["order"] = order;
            return View();
        }

        [HttpPost]

        public IActionResult ChangeOrderStatus(int orderId, int orderState, string isList)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			bool isListBool = isList == "true";
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
            if (isListBool == true)
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
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

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
        public IActionResult CustomerOrders(int id)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			IEnumerable<Order> orders = context.Orders.Where(o => o.CustomerId == id);
            //var orderArchives= context.OrderArchives.Where(o=>o.Order.CustomerId == id);
            //ViewBag.orders = orders;
            //ViewBag.orderArchives = orderArchives;

            return View(orders);
        }


    }
}