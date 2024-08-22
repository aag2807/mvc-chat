using ChatApplication.Persistence.Chat;
using ChatApplication.Persistence.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs;

public sealed class ChatHub : Hub
{
    private readonly IChatRepository _chatRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatRepository"></param>
    public ChatHub(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public override async Task OnConnectedAsync()
    {
        await LoadChatList();
        await base.OnConnectedAsync();
    }

    public async Task SendMessageInGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    private async Task LoadChatList()
    {
        IEnumerable<ChatGroup> chats = await _chatRepository.LoadChatList().ConfigureAwait(true);
        await Clients.Caller.SendAsync("LoadChatList", chats);
    }

    public async Task GetMessages(int groupId)
    {
        IEnumerable<Message> messages = await _chatRepository.LoadChatMessages(groupId).ConfigureAwait(true);
        await Clients.Caller.SendAsync("ReceiveMessages", messages).ConfigureAwait(true);
    }
}