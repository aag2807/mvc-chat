
using ChatApplication.Persistence.Context;
using Dapper;
using System.Security.Cryptography;
using System.Text;
using ChatApplication.Persistence.Models;

namespace ChatApplication.Persistence.Utils;

public class DummyDataGenerator
{
    private readonly ISqlLiteContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public DummyDataGenerator(ISqlLiteContext context)
    {
        _context = context;
    }

    public async Task GenerateDummyData()
    {
        await using var connection = await _context.GetConnection();

        // Create users
        List<User> users =
        [
            new User { Username = "alice", Email = "alice@example.com", PasswordHash = HashPassword("password123"), CreatedAt = DateTime.UtcNow },
            new User { Username = "bob", Email = "bob@example.com", PasswordHash = HashPassword("password123"), CreatedAt = DateTime.UtcNow },
            new User { Username = "charlie", Email = "charlie@example.com", PasswordHash = HashPassword("password123"), CreatedAt = DateTime.UtcNow }
        ];

        foreach (User user in users)
        {
            await connection.ExecuteAsync(
                "INSERT INTO User (Username, Email, PasswordHash, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @CreatedAt)",
                user);
        }

        // Create chat groups
        List<ChatGroup> chatGroups = new List<ChatGroup>
        {
            new() { Name = "General Chat", Description = "General discussion for all users", CreatedAt = DateTime.UtcNow },
            new() { Name = "Tech Talk", Description = "Discussions about technology", CreatedAt = DateTime.UtcNow }
        };

        foreach (ChatGroup group in chatGroups)
        {
            await connection.ExecuteAsync(
                "INSERT INTO ChatGroup (Name, Description, CreatedAt) VALUES (@Name, @Description, @CreatedAt)",
                group);
        }

        // Add users to chat groups
        List<UserChatGroup> userChatGroups =
        [
            new() { UserId = 1, ChatGroupId = 1, JoinedAt = DateTime.UtcNow },
            new() { UserId = 2, ChatGroupId = 1, JoinedAt = DateTime.UtcNow },
            new() { UserId = 3, ChatGroupId = 1, JoinedAt = DateTime.UtcNow },
            new() { UserId = 1, ChatGroupId = 2, JoinedAt = DateTime.UtcNow },
            new() { UserId = 2, ChatGroupId = 2, JoinedAt = DateTime.UtcNow }
        ];

        foreach (UserChatGroup ucg in userChatGroups)
        {
            await connection.ExecuteAsync(
                "INSERT INTO UserChatGroup (UserId, ChatGroupId, JoinedAt) VALUES (@UserId, @ChatGroupId, @JoinedAt)",
                ucg);
        }

        // Create some messages
        List<Message> messages =
        [
            new() { SenderId = 1, ChatGroupId = 1, Content = "Hello everyone!", SentAt = DateTime.UtcNow.AddMinutes(-30) },
            new() { SenderId = 2, ChatGroupId = 1, Content = "Hi Alice!", SentAt = DateTime.UtcNow.AddMinutes(-25) },
            new() { SenderId = 3, ChatGroupId = 1, Content = "Welcome to the general chat!", SentAt = DateTime.UtcNow.AddMinutes(-20) },
            new() { SenderId = 1, ChatGroupId = 2, Content = "Anyone interested in discussing the latest tech news?", SentAt = DateTime.UtcNow.AddMinutes(-15) },
            new() { SenderId = 2, ChatGroupId = 2, Content = "Sure, what's on your mind?", SentAt = DateTime.UtcNow.AddMinutes(-10) }
        ];

        foreach (Message message in messages)
        {
            await connection.ExecuteAsync(
                "INSERT INTO Message (SenderId, ChatGroupId, Content, SentAt) VALUES (@SenderId, @ChatGroupId, @Content, @SentAt)",
                message);
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}