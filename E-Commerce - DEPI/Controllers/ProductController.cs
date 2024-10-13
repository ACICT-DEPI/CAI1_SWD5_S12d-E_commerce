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

        public IActionResult Index(int pagenum, int catid)
        {
            const int ItemsPerPage = 9;

            if (pagenum < 1)
                pagenum = 1;

            IQueryable<Product> products = context.Products;
            if (catid > 0)
                products = products.Where(x => x.CatId == catid);

            ViewBag.CurrentPage = pagenum;
            ViewBag.TotalPages = (int)Math.Ceiling(products.Count() / (float)ItemsPerPage);

            User? user = SessionHelper.GetUser(this, context);
            ViewBag.IsAdmin = user != null && user.IsAdmin;
            ViewBag.CatId = catid;

            return View(products
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
                Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
                if (prd != null)
                {
                    User? user = SessionHelper.GetUser(this, context);

                    if ((user != null) && SessionHelper.IsLoggedIn(this, context))
                    {
                        List<Feedback> _RelatedFeedbacks = context.Feedbacks.Where(f => f.ProductId == id).ToList();
                        ViewBag.Rate_count = 22;
                        ViewBag.RelatedFeedbacks = _RelatedFeedbacks;
                        ViewBag.FeedbackToAdd = new Feedback();

                        ViewBag.Id = user.Id;
                        return View("Detail", prd);
                    }
                    else
                    {
                        return View(HomeController.LoggedInView);
                    }

                }
                return RedirectToAction("NotFound", id);
            }
       

		[HttpPost]
		public IActionResult AddFeedback(Feedback feedback)
		{
			if (ModelState.IsValid)
			{
				context.Feedbacks.Add(feedback);
				context.SaveChanges();
				return RedirectToAction("Detail", new { id = feedback.ProductId });
			}
			if (!ModelState.IsValid)
			{
				// Log or inspect the validation errors     
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				foreach (var error in errors)
				{
					_logger.LogError(error.ErrorMessage);
				}
			}
			return View("Details", feedback.ProductId);
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
