using ChatApplication.Persistence.Models;

namespace ChatApplication.Persistence.Chat;

public interface IChatRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ChatGroup>> LoadChatList();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    Task<ChatGroup> CreateChatGroup(string name, string description);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatGroupId"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    Task<IEnumerable<Message>> LoadChatMessages(int chatGroupId, int limit = 50);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<Message> SaveMessage(Message message);
}