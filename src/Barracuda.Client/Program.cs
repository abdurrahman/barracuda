using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Barracuda.Application.Users.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace Barracuda.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userList = new List<LoginModel>
            {
                new LoginModel("Quentin", "admin"),
                new LoginModel("Martin", "admin"),
                new LoginModel("David", "admin")
            };
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5206/messagehub")
                // .WithAutomaticReconnect()
                .Build();
            
            connection.On("ReceiveMessage",
                (DateTime timestamp, string username, string message) =>
                {
                    Console.WriteLine($"{timestamp}: {username} | {message}");
                });

            connection.On("BlockUser", (DateTime timestamp, string username) =>
            {
                Console.WriteLine($"{timestamp}: {username} has blocked you.");
            });
            
            Console.WriteLine("-Barracuda Message Service-\nWelcome");
            Console.WriteLine("connecting...");
            
            await connection.StartAsync().ContinueWith(c =>
            {
                GetClientStatus(c, "connected, you're in !");
            });

            Console.WriteLine("Now you can start to chat with @everyone");
            Console.Write("Choose your credential\n{0} Quentin, {1} Martin, {2} David : ");
            var pickedCredential = Convert.ToInt32(Console.ReadLine());

            var loginData = userList[pickedCredential];
            var response = await connection.InvokeCoreAsync<LoginModel>("Login", new object[]
            {
                loginData.UserName, 
                loginData.Password
            });

            var nickname = loginData.UserName;
            
            try
            {
                string message;
                do
                {
                    message = Console.ReadLine();
                    if (message?.ToLower() == "block")
                    {
                        Console.Write("Enter your chat mate's username: ");
                        var blockedUserName = Console.ReadLine();

                        await connection.InvokeCoreAsync("BlockUser", new object[] {blockedUserName});
                    }
                    else
                        await connection.InvokeCoreAsync("SendMessage", new object[] {nickname, message});
                } while (message?.ToLower() != "exit");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            
            await connection.DisposeAsync().ContinueWith(c =>
            {
                GetClientStatus(c, "disconnected");
            });
        }

        private static void GetClientStatus(Task task, string message)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine(task.Exception?.GetBaseException());
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}