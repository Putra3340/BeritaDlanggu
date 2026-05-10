namespace BeritaDlanggu.Models.ViewModels
{
    public class NavbarItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Type { get; set; } = "category";
        public bool Visible { get; set; } = true;
        public List<NavbarSubItem> SubItems { get; set; } = new();
    }

    public class NavbarSubItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
