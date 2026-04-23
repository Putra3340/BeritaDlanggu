using BeritaDlanggu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeritaDlanggu.Controllers
{
    public class ArticleController : Controller
    {
        private readonly BeritaDlangguNetContext _context;
        public ArticleController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        [HttpGet("/Article/{slug}")]
        public IActionResult Index(string slug)
        {
            // Need this every Page
            var siteNameSetting = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.WebsiteName);
            var tagline = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.TagLine);
            var theme = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ThemeColor);
            var accentColor = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ThemeAccentColor);
            var articlesPerPage = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ArticlePerPage);
            ViewData[ServerSettingsKey.WebsiteName] = siteNameSetting?.Value ?? "";
            ViewData[ServerSettingsKey.TagLine] = tagline?.Value ?? "";
            ViewData[ServerSettingsKey.ThemeColor] = theme?.Value ?? "blue";
            ViewData[ServerSettingsKey.ThemeAccentColor] = accentColor?.Value ?? "blue";
            ViewData[ServerSettingsKey.ArticlePerPage] = articlesPerPage?.Value ?? "9";
            ViewData["CatList"] = _context.Categories.Include(c => c.SubCategories).AsNoTracking().ToList();


            var article = _context.Articles.Include(x=>x.Cat).ThenInclude(x=>x.SubCategories).Include(x=>x.SubCat).Include(x=>x.Author).FirstOrDefault(a => a.Slug == slug);
            if (article == null)
            {
                return NotFound();
            }
            ViewBag.Related = _context.Articles.Include(x=>x.Cat).Include(x=>x.Author).Where(x => x.CatId == article.CatId && x.Id != article.Id).Take(3).ToList();

            // the views counter
            if (HttpContext.Session.GetString($"viewed_{article.Id}") == null)
            {
                article.Views++;
                HttpContext.Session.SetString($"viewed_{article.Id}", "1");
                _context.SaveChanges();
            }
            return View(article);
        }
    }
}
