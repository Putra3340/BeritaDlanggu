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
        public const string TimerSlider = "timerslider";
        public const string FooterDescription = "footerdescription";
        public const string InstagramUrl = "instagramurl";
        public const string FacebookUrl = "facebookurl";
        public const string TwitterUrl = "twitterurl";
        public const string YoutubeUrl = "youtubeurl";
        public const string TiktokUrl = "tiktokurl";
        public const string FooterAddress = "footeraddress";
        public const string FooterEmail = "footeremail";
        public const string FooterPhone = "footerphone";
    }
}
