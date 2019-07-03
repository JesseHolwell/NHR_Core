using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NHR_Core.Models;

namespace NHR_Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        public IActionResult Denied()
        {
            return View();
        }

        [Authorize]
        public IActionResult Configure()
        {
            return View();
        }

        public IActionResult Band()
        {
            return View();
        }

        public IActionResult Discography()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl, string username, string password)
        {
            if (username != null && username.ToLower() == "admin" && password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Admin", ClaimValueTypes.String, String.Concat(Request.Host.Value))
                };

                var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });

                return GoToReturnUrl(returnUrl);
            }
            
                ModelState.AddModelError("", "Login failed. Please check Username and/or password");
            //return RedirectToAction(nameof(Denied));
            return View();

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        private IActionResult GoToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
