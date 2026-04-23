namespace BeritaDlanggu.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<ArticleViewModel> FeaturedArticles { get; set; }
        public List<ArticleViewModel> LatestArticles { get; set; }
        public List<ArticleViewModel> TrendingArticles { get; set; }
        public List<ArticleViewModel> AnnouncementArticles { get; set; }
    }

    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string Banner { get; set; } = null!;
        public int ViewsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
