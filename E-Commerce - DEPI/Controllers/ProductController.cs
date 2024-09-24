using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

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

        public IActionResult Detail(int id)
        {
            Product? inst = _context.Products.FirstOrDefault(x => x.Id == id);
            if (inst != null)
            {
                //_context.Entry(inst).Reference(x => x.).Load();
            }

            return View(inst);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

        }

        public IActionResult Add(Product inst)
        {
            return View(inst);
        }

        [HttpPost]
        public IActionResult PostAdd(Product inst)
        {
            if (!string.IsNullOrEmpty(inst.Name) && inst.Salary > 0 && !string.IsNullOrEmpty(inst.Address))
            {
                _context.Products.Add(inst);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Add", inst);
        }

        public IActionResult Edit(int id)
        {
            Product? inst = _context.Products.FirstOrDefault(x => x.Id == id);
            if (inst != null)
            {
                
            }

            return View(inst);
        }

        [HttpPost]
        public IActionResult PostEdit(Product inst)
        {
            if (!string.IsNullOrEmpty(inst.Name) && inst.Salary > 0 && !string.IsNullOrEmpty(inst.Address))
            {
                var Product = _context.Products.FirstOrDefault(x => x.Id == inst.Id);
                if (Product != null)
                {
                    Product.Name = inst.Name;
                    Product.Image = inst.Image;
                    Product.Salary = inst.Salary;
                    Product.Address = inst.Address;

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Edit", inst);
        }
    }
}
