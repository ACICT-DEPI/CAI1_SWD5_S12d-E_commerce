using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce___DEPI.Controllers
{
    public class ProfileController : Controller
    {
        DbIntities _context = new DbIntities();
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Update(Customer model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(model);
        }

    }
}
