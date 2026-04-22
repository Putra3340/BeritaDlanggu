namespace BeritaDlanggu.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Editor = "Editor";
    }
    public enum ArticleStatus
    {
        Draft,
        Published
    }
    public static class ServerSettingsKey
    {
        public const string WebsiteName = "websitename";
        public const string TagLine = "tagline";
        public const string ThemeColor = "themecolor";
        public const string ThemeAccentColor = "themeaccentcolor";
        public const string WebsocketUrl = "wsurl";
        public const string ArticlePerPage = "articleperpage";
    }
}
