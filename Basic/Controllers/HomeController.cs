using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }
        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@email.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Grandma.say","very nice")
            };
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var usePrincipal = new ClaimsPrincipal(new[] { grandmaIdentity});
            HttpContext.SignInAsync(usePrincipal);
            return RedirectToAction("Index");
        }
    }
}
