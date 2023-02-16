using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using GymRealmUsers.Data;
using GymRealmUsers.Models;

namespace GymRealmUsers.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (!string.IsNullOrEmpty(model.Email) && user == null)
            {
                ModelState.AddModelError("", "Email does not exist. Go to register");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Incorrect password");
                    return View(model);
                }
                else 
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Users");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if(user != null)
            {
                ModelState.AddModelError("", "Email exists. Go to login");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Telephone = model.Telephone,
                    City = model.City
                });
                await db.SaveChangesAsync();

                await Authenticate(model.Email);

                return RedirectToAction("Index", "Users");
            }
            return View(model);
        }

        private async Task Authenticate(string userEmail)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail)
            };
            ClaimsIdentity claimId = new ClaimsIdentity(claims, "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
