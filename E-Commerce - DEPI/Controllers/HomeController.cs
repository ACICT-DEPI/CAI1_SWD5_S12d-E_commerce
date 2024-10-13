using Castle.Core.Resource;
using E_Commerce___DEPI.Models;
using E_Commerce___DEPI.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace GP.Controllers
{
	public class HomeController : Controller
	{
		public const string LoggedInView = "../Home/LoginRequired",
							LoggedOutView = "../Home/LogoutRequired",
							UnauthorizedView = "../Home/Unauthorized";

		DbIntities context;
		private readonly ILogger<HomeController> _logger;
		private const int PageSize = 10; // Number of items per page

		public HomeController(DbIntities _context, ILogger<HomeController> logger)
		{
			context = _context;
			_logger = logger;
		}

		public IActionResult Privacy()
		{

			return View();
		}

		public ActionResult Index(int page = 1, int categoryPage = 1)
		{
			const int PageSize = 1; // Number of items per page for products
			const int CategoryPageSize = 3; // Number of items per page for categories

			// Fetch Best Selling Products (already implemented)
			var topSellings = context.OrderdItems
				.GroupBy(x => x.ProductId)
				.Select(g => new
				{
					ProductId = g.Key,
					Amount = g.Count()
				})
				.OrderByDescending(x => x.Amount)
				.Join(context.Products,
					o => o.ProductId,
					p => p.Id,
					(o, p) => new
					{
						p.Id,
						p.Name,
						p.Price,
						p.Description,
						p.img1,
						p.Rate,
						AmountSold = o.Amount
					})
				.Skip((page - 1) * PageSize)
				.Take(PageSize)
				.ToList();

			// Count total products for pagination (already implemented)
			var totalProducts = context.OrderdItems
				.GroupBy(x => x.ProductId)
				.Count();

			// Fetch Categories for pagination
			var categories = context.Categories
				.OrderBy(c => c.Name) // Optional: order by category name or any other property
				.Skip((categoryPage - 1) * CategoryPageSize)
				.Take(CategoryPageSize)
				.ToList();

			// Count total categories for pagination
			var totalCategories = context.Categories.Count();

			// Pass data via ViewBag
			ViewBag.BestSellingProducts = topSellings;
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = 5;

			ViewBag.Categories = categories;
			ViewBag.CategoryCurrentPage = categoryPage;
			ViewBag.CategoryTotalPages = (int)Math.Ceiling((double)totalCategories / CategoryPageSize);

			return View(categories);
		}



		public IActionResult Search(string searchQuery, int pageNo = 1, string sortOrder = "asc")
		{
			const int PageSize = 6; // Number of products per page

			// Fetch all products that match the search query
			var productsQuery = context.Products.Where(x => x.Name.Contains(searchQuery));

			// Sorting logic
			ViewBag.CurrentSortOrder = sortOrder;
			productsQuery = sortOrder == "asc" ? productsQuery.OrderBy(p => p.Price) : productsQuery.OrderByDescending(p => p.Price);

			// Get the total count of products for pagination
			var totalProducts = productsQuery.Count();

			// Fetch products for the current page
			var products = productsQuery.Skip((pageNo - 1) * PageSize).Take(PageSize).ToList();

			// Set ViewBag variables for pagination and sorting
			ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
			ViewBag.CurrentPage = pageNo;
			ViewBag.SearchQuery = searchQuery;
			ViewBag.NextSortOrder = (sortOrder == "asc") ? "desc" : "asc";

			if (products.Count > 0)
			{
				return View(products);
			}
			else
			{
				return View("SearchNoRes");
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Blog()
		{
			return View();
		}

		public IActionResult PrivacyPolicy()
		{
			return View();
		}

		public IActionResult TermsAndConditions()
		{
			return View();
		}

		public IActionResult Login()
		{
			if (SessionHelper.IsLoggedIn(this, context))
				return View(LoggedOutView);
			return View();
		}

		[HttpPost]
		public IActionResult PostLogin(Customer info)
		{
			if (SessionHelper.IsLoggedIn(this, context))
				return View(LoggedOutView);

			else if (!string.IsNullOrEmpty(info.Password) && !Regex.IsMatch(info.Password.ToLower(), "^[a-zA-Z0-9]*$"))
				ModelState.AddModelError("Password", "Password must contain only alphanumeric characters.");

			if (ModelState.GetValidationState("Email") == ModelValidationState.Valid &&
				ModelState.GetValidationState("Password") == ModelValidationState.Valid)
			{
				Customer? user = context.Customers
				.Where(x =>
								x.Email.ToLower() == info.Email.ToLower() &&
								x.Password.ToLower() == info.Password
						  )
					.FirstOrDefault();

				if (user != null)
				{
					SessionHelper.SetUser(this, user);
					return RedirectToAction("Index");
				}
				else
					ModelState.AddModelError(string.Empty, "Email address or password was incorrect, please try again.");
			}
			return View("Login");
		}

		public IActionResult Register()
		{
			if (SessionHelper.IsLoggedIn(this, context))
				return View(LoggedOutView);
			return View();
		}

		[HttpPost]
		public IActionResult PostRegister(Customer customer, string pwconfirm)
		{
			if (SessionHelper.IsLoggedIn(this, context))
				return View(LoggedOutView);

			else if (!string.IsNullOrEmpty(customer.Password) && !Regex.IsMatch(customer.Password.ToLower(), "^[a-zA-Z0-9]*$"))
				ModelState.AddModelError("Password", "Password must contain only alphanumeric characters.");

			else if (string.IsNullOrEmpty(pwconfirm) || (!string.IsNullOrEmpty(customer.Password) && customer.Password != pwconfirm))
				ModelState.AddModelError(string.Empty, "Please confirm your password correctly.");

			if (ModelState.IsValid)
			{
				if (context.Customers.FirstOrDefault(x => x.Email.ToLower() == customer.Email.ToLower()) == null)
				{
					context.Customers.Add(customer);
					context.SaveChanges();

					SessionHelper.SetUser(this, customer);
					return RedirectToAction("Index");
				}
				else
					ModelState.AddModelError(string.Empty, "This email address is already in use.");
			}
			return View("Register");
		}

		public IActionResult Logout()
		{
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(LoggedInView);

			SessionHelper.DeleteUser(this);
			return RedirectToAction("Index");
		}
	}
}