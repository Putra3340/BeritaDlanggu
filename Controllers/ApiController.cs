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
                    x.Category.FirstOrDefault().Name
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
        [HttpGet("seed")]
        public IActionResult Seed()
        {
            // Seed admin user (password: Admin@123)
            var userlist = new List<Users>
            {
                new Users
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@smkn1dlanggu.sch.id",
                    PasswordHash = Argon2.Hash("Admin@123"),
                    Role = Roles.Admin,
                    FullName = "Administrator",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local)
                },
                new Users
                {
                    Id = 2,
                    Username = "editor",
                    Email = "editor@smkn1dlanggu.sch.id",
                    PasswordHash = Argon2.Hash("Editor@123"),
                    Role = Roles.Editor,
                    FullName = "Budi Santoso",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local)
                }
            };
            

            // Seed categories
            var categorylist = new List<Categories> {
                new Categories { Id = 1, Name = "Berita Sekolah", Slug = "berita-sekolah", Description = "Informasi terkini kegiatan sekolah", SortOrder = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 2, Name = "Kegiatan Siswa", Slug = "kegiatan-siswa", ParentId = 1, SortOrder = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 3, Name = "Kegiatan Guru", Slug = "kegiatan-guru", ParentId = 1, SortOrder = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 4, Name = "Fasilitas", Slug = "fasilitas", ParentId = 1, SortOrder = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 5, Name = "Prestasi", Slug = "prestasi", Description = "Pencapaian siswa dan guru", SortOrder = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 6, Name = "Akademik", Slug = "akademik", ParentId = 5, SortOrder = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 7, Name = "Non-Akademik", Slug = "non-akademik", ParentId = 5, SortOrder = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 8, Name = "Lomba", Slug = "lomba", ParentId = 5, SortOrder = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 9, Name = "Pengumuman", Slug = "pengumuman", Description = "Pengumuman resmi sekolah", SortOrder = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 10, Name = "PPDB", Slug = "ppdb", ParentId = 9, SortOrder = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 11, Name = "Ujian", Slug = "ujian", ParentId = 9, SortOrder = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 12, Name = "Libur", Slug = "libur", ParentId = 9, SortOrder = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 13, Name = "Ekstrakurikuler", Slug = "ekstrakurikuler", Description = "Kegiatan ekskul sekolah", SortOrder = 4, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 14, Name = "Olahraga", Slug = "olahraga", ParentId = 13, SortOrder = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 15, Name = "Seni", Slug = "seni", ParentId = 13, SortOrder = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 16, Name = "Teknologi", Slug = "teknologi", ParentId = 13, SortOrder = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Categories { Id = 17, Name = "Agenda", Slug = "agenda", Description = "Jadwal dan agenda sekolah", SortOrder = 5, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            };

            // Seed settings
            var settings = new List<Settings> {
                new Settings { Id = 1, Key = "site_name", Value = "Berita SMKN 1 Dlanggu", Description = "Nama website", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 2, Key = "site_tagline", Value = "Portal Berita Resmi Sekolah", Description = "Tagline website", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 3, Key = "site_description", Value = "Portal berita resmi SMKN 1 Dlanggu yang menyajikan informasi terkini seputar kegiatan sekolah, prestasi siswa, pengumuman, dan berbagai agenda penting.", Description = "Deskripsi website", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 4, Key = "theme_color", Value = "#7c3aed", Description = "Warna tema utama", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 5, Key = "site_address", Value = "Jl. Raya Dlanggu, Kec. Dlanggu, Kab. Mojokerto, Jawa Timur", Description = "Alamat sekolah", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 6, Key = "site_phone", Value = "(0321) 123456", Description = "Nomor telepon", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 7, Key = "site_email", Value = "info@smkn1dlanggu.sch.id", Description = "Email sekolah", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 8, Key = "articles_per_page", Value = "9", Description = "Jumlah artikel per halaman", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 9, Key = "social_facebook", Value = "#", Description = "URL Facebook", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 10, Key = "social_instagram", Value = "#", Description = "URL Instagram", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Settings { Id = 11, Key = "social_youtube", Value = "#", Description = "URL YouTube", UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            };

            // Seed Article
            var articlelist = new List<Articles>();
            
            
            foreach(var x in categorylist)
            {
                articlelist.Add(new Articles { Title = "Pendaftaran Peserta Didik Baru Tahun Ajaran 2026/2027 Dibuka", Slug = Guid.NewGuid().ToString(), Content = "INTINYA ISI", Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", ThumbnailUrl = "https://images.unsplash.com/photo-1580582932707-520aed937b7b?w=800&h=500&fit=crop", Status = (byte)ArticleStatus.Published, IsFeatured = false, Views = 67, MetaTitle = "Pendaftaran Peserta Didik Baru Tahun Ajaran 2026/2027 Dibuka", AuthorId = 2, CreatedAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(5)), UpdatedAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(5)), PublishedAt = DateTime.Now, Category = categorylist.Take(3).ToList() });
            }

            _context.Users.AddRange(userlist);
            _context.Categories.AddRange(categorylist);
            _context.Settings.AddRange(settings);
            _context.Articles.AddRange(articlelist);
            _context.SaveChanges();
            return Ok();
    }
}
}
