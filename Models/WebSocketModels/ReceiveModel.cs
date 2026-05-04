namespace BeritaDlanggu.Models.WebSocketModels
{
    public class ReceiveModel
    {
        public List<ReceiveItem> Items { get; set; } = new();
    }
    public class ReceiveItem
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
    }
}
