using Microsoft.AspNetCore.SignalR;

namespace ChessMate.Hubs
{
    public class ChessHub : Hub
    {
        public async Task SendMove(string start, string end)
        {
            Console.WriteLine($"Move received: {start} -> {end}");
            await Clients.All.SendAsync("ReceiveMove", start, end);
        }
    }
}
