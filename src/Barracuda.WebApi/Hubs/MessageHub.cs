using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Barracuda.WebApi.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("NewMessage", DateTime.Now, message);
        }

        public async Task BlockUser(string userId, string message)
        {
            await Clients.Client(userId).SendAsync("BlockMember", DateTime.Now);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("OnConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("OnDisconnected", DateTime.Now);
        }
    }
}