using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

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

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }
        public IActionResult Authenticate()
        {
            List<Claim> grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@email.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Grandma.say","very nice")
            };
            ClaimsIdentity grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            ClaimsPrincipal usePrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            HttpContext.SignInAsync(usePrincipal);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff()
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var custompolicy = builder.RequireClaim("hello").Build();
            var authresult = await _authorizationService.AuthorizeAsync(User,custompolicy);

            if(authresult.Succeeded)
            {
                return View("Index");
            }
            return View("Index");
        }
    }
}
