using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class CategoriesController : Controller
    {
        private ICategorySerivce _categoryService;
        public CategoriesController(ICategorySerivce categoryService)
        {

            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult CategoryList()
        {
            var productList = _categoryService.GetList();
            return View(productList);
        }
    }
}
