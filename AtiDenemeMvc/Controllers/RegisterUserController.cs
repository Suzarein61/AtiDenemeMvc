using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RegisterUserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        public RegisterUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpViewModel userSignUpViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    NameSurname = userSignUpViewModel.NameSurname,
                    UserName = userSignUpViewModel.UserName,
                    Email = userSignUpViewModel.Mail
                };
                
                var result = await _userManager.CreateAsync(appUser, userSignUpViewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
        }


    }
}

