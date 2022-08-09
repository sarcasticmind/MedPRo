using keef2.ViewModels;
using Keefa1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace keef2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> _userManager , 
            SignInManager<IdentityUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        //register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel newUser)
        {
            if(ModelState.IsValid == true)
            {
                IdentityUser userModel = new IdentityUser();
                userModel.Email = newUser.Email;
                userModel.UserName = newUser.Name;
                userModel.PasswordHash = newUser.Password; // not required
               IdentityResult result = await userManager.CreateAsync(userModel, newUser.Password);

                if(result.Succeeded)
                {
                    //save to database
                    //create cookie(authorize id)
                  await  signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Index", "Doctors");

                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }



        //login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
               var user =  await userManager.FindByNameAsync(userLogin.UserName);
                if (user != null)
                {
                   SignInResult result =  await signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.isPresistant, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Doctors");
                    }
                    else ModelState.AddModelError("", "invalid password");
                }
                else
                {
                    ModelState.AddModelError("", "invalid username or password");
                }
            }
            return View(userLogin);
        }


        //logout

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        //Admin Register
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister(RegisterViewModel newUser)
        {
            if (ModelState.IsValid == true)
            {
                IdentityUser userModel = new IdentityUser();
                userModel.Email = newUser.Email;
                userModel.UserName = newUser.Name;
                userModel.PasswordHash = newUser.Password; // not required
                IdentityResult result = await userManager.CreateAsync(userModel, newUser.Password);

                if (result.Succeeded)
                {
                    //add role

                    await userManager.AddToRoleAsync(userModel, "Admin");
                    //create cookie(authorize id)
                    await signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Index", "Doctors");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("Register");
        }







        public IActionResult Index()
        {
            return View();
        }




        //public IActionResult TestUnique(string name)
        //{
        //    IdentityUser crs = context.Users.
        //    if (crs == null)
        //    {
        //        return Json(true);
        //    }
        //    else return Json(false);
        //}
    }
}
