using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce___DEPI.Controllers
{
    public class OrderController : Controller
    {
        DbIntities _context = new DbIntities();
        public IActionResult Index(int id)
        {
            var orders = _context.Orders
                .Where(o => o.CustomerId == id)
                .ToList();
            return View("Index", orders);
        }
        public IActionResult OrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderdItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }



        public IActionResult Create(int customerId)
        {
            var order = new Order { CustomerId = customerId, Date = DateTime.Now };
            return View("Create", order);
        }


        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { customerId = order.CustomerId });
            }

            return View("Create", order);
        }


        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View("Edit", order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { customerId = order.CustomerId });
            }

            return View("Edit", order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public IActionResult ViewOrders(int customerId)
        {
            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderdItems)
                    .ThenInclude(oi => oi.Product)
                .ToList();

            return View("ViewOrders", orders);
        }

    }
}
