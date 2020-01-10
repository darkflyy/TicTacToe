using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TicTacToe.Web.Hubs
{
    public class PlayingRoomListHub : Hub
    {
        public async Task SendToAllAsync(string method, object data)
        {
            await Clients.All.SendAsync(method, data);
        }
    }
}