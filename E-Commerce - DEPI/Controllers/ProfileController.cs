using E_Commerce___DEPI.Models;
using E_Commerce___DEPI.Session;
using GP.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce___DEPI.Controllers
{
    public class ProfileController : Controller
    {
		DbIntities context;
		private readonly ILogger<ProfileController> _logger;

		public ProfileController(DbIntities _context, ILogger<ProfileController> logger)
		{
            context = _context;
			_logger = logger;
		}

		public IActionResult Edit(int id)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			var customer = context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Update(Customer model)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			if (ModelState.IsValid)
            {
                context.Update(model);
                context.SaveChanges();
                return RedirectToAction("ShowProfile", new { id = model.Id });
            }
            return View(model);
        }
        public IActionResult ShowProfile(int id) 
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			var customer= context.Customers.Find(id);
            ViewBag.CustomerId = id;
            ViewData["customer"] = customer;
            return View(customer);
        }
    }
}
