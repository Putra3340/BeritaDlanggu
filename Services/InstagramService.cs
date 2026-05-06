using System.Net.WebSockets;
using System.Text;

namespace BeritaDlanggu.Services
{
    public class InstagramService : IAsyncDisposable
    {
        private ClientWebSocket _ws = new();
        public ClientWebSocket WebSocket => _ws;

        public event Action<string> OnMessage;

        public async Task ConnectAsync()
        {
            if (_ws.State == WebSocketState.Open) return;

            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(new Uri("ws://localhost:8080/ws/"), CancellationToken.None);

            _ = Task.Run(ListenLoop);
        }

        private async Task ListenLoop()
        {
            var buffer = new byte[40960];

            while (_ws.State == WebSocketState.Open)
            {
                var result = await _ws.ReceiveAsync(buffer, CancellationToken.None);
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessage?.Invoke(msg);
            }
        }

        public async Task SendAsync(string message)
        {
            if (_ws.State != WebSocketState.Open) return;

            var buffer = Encoding.UTF8.GetBytes(message);
            await _ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task DisconnectAsync()
        {
            if (_ws.State == WebSocketState.Open || _ws.State == WebSocketState.CloseReceived)
            {
                await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }

            _ws.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
        }
    }
}
