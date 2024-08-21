using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs;

public sealed class ChatHub : Hub
{
    public ChatHub()
    {
        
    }
    
    public override Task OnConnectedAsync()
    {
        
        return base.OnConnectedAsync();
    }

    public async Task SendMessageInGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }
}