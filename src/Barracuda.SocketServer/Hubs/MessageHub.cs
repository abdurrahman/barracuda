using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Application.Message;
using Barracuda.Application.Users;
using Barracuda.Application.Users.Dtos;
using Barracuda.Core.Authorization;
using Flurl.Http;
using Microsoft.AspNetCore.SignalR;

namespace Barracuda.SocketServer.Hubs
{ 
    public class MessageHub : Hub
    {
        // Todo: Move this appsettings and use IOptions<> for setting values.
        private const string BaseApiUrl = "http://localhost:5106/api";
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        // Todo: store credential info into the cache.
        private static ConcurrentDictionary<string, JwtSecurityToken> ConnectedUserTokens { get; set; } =
            new ConcurrentDictionary<string, JwtSecurityToken>();

        private static ConcurrentDictionary<string, string> ConnectedUsers { get; set; } =
            new ConcurrentDictionary<string, string>();

        public MessageHub(IMessageService messageService, IUserService service)
        {
            _messageService = messageService;
            _userService = service;
        }

        public async Task SendMessage(string username, string message)
        {
            await _messageService.SendMessage(new MessageDto
            {
                Text = message,
                SenderId = string.Empty, // to everyone
                RecipientId = Context.ConnectionId
            });

            await Clients.All.SendAsync("ReceiveMessage", DateTime.Now, username, message);
        }

        // Todo: Refactor this method
        [HubMethodName("BlockUser")]
        public async Task BlockUser(string username)
        {
            var blockedUser = await _userService.GetUserByUserName(username);

            var complaintUser = ConnectedUsers[Context.ConnectionId];
            var complaintToken = ConnectedUserTokens[complaintUser];
            var complaintUserId = complaintToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            await _userService.BlockUser(new UserBlockDto
            {
                BlockedUserId = blockedUser.Id,
                ComplaintUserId = complaintUserId,
            });
            
            var blockedConnectionId = ConnectedUsers.FirstOrDefault(c => c.Value == username).Key;
            // Check if opposite user is online
            if (!string.IsNullOrEmpty(blockedConnectionId))
            {
                await Clients.Client(blockedConnectionId).SendAsync("BlockUser", complaintUser);   
            }
        }

        // Todo: Refactor this method
        [HubMethodName("Login")]
        public async Task Login(string username, string password)
        {
            var isAlreadyLoggedIn = ConnectedUsers.Any(c => c.Value == username);
            if (isAlreadyLoggedIn)
            {
                Context.Abort(); 
                return;
            }
            
            var loginPayload = new LoginModel(username, password);
            var token = await GetTokenFromApi(loginPayload);
            if (token == null)
            {
                await OnDisconnectedAsync(new Exception("Incorrect user or password"));
            }

            var name = token?.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            
            ConnectedUsers[Context.ConnectionId] = username;
            ConnectedUserTokens[username] = token;

            await Clients.All.SendAsync("ReceiveMessage", DateTime.Now, username,
                $"{username} has logged in. Count: {ConnectedUsers.Count}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.TryRemove(Context.ConnectionId, out var username);
            ConnectedUserTokens.TryRemove(username, out var token);
            await Clients.All.SendAsync("OnDisconnected", DateTime.Now, username, ConnectedUsers.Count);
        }

        private async Task<JwtSecurityToken> GetTokenFromApi(LoginModel loginPayload)
        {
            try
            {
                var apiResponse = await $"{BaseApiUrl}/account/login"
                    .WithHeader("Content-Type", "application/json")
                    .PostJsonAsync(loginPayload)
                    .ReceiveJson<TokenResponseModel>();

                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(apiResponse.Token);
            }
            catch (FlurlHttpException ex)
            {
                await OnDisconnectedAsync(ex);
            }
            catch (Exception ex)
            {
                await OnDisconnectedAsync(ex);
            }

            return null;
        }
    }
}