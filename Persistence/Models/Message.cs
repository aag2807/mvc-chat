namespace ChatApplication.Persistence.Models;

public sealed class Message
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? ChatGroupId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? RecipientId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public DateTime SentAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public User Sender { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ChatGroup ChatGroup { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public User Recipient { get; set; }
}