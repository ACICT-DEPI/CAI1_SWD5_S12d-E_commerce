using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GP.Controllers
{
    public class ProductController : Controller
    {
        DbIntities context = new DbIntities();
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(context.Products.ToList());
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
                context.Entry(prd).Reference(x => x.Category).Load();
                context.Entry(prd).Reference(x => x.UpholsteryMat).Load();

                return View(prd);
            }
            return RedirectToAction("NotFound", id);
        }

        public IActionResult Add(Product prd)
        {
            return View(prd);
        }

        [HttpPost]
        public IActionResult PostAdd(Product prd)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(prd);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Add", prd);
        }

        public IActionResult Edit(int id)
        {
            Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
            if (prd != null)
                return View(prd);
            return RedirectToAction("NotFound", id);
        }

        [HttpPost]
        public IActionResult PostEdit(Product prd)
        {
            if (ModelState.IsValid)
            {
                var product = context.Products.FirstOrDefault(x => x.Id == prd.Id);
                if (product != null)
                {
                    product.Update(prd);
                    context.SaveChanges();

                    return RedirectToAction("Details", prd.Id);
                }
            }
            return RedirectToAction("NotFound", prd.Id);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product? prd = context.Products.FirstOrDefault(x => x.Id == id);
            if (prd != null)
            {
                context.Products.Remove(prd);
                context.SaveChanges();

                return View(id);
            }
            return RedirectToAction("NotFound", id);
        }
    }
}
