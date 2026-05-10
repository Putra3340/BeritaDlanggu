using BeritaDlanggu.Models;
using BeritaDlanggu.Models.ViewModels;
using BeritaDlanggu.Pages.Admin;
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
            #region NAV THING
            List<NavbarItem> NavbarItems = new();
            var categoryy = _context.NavSettings
    .Include(x => x.Cat)
    .ThenInclude(x => x.SubCategories)
    .Where(x => x.Cat != null)
    .ToList();

            if (categoryy.Count != 0)
            {
                foreach (var x in categoryy)
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
            }
            // no child
            var customnav = _context.NavSettings.Include(x => x.Article).Where(x => x.Article != null && x.Parent == null).ToList();
            if (customnav.Count != 0)
            {
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
            }
            ;
            var customsubnav = _context.NavSettings.Include(x => x.Article).Where(x => x.Article != null && x.Parent != null).ToList();
            if (customsubnav.Count != 0)
            {
                foreach (var x in customsubnav)
                {
                    var parent = NavbarItems.FirstOrDefault(n => n.Id == x.ParentId);
                    parent.SubItems.Add(new NavbarSubItem { Id = x.Id, Title = x.Title, Url = $"/Article/{x.Article.Slug}" });
                }
            }
            ;
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

        public async Task<IActionResult> LoadMoreArticles(
    int page = 1,
    int cat = 0,
    int subcat = 0)
        {
            var articlesPerPage = _context.Settings.FirstOrDefault(x => x.Key == ServerSettingsKey.ArticlePerPage);
            int pageSize = int.Parse(articlesPerPage?.Value ?? "9");

            var query = _context.Articles
                .Where(x => x.Status == (int)ArticleStatus.Published)
                .AsQueryable();

            // category filter
            if (cat > 0)
            {
                query = query.Where(x => x.CatId == cat);
            }

            // sub category filter
            if (subcat > 0)
            {
                query = query.Where(x => x.SubCatId == subcat);
            }

            var articles = await query
                .Include(x=>x.Cat).Include(x=>x.SubCat).Include(x=>x.Author)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PartialView("_NewsGrid", articles);
        }
    }
}
