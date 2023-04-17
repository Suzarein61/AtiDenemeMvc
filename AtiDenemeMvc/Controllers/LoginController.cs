using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<LoginController> _logger;
        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserSignInViewModel userSignInViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userSignInViewModel.UserName, userSignInViewModel.Password, false, true);
                if (result.Succeeded)
                {
                    _logger.LogWarning(userSignInViewModel.UserName + " login oldu");

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "şifre yanlış.");
                    return View(userSignInViewModel);

                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

    }



    //[HttpPost]
    //public async Task<IActionResult> Index(UserSignInViewModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        AppUser user = await _userManager.FindByEmailAsync(model.Email);
    //        if (user != null)
    //        {
    //            //İlgili kullanıcıya dair önceden oluşturulmuş bir Cookie varsa siliyoruz.
    //            await _signInManager.SignOutAsync();
    //            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password,false,true);

    //            if (result.Succeeded)
    //                return Redirect(TempData["returnUrl"].ToString());
    //        }
    //        else
    //        {
    //            ModelState.AddModelError("NotUser", "Böyle bir kullanıcı bulunmamaktadır.");
    //            ModelState.AddModelError("NotUser2", "E-posta veya şifre yanlış.");
    //        }
    //    }
    //    return View(model);
    //}


}
