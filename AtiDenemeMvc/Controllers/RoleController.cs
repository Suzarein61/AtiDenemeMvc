using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RoleController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly UserManager<AppUser> _userManager;
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RoleAssign(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            List<AppRole> allRoles = _roleManager.Roles.ToList();
            List<string> userRoles = await _userManager.GetRolesAsync(user) as List<string>;
            List<RoleAssignViewModel> assignRoles = new List<RoleAssignViewModel>();
            allRoles.ForEach(role => assignRoles.Add(new RoleAssignViewModel
            {
                HasAssign = userRoles.Contains(role.Name),
                RoleId = role.Id,
                RoleName = role.Name
            }));

            return View(assignRoles);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> RoleAssign(List<RoleAssignViewModel> modelList, string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            foreach (RoleAssignViewModel role in modelList)
            {
                if (role.HasAssign)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }
            return RedirectToAction("UserList", "Login");
        }


        public IActionResult CreateRole()
        {
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            //IdentityResult result = null;
            //if (id != null)
            //{
            //    AppRole role = await _roleManager.FindByIdAsync(id);
            //    role.Name = model.RoleName;
            //    result = await _roleManager.UpdateAsync(role);
            //}
            //else
            var result = await _roleManager.CreateAsync(new AppRole { Name = model.RoleName });

            if (result.Succeeded)
            {
                //Başarılı...
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                //Başarılı...
            }
            return RedirectToAction("Index");
        }
    }
}
