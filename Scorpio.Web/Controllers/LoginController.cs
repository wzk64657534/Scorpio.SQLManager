using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Scorpio.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            int count = 0;
            if (username == "admin" && password == "123456")
            {
                var user = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,username)
                };
                var claimIndentity = new ClaimsIdentity(user, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIndentity));
                count = 1;
            }
            return Json(count);
        }
    }
}
