using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Application.Message;
using Microsoft.AspNetCore.SignalR;

namespace Barracuda.WebSocket.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageService _messageService;
        
        public MessageHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        private static ConcurrentDictionary<string, string> Users { get; set; } = new ConcurrentDictionary<string, string>();

        public async Task SetUserName(string username)
        {
            bool isTaken = Users.Any(p => p.Value == username);
            if (isTaken) { Context.Abort(); return; }
            Users[Context.ConnectionId] = username;
            await Clients.All.SendAsync("OnJoin", DateTime.Now, username, Users.Count);
        }

        public async Task SendMessage(string message)
        {
            string username = Users[Context.ConnectionId];
            await Clients.All.SendAsync("NewMessage", DateTime.Now, username, message);
            await _messageService.SendMessage(new MessageDto
            {
                Text = message,
                RecipientId = "4d4b8088-c7ce-4b3c-b5fb-496b1d3df67c",
                SenderId = "5b44139f-9f0f-4add-a5e7-f29fea52788e"
            });
        }

        public async Task SendMessageToCaller(string message)
        {
            string username = Users[Context.ConnectionId];
            
            await Clients.Caller.SendAsync("NewMessage", DateTime.Now, username, message);
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
            Users.TryRemove(Context.ConnectionId, out var username);
            await Clients.All.SendAsync("OnDisconnected", DateTime.Now, username, Users.Count);
        }
    }
}