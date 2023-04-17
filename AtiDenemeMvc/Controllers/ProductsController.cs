using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private ICategorySerivce _categoryService;
        public ProductsController(IProductService productService, ICategorySerivce categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }   

            var productList = _productService.GetList();
            return View(productList);
        }

        
        [HttpPost]
        public IActionResult ProductGetListByCategory(int id)
        {
            var product = _productService.GetListByCategory(id);

            if (product.Count ==0)
            {
                string errorMessage = "Bu kategoride ürün bulunamamaıştır";
                
                return RedirectToAction("Index", "Products", new { errorMessage });
            }
            else
            {
                return View(product);
            }
            
        }

        
        [HttpPost]
        public IActionResult ProductGetById(int id)
        {
            var product = _productService.GetByID(id);
            if (product == null)
            {
                string errorMessage = "Bu id bir ürün ile eşleşmemiştir";
                // Eğer model null ise, bir önceki sayfaya dönüyoruz.
                return RedirectToAction("Index", "Products", new { errorMessage});
            }
            else
            {
                return View(product);
            }
            

        }



        [Authorize(Roles = "admin")]
        [HttpGet, ActionName("ProductDelete")]
        public IActionResult ProductDelete(int id)
        {
            var product = _productService.GetByID(id);
            _productService.Delete(product);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult ProductAdd()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult ProductAdd(Product product)
        {
            _productService.Add(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
