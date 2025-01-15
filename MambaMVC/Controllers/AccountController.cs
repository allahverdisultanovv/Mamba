using MambaMVC.Models;
using MambaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid) return View(userVM);
            AppUser user = new AppUser()
            {
                Name = userVM.Name,
                Email = userVM.Email,
                Surname = userVM.Surname,
                UserName = userVM.UserName,
            };
            IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM, string? url)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Name.Contains(userVM.UsernameOrEmail) || u.Email.Contains(userVM.UsernameOrEmail));
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email or username or passsword is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.IsPersistent, true);


            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account blocked , pls try again later");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or username or passsword is incorrect");
                return View();
            }

            if (url is not null)
            {
                return Redirect(url);
            }

            return RedirectToAction("Index", "Home");


        }


    }
}
