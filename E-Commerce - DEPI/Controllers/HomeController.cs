using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

namespace GP.Controllers
{
    public class HomeController : Controller
    {
        DbIntities context = new DbIntities();
        private readonly ILogger<HomeController> _logger;
        private const int PageSize = 10; // Number of items per page

        public HomeController(ILogger<HomeController> logger)
        {
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
            ViewBag.TotalPages =5;

            ViewBag.Categories = categories;
            ViewBag.CategoryCurrentPage = categoryPage;
            ViewBag.CategoryTotalPages = (int)Math.Ceiling((double)totalCategories / CategoryPageSize);

            return View(categories);
        }




        //  ISSUE HERE: need to sort first then paginate , and may combine this action with the show products
        public IActionResult ShowProductsSorted(string criteria, int id , int Catid, int pageNo = 0)
        {
            if(criteria == "Rate")
            {
                ViewBag.catid = Catid;
                ViewBag.next = pageNo + 1;
                ViewBag.prev = pageNo - 1;
                var page = context.Products.Where(x => x.CatId == Catid).Skip(pageNo * 6).OrderBy(x => x.Rate).Take(6).ToList();

                return View("ShowProducts", page);
            }
            else if (criteria == "Price")
            {
                ViewBag.catid = Catid;
                ViewBag.next = pageNo + 1;
                ViewBag.prev = pageNo - 1;
                var page = context.Products.Where(x => x.CatId == Catid).Skip(pageNo * 6).OrderBy(x => x.Price).Take(6).ToList();

                return View("ShowProducts", page);
            }
            else if (criteria == "Alpha")
            {
                ViewBag.catid = Catid;
                ViewBag.next = pageNo + 1;
                ViewBag.prev = pageNo - 1;
                var page = context.Products.Where(x => x.CatId == Catid).Skip(pageNo * 6).OrderBy(x => x.Name).Take(6).ToList();

                return View("ShowProducts", page);
            }
            else
            {
                ViewBag.catid = Catid;
                ViewBag.next = pageNo + 1;
                ViewBag.prev = pageNo - 1;
                var page = context.Products.Where(x => x.CatId == Catid).Skip(pageNo * 6).OrderBy(x => x.Rate).Take(6).ToList();

                return View("ShowProducts", page);
            }

        }
        public IActionResult Search(string searchQuery)
        {
            List<Product>products = context.Products.Where(x=>x.Name.Contains(searchQuery) ).ToList();
            if(products .Count > 0)
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

	}
}
