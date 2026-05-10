using BeritaDlanggu.Helpers;
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
            #region NAV THING
            List<NavbarItem> NavbarItems = new();
            var category = _context.NavSettings
    .Include(x => x.Cat)
    .ThenInclude(x => x.SubCategories)
    .Where(x => x.Cat != null)
    .ToList();

            if (category.Count != 0)
            {
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
