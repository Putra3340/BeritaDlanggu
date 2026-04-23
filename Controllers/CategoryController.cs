using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeritaDlanggu.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BeritaDlangguNetContext _context;
        public CategoryController(BeritaDlangguNetContext context)
        {
            _context = context;
        }
        [HttpGet("/Kategori/{slug?}/{subSlug?}")]
        public IActionResult Index(string slug = null, string subSlug = null)
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


                var scheme = $"{Request.Scheme}://{Request.Host}";
            CategoryViewModel model;
            if (string.IsNullOrEmpty(slug) && string.IsNullOrEmpty(subSlug)) // All Post
            {
                model = new CategoryViewModel
                {
                    Categories = _context.Categories.Include(x => x.Articles).Include(c => c.SubCategories).AsNoTracking().ToList(),
                    Articles = _context.Articles.Include(a => a.Cat).Include(a => a.Author).Where(a => a.Status == (int)ArticleStatus.Published).OrderByDescending(a => a.PublishedAt).ToList()
                };

                ViewData["MetaTitle"] = "Semua Berita - " + (siteNameSetting?.Value ?? "");
                ViewData["MetaDesc"] = "Menampilkan semua berita terbaru dari SMKN 1 Dlanggu";
                ViewData["MetaKeyword"] = siteNameSetting?.Value ?? "" + "," + tagline?.Value ?? ""; ;
                ViewData["MetaAuthor"] = "Putra3340";
                ViewData["MetaRobot"] = "index, follow";
                ViewData["MetaImage"] = scheme + "/icon-dlanggu.png";
                ViewData["MetaUrl"] = scheme;
                ViewData["MetaType"] = "website";
            }
            else if (!string.IsNullOrEmpty(slug) && string.IsNullOrEmpty(subSlug)) // Category
            {
                var category = _context.Categories.Include(c => c.SubCategories).FirstOrDefault(c => c.Slug == slug);
                if (category == null)
                {
                    return NotFound();
                }
                model = new CategoryViewModel
                {
                    CurrentCategory = category,
                    Categories = _context.Categories.Include(x => x.Articles).Include(c => c.SubCategories).AsNoTracking().ToList(),
                    Articles = _context.Articles.Include(a => a.Cat).Include(a => a.Author).Where(a => a.Status == (int)ArticleStatus.Published && a.CatId == category.Id).OrderByDescending(a => a.PublishedAt).ToList()
                };
                ViewData["MetaTitle"] = category.Name + " - " + (siteNameSetting?.Value ?? "");
                ViewData["MetaDesc"] = category.Description ?? "";
                ViewData["MetaKeyword"] = siteNameSetting?.Value ?? "" + "," + tagline?.Value ?? ""; ;
                ViewData["MetaAuthor"] = "Putra3340";
                ViewData["MetaRobot"] = "index, follow";
                ViewData["MetaImage"] = scheme + "/icon-dlanggu.png";
                ViewData["MetaUrl"] = scheme;
                ViewData["MetaType"] = "website";
            }
            else // with SubCategory
            {
                var category = _context.Categories.Include(c => c.SubCategories).FirstOrDefault(c => c.Slug == slug);
                if (category == null)
                {
                    return NotFound();
                }
                var subcategory = _context.SubCategories.FirstOrDefault(sc => sc.Slug == subSlug);
                model = new CategoryViewModel
                {
                    CurrentCategory = category,
                    CurrentSubCategory = subcategory,
                    Categories = _context.Categories.Include(x => x.Articles).Include(c => c.SubCategories).AsNoTracking().ToList(),
                    Articles = _context.Articles.Include(a => a.Cat).Include(a => a.Author).Where(a => a.Status == (int)ArticleStatus.Published && a.SubCat.Slug == subSlug).OrderByDescending(a => a.PublishedAt).ToList()
                };
                ViewData["MetaTitle"] = category.Name + " - " + (siteNameSetting?.Value ?? "");
                ViewData["MetaDesc"] = category.Description ?? "";
                ViewData["MetaKeyword"] = siteNameSetting?.Value ?? "" + "," + tagline?.Value ?? ""; ;
                ViewData["MetaAuthor"] = "Putra3340";
                ViewData["MetaRobot"] = "index, follow";
                ViewData["MetaImage"] = scheme + "/icon-dlanggu.png";
                ViewData["MetaUrl"] = scheme;
                ViewData["MetaType"] = "website";
            }
            
            return View(model);
        }
    }
}
