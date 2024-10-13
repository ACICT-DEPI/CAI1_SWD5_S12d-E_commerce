using E_Commerce___DEPI.Models;
using E_Commerce___DEPI.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GP.Controllers
{
    public class ProductController : Controller
    {
        DbIntities context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(DbIntities _context, ILogger<ProductController> logger)
        {
			context = _context;
			_logger = logger;
        }

        public IActionResult Index(int pagenum)
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			const int ItemsPerPage = 9;

            if (pagenum < 1)
                pagenum = 1;

            ViewBag.CurrentPage = pagenum;
            ViewBag.TotalPages = (int)Math.Ceiling(context.Products.Count() / (float)ItemsPerPage);

            return View(context.Products
                .Skip((pagenum - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .Include(x => x.Category)
                .ToList()
                );
        }

        public IActionResult NotFound(int id)
        {
            return View(id);
        }

        public IActionResult Detail(int id)
        {
            User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
            if (prd != null)
            {
                context.Entry(prd).Reference(x => x.Category).Load();
				//context.Entry(prd).Reference(x => x.UpholsteryMat).Load();

				ViewBag.IsAdmin = user.IsAdmin;
				return View(prd);
            }
            return RedirectToAction("NotFound", id);
        }

        public IActionResult Add()
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			ViewBag.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
			return View();
        }

        [HttpPost]
        public IActionResult PostAdd(Product prd)
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			if (ModelState.IsValid)
            {
                context.Products.Add(prd);
                context.SaveChanges();  

                return RedirectToAction("Index");
            }

			ViewBag.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
			return View("Add", prd);
        }

        public IActionResult Edit(int id)
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
            if (prd != null)
            {
				ViewBag.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
				return View(prd);
            }
            return RedirectToAction("NotFound", id);
        }

        [HttpPost]
        public IActionResult PostEdit(Product prd)
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			if (ModelState.IsValid)
            {
                var product = context.Products.FirstOrDefault(x => x.Id == prd.Id);
                if (product != null)
                {
                    product.Update(prd);
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("NotFound", prd.Id);
            }
            return View("Edit", prd);
        }

        public IActionResult Delete(int id)
        {
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
			if (prd != null)
			{
				context.Entry(prd).Reference(x => x.Category).Load();
				//context.Entry(prd).Reference(x => x.UpholsteryMat).Load();

				return View(prd);
			}
			return RedirectToAction("NotFound", id);
		}

		[HttpGet]
		public IActionResult PostDelete(int id)
		{
			if (!SessionHelper.IsLoggedIn(this, context, true))
				return View(HomeController.UnauthorizedView);

			Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
			if (prd != null)
			{
				context.Products.Remove(prd);
				context.SaveChanges();

				return RedirectToAction("Index");
			}
			return RedirectToAction("NotFound", id);
	
        }
    }
}
