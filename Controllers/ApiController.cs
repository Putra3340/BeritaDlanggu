using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using BeritaDlanggu.Pages.Admin;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeritaDlanggu.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly BeritaDlangguNetContext _context;

        public ApiController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        [HttpGet("wipe")]
        public IActionResult Wipe()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.Categories.RemoveRange(_context.Categories);
            _context.Settings.RemoveRange(_context.Settings);
            _context.Articles.RemoveRange(_context.Articles);
            _context.SaveChanges();
            return Ok();
        }
       
        [HttpGet("/test")]
        public async Task<IActionResult> Test()
        {
            var hash = Argon2.Hash("123");
            var valid = Argon2.Verify("123", hash);

            Console.WriteLine(hash);
            Console.WriteLine(valid);
            return Ok(valid);
        }
        
    }
}
