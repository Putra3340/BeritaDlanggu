using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeritaDlanggu.Controllers
{
    public class SearchController : Controller
    {
        private readonly BeritaDlangguNetContext _context;
        public SearchController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        [HttpGet("/Search")]
        public IActionResult Index(string query = null, string catId = null)
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
            SearchViewModel model;
            if (query == null || query == "")
            {
                model = new SearchViewModel
                {
                    Articles = _context.Articles.Include(x=>x.Cat).Include(x=>x.Author).Where(x=>x.Status == (int)ArticleStatus.Published).Take(10).ToList(),
                    Categories = _context.Categories.ToList()
                };
            } else if (query != null && catId == null)
            {
                model = new SearchViewModel
                {
                    Query = query,
                    Articles = _context.Articles.Include(x=>x.Cat).Include(x=>x.Author).Where(a => a.Title.Contains(query) && a.Status == (int)ArticleStatus.Published).ToList(),
                    Categories = _context.Categories.ToList()
                };
            }
            else
            {
                model = new SearchViewModel
                {
                    CatId = catId,
                    Query = query,
                    Articles = _context.Articles.Include(x=>x.Cat).Include(x=>x.Author).Where(a => a.Title.Contains(query) && a.CatId.ToString() == catId && a.Status == (int)ArticleStatus.Published).ToList(),
                    Categories = _context.Categories.ToList()
                };
            }
            var scheme = $"{Request.Scheme}://{Request.Host}";

            ViewData["MetaTitle"] = model.Query + " - " + (siteNameSetting?.Value ?? "");
            ViewData["MetaDesc"] = tagline?.Value ?? "";
            ViewData["MetaKeyword"] = siteNameSetting?.Value ?? "" + "," + tagline?.Value ?? ""; ;
            ViewData["MetaAuthor"] = "Putra3340";
            ViewData["MetaRobot"] = "index, follow";
            ViewData["MetaImage"] = scheme + "/icon-dlanggu.png";
            ViewData["MetaUrl"] = scheme;
            ViewData["MetaType"] = "website";
            return View(model);
        }
    }
}
