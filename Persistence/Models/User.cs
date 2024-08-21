namespace ChatApplication.Persistence.Models;

public sealed class User
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public DateTime CreatedAt { get; set; }
}