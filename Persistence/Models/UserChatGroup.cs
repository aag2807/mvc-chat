namespace ChatApplication.Persistence.Models;

public sealed class UserChatGroup
{
    /// <summary>
    /// 
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int ChatGroupId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ChatGroup ChatGroup { get; set; }
}