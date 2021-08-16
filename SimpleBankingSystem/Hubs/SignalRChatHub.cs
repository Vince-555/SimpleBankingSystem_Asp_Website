using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Hubs
{
    public class SignalRChatHub :Hub
    {
        public const string HubUrl = "/chat";

        public const string CustomerServiceWaitingRoom = "CSWait";

        public const string MessageForAdmin = "[Notice] User is waiting for assistance";

        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }

        public async Task JoinRoomCSWaiting(string username)
        {
            await Groups.AddToGroupAsync(this.Context.ConnectionId,CustomerServiceWaitingRoom);
            await Clients.Group(CustomerServiceWaitingRoom).SendAsync("BroadcastToAdmin", username, MessageForAdmin);
        }

        public async Task JoinMainRoom(string usernameForGroup)
        {
            await Groups.AddToGroupAsync(this.Context.ConnectionId, usernameForGroup);

        }

        public async Task BroadcastToMain(string username, string message)
        {
            await Clients.Group(username).SendAsync("BroadcastToMain", username, message);
        }

        public async Task BroadcastToAdmin(string username, string message)
        {
            await Clients.Group(CustomerServiceWaitingRoom).SendAsync("broadcastToAdmin", username, message);
        }
    }
}
