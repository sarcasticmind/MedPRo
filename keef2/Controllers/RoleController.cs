using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace keef2.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> New(string roleName)
        {
            if(roleName != null)
            {
                IdentityRole role = new IdentityRole(roleName);
                IdentityResult result =  await roleManager.CreateAsync(role);

                if(result.Succeeded)
                {
                    return View();
                }
                else
                {
                    TempData["message"] = result.Errors.FirstOrDefault().Description;
                }
            }
            ViewData["roleName"] = roleName;
            return View();
        }
    }
}
