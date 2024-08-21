using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatApplication.Persistence.Context;

public sealed class SqlLiteContext : ISqlLiteContext
{
    private const string DatabaseName = "app.db";

    private SqliteConnection CreateDatabaseConnection()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = DatabaseName,
            Mode = SqliteOpenMode.ReadWriteCreate
        };
        return new SqliteConnection(connectionStringBuilder.ConnectionString);
    }

    /// <inheritdoc />
    public async Task<SqliteConnection> GetConnection()
    {
        SqliteConnection connection = CreateDatabaseConnection();
        await EnsureDatabaseIntegrity(connection);
        return connection;
    }

    private async Task EnsureDatabaseIntegrity(SqliteConnection connection)
    {
        try
        {
            await connection.OpenAsync();
            await CreateTablesIfNotExist(connection);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == SQLitePCL.raw.SQLITE_CORRUPT)
        {
            await RecreateDatabase(connection);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == SQLitePCL.raw.SQLITE_READONLY)
        {
            await HandleReadOnlyDatabase(connection);
        }
    }

    private async Task CreateTablesIfNotExist(SqliteConnection connection)
    {
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS User (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                CreatedAt DATETIME NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ChatGroup (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT,
                CreatedAt DATETIME NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Message (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                SenderId INTEGER NOT NULL,
                ChatGroupId INTEGER,
                RecipientId INTEGER,
                Content TEXT NOT NULL,
                SentAt DATETIME NOT NULL,
                FOREIGN KEY (SenderId) REFERENCES User(Id),
                FOREIGN KEY (ChatGroupId) REFERENCES ChatGroup(Id),
                FOREIGN KEY (RecipientId) REFERENCES User(Id)
            );

            CREATE TABLE IF NOT EXISTS UserChatGroup (
                UserId INTEGER NOT NULL,
                ChatGroupId INTEGER NOT NULL,
                JoinedAt DATETIME NOT NULL,
                PRIMARY KEY (UserId, ChatGroupId),
                FOREIGN KEY (UserId) REFERENCES User(Id),
                FOREIGN KEY (ChatGroupId) REFERENCES ChatGroup(Id)
            );
        ");
    }

    private async Task RecreateDatabase(SqliteConnection connection)
    {
        connection.Close();
        File.Delete(DatabaseName);
        await EnsureDatabaseIntegrity(connection);
    }

    private async Task HandleReadOnlyDatabase(SqliteConnection connection)
    {
        connection.Close();

        // Check if the file exists and is read-only
        var fileInfo = new FileInfo(DatabaseName);
        if (fileInfo.Exists && fileInfo.IsReadOnly)
        {
            // Attempt to make the file writable
            fileInfo.IsReadOnly = false;
        }

        // If the file doesn't exist or we've made it writable, try to recreate the database
        await EnsureDatabaseIntegrity(connection);
    }
}