using System.Text.Json;

namespace BeritaDlanggu.Models.WebSocketModels
{
    public class WsDataModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public JsonElement Data { get; set; }
    }
    public class RequestPostModel
    {
        public string Username { get; set; }
        public int Count { get; set; }
    }
    public class RequestDetailPostModel
    {
        public string PostId { get; set; }
    }

    // LIST
    public class ResponsePostModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
    }
    public class ResponseDetailPostModel
    {
        public string Caption { get; set; }
    }
    public class ResponseLogModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
