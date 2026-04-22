using BeritaDlanggu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Isopoh.Cryptography.Argon2;
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
        [HttpGet("latest")]
        public IActionResult GetArticles(int page = 1, int pageSize = 6)
        {
            var data = _context.Articles
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.ThumbnailUrl,
                    x.Content,
                    x.Author.FullName,
                    x.Views,
                    x.CreatedAt,
                    x.Cat.Name
                }).ToList();

            return Ok(data);
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
