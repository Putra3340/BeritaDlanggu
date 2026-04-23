namespace BeritaDlanggu.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Categories CurrentCategory { get; set; }
        public SubCategories CurrentSubCategory { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Articles> Articles { get; set; }
    }
}
