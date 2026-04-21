namespace BeritaDlanggu.Services
{
    public class NotificationService
    {
        public event Action<string, string> OnNotify;

        public void Show(string message, string type = "info")
        {
            OnNotify?.Invoke(message, type);
        }
    }
}
