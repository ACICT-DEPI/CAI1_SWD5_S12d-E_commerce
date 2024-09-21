using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

// I NEED TO CHANGE THE WAY I HANDLE DATA TO TAKE IT FROM DATA BASE NOT THOSE IN MEMORY LISTS
namespace GP.Controllers
{
    public class HomeController : Controller
    {
        DbIntities context = new DbIntities();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            //var page = products.Skip(id*6).Take(6).ToList();
            //ViewBag.next = id+1;
            //ViewBag.prev = id - 1;
            return View(categories);
        }
        public IActionResult ShowProducts(int id)
        {
            var products = context.Products.Where(x=>x.CatId == id).ToList();
            return View(products);
        }
        public IActionResult ShowDetails(int id)
        {
            Product product = context.Products.Find( id);
            return View(product);
        }
        public IActionResult Search(string searchQuery)
        {
            List<Product>products = context.Products.Where(x=>x.Name.Contains(searchQuery) ).ToList();
            return View(products);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
     
    }
}
