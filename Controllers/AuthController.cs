using BeritaDlanggu.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeritaDlanggu.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToActionPermanent("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string username, [FromForm] string password)
        {
            if (username == "admin" && password == "123")
            {
                var claims = new List<Claim>
        {
                    new Claim("Id", "1"),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, Roles.Admin)
        };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return Redirect("/Admin");
            }

            ViewBag.Error = "Login gagal";
            return View();
        }
    }
}
