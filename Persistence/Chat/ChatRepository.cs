using ChatApplication.Persistence.Context;
using ChatApplication.Persistence.Models;
using Dapper;

namespace ChatApplication.Persistence.Chat;

public sealed class ChatRepository : IChatRepository
{
    private readonly ISqlLiteContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public ChatRepository(ISqlLiteContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    async Task<IEnumerable<ChatGroup>> IChatRepository.LoadChatList()
    {
        await using var connection = await _context.GetConnection();
        return await connection.QueryAsync<ChatGroup>("SELECT * FROM ChatGroup ORDER BY CreatedAt DESC");
    }

    /// <inheritdoc />
    async Task<ChatGroup> IChatRepository.CreateChatGroup(string name, string description)
    {
        await using var connection = await _context.GetConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            @"INSERT INTO ChatGroup (Name, Description, CreatedAt) 
              VALUES (@Name, @Description, @CreatedAt);
              SELECT last_insert_rowid();",
            new { Name = name, Description = description, CreatedAt = DateTime.UtcNow }
        );

        return new ChatGroup { Id = id, Name = name, Description = description, CreatedAt = DateTime.UtcNow };
    }

    /// <inheritdoc />
    async Task<IEnumerable<Message>> IChatRepository.LoadChatMessages(int chatGroupId, int limit = 50)
    {
        await using var connection = await _context.GetConnection();
        return await connection.QueryAsync<Message>(
            @"SELECT * FROM Message 
              WHERE ChatGroupId = @ChatGroupId 
              ORDER BY SentAt DESC 
              LIMIT @Limit",
            new { ChatGroupId = chatGroupId, Limit = limit }
        );
    }

    /// <inheritdoc />
    async Task<Message> IChatRepository.SaveMessage(Message message)
    {
        await using var connection = await _context.GetConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            @"INSERT INTO Message (SenderId, ChatGroupId, RecipientId, Content, SentAt) 
              VALUES (@SenderId, @ChatGroupId, @RecipientId, @Content, @SentAt);
              SELECT last_insert_rowid();",
            message
        );

        message.Id = id;
        return message;
    }
}