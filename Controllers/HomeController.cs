using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BeritaDlanggu.Controllers
{
    public class HomeController : Controller
    {
        private readonly BeritaDlangguNetContext _context;

        public HomeController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var scheme = $"{Request.Scheme}://{Request.Host}";


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

            var featured = _context.Articles
    .Where(a => a.IsFeatured && a.Status == (int)ArticleStatus.Published)
    .OrderByDescending(a => a.PublishedAt)
    .Take(5)
    .Select(a => new ArticleViewModel
    {
        Id = a.Id,
        Title = a.Title,
        Banner = scheme + a.ThumbnailUrl,
        CategoryName = a.Cat.Name,
        Slug = a.Slug,
        Content = a.Excerpt,
        ViewsCount = a.Views,
        AuthorName = a.Author.FullName,
        CreatedAt = a.CreatedAt
    })
    .ToList();
            var latest = _context.Articles
    .Where(a => a.Status == (int)ArticleStatus.Published)
    .OrderByDescending(a => a.CreatedAt)
    .Take(int.Parse(articlesPerPage?.Value ?? "9"))
    .Select(a => new ArticleViewModel
    {
        Id = a.Id,
        Title = a.Title,
        Slug = a.Slug,
        Banner = a.ThumbnailUrl,
        CategoryName = a.Cat.Name,
        Content = a.Excerpt,
        ViewsCount = a.Views,
        AuthorName = a.Author.FullName,
        CreatedAt = a.CreatedAt
    })
    .ToList();
            var trending = _context.Articles
    .Where(a => a.Status == (int)ArticleStatus.Published)
    .OrderByDescending(a => a.Views)
    .Take(5)
    .Select(a => new ArticleViewModel
    {
        Id = a.Id,
        Title = a.Title,
        Slug = a.Slug,
        Content = a.Excerpt,
        ViewsCount = a.Views,
        AuthorName = a.Author.FullName,
        CreatedAt = a.CreatedAt
    })
    .ToList();
            var announcements = _context.Articles
    .Where(a => a.Cat.Slug == "pengumuman" && a.Status == (int)ArticleStatus.Published)
    .OrderByDescending(a => a.CreatedAt)
    .Take(5)
    .Select(a => new ArticleViewModel
    {
        Id = a.Id,
        Title = a.Title,
        Content = a.Excerpt,
        Slug = a.Slug,
        ViewsCount = a.Views,
        AuthorName = a.Author.FullName,
        CreatedAt = a.CreatedAt
    })
    .ToList();

            var model = new HomeViewModel
            {
                FeaturedArticles = featured,
                LatestArticles = latest,
                TrendingArticles = trending,
                AnnouncementArticles = announcements
            };

            return View(model);
        }

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

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
