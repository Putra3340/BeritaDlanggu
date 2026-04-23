namespace BeritaDlanggu.Models.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public string CatId { get; set; }
        public List<Articles> Articles { get; set; }
        public List<Categories> Categories { get; set; }
    }
}
