using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Hubs
{
    public class SignalRChatHub :Hub
    {
        public const string HubUrl = "/chat";

        public const string CustomerServiceWaitingRoom = "CSWait";

        public string MessageForAdmin = "[Notice] User is waiting for assistance";

        public async Task JoinRoomCSWaiting(string username)
        {
            await Groups.AddToGroupAsync(this.Context.ConnectionId,CustomerServiceWaitingRoom);
            if(username=="Administrator") { this.MessageForAdmin = "Monitoring requests started successfully."; }
            await Clients.Group(CustomerServiceWaitingRoom).SendAsync("BroadcastToAdmin", username, this.MessageForAdmin);
          
        }

        public async Task JoinMainRoom(string usernameForGroup)
        {
            await Groups.AddToGroupAsync(this.Context.ConnectionId, usernameForGroup);

        }

        public async Task BroadcastToMain(string username, string message)
        {
            await Clients.Group(username).SendAsync("BroadcastToMain", username, message);
        }

        public async Task BroadcastToMainForAdmin(string userNameForGroup,string username, string message)
        {
            await Clients.Group(userNameForGroup).SendAsync("BroadcastToMain", username, message);
        }

        public async Task BroadcastToAdmin(string username, string message)
        {
            await Clients.Group(CustomerServiceWaitingRoom).SendAsync("broadcastToAdmin", username, message);
        }
    }
}
