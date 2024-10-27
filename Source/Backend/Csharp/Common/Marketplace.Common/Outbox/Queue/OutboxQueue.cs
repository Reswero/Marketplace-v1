using Dapper;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace Marketplace.Common.Outbox.Queue;

/// <summary>
/// Реализация паттерна Outbox на основе очереди
/// </summary>
/// <typeparam name="T">Тип элемента</typeparam>
public class OutboxQueue<T> : IOutboxQueue<T>
{   
    private const string _tableName = "queue";
    private const string _sqlCreateTable = $"""
        CREATE TABLE IF NOT EXISTS {_tableName} (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            value TEXT NOT NULL
        );
        """;
    private const string _sqlInsert = $"""
        INSERT INTO {_tableName} (value)
        VALUES (@value);
        """;
    private const string _sqlSelectValue = $"""
        SELECT value FROM {_tableName}
        ORDER BY id
        LIMIT @count;
        """;
    private const string _sqlSelectAll = $"""
        SELECT * FROM {_tableName}
        ORDER BY id
        LIMIT @count;
        """;
    private const string _sqlDelete = $"""
        DELETE FROM {_tableName}
        WHERE id in @ids;
        """;

    private readonly string _connectionString;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="databaseName">Название базы данных</param>
    /// <param name="pooling">Включен ли пулинг соединений</param>
    public OutboxQueue(string databaseName, bool pooling = true)
    {
        SqliteConnectionStringBuilder builder = new()
        {
            DataSource = databaseName,
            Pooling = pooling
        };

        _connectionString = builder.ToString();
        CreateTable();
    }

    /// <inheritdoc/>
    public async Task PushAsync(T item, CancellationToken cancellationToken = default)
        => await PushAsync([item], cancellationToken);

    /// <inheritdoc/>
    public async Task PushAsync(T[] items, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();

        List<string> values = new(items.Length);
        foreach (var item in items)
        {
            var value = JsonSerializer.Serialize(item);
            values.Add(value);
        }

        await connection.OpenAsync(cancellationToken);
        var tx = await connection.BeginTransactionAsync(cancellationToken);
        await connection.ExecuteAsync(_sqlInsert, values.Select(v => new { value = v }));
        await tx.CommitAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T> PeekAsync(CancellationToken cancellationToken = default)
        => (await PeekAsync(1, cancellationToken)).Single();

    /// <inheritdoc/>
    public async Task<List<T>> PeekAsync(int count, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        var values = await connection.QueryAsync<string>(_sqlSelectValue, new { count });

        List<T> items = new(count);
        foreach (var value in values)
        {
            var item = JsonSerializer.Deserialize<T>(value)!;
            items.Add(item);
        }

        return items;
    }

    /// <inheritdoc/>
    public async Task<T> PopAsync(CancellationToken cancellationToken = default)
        => (await PopAsync(1, cancellationToken)).Single();

    /// <inheritdoc/>
    public async Task<List<T>> PopAsync(int count, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        var rows = await connection.QueryAsync(_sqlSelectAll, new { count });

        List<T> items = new(count);
        foreach (var row in rows)
        {
            T item = JsonSerializer.Deserialize<T>(row.value)!;
            items.Add(item);
        }

        var ids = rows.Select(r => r.id).ToList();
        await connection.ExecuteAsync(_sqlDelete, new { ids });

        return items;
    }

    private SqliteConnection CreateConnection()
        => new(_connectionString);

    private void CreateTable()
    {
        using var connection = CreateConnection();
        connection.Execute(_sqlCreateTable);
    }
}
