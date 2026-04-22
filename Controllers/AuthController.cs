using BeritaDlanggu.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeritaDlanggu.Controllers
{
    public class AuthController : Controller
    {
        private readonly BeritaDlangguNetContext _context;
        public AuthController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
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
            var user = _context.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                ViewBag.Error = "Login gagal";
                return View();
            }
            var hasher = new PasswordHasher<object>();
            // verify
            if(hasher.VerifyHashedPassword(null, user.PasswordHash, password) == PasswordVerificationResult.Success);

            var claims = new List<Claim>
    {
        new Claim("Id", user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);
            // Activity Log
            var activity = new ActivityLogs
            {
                Action = "login",
                Details = user.Username + " logged in",
                IpAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserId = user.Id,
                Timestamp = DateTime.Now
            };
            _context.ActivityLogs.Add(activity);
            _context.SaveChanges();
            return Redirect("/Admin");
        }
    }
}
