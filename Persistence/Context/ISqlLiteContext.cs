using Microsoft.Data.Sqlite;

namespace ChatApplication.Persistence.Context;

/// <summary>
/// 
/// </summary>
public interface ISqlLiteContext
{
    /// <summary>
    /// 
    /// </summary>
    Task<SqliteConnection> GetConnection();
}