using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

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
            List<Product> products= context.Products.ToList();
            ViewBag.products = products;
            return View(categories);
        }
        public IActionResult ShowProducts(int Catid,int pageNo=0)
        {
            ViewBag.catid = Catid;
            ViewBag.next = pageNo + 1;
            ViewBag.prev = pageNo - 1;
            if(pageNo<=0)
            {
                var page = context.Products.Where(x => x.CatId == Catid).Skip(0 * 6).Take(6).ToList();
                ViewBag.next = pageNo + 2;

                return View(page);

            }
            else if(pageNo>=context.Products.Count()/6)
            {
                int no = (context.Products.Count() / 6) - 1;
                var page = context.Products.Where(x => x.CatId == Catid).Skip(no * 6).Take(6).ToList();
                ViewBag.prev = pageNo - 2;

                return View(page);

            }
            else
            {
                var page = context.Products.Where(x => x.CatId == Catid).Skip(pageNo * 6).Take(6).ToList();
                return View(page);

            }

        }
        public IActionResult ShowDetails(int id)
        {
            Product product = context.Products.Find( id);
            return View(product);
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
     
    }
}
