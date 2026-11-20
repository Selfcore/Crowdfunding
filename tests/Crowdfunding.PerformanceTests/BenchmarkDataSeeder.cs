using Npgsql;

namespace Crowdfunding.PerformanceTests;

public sealed class BenchmarkDataSeeder
{
    private readonly string _connectionString;

    public BenchmarkDataSeeder(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<Guid>> SeedUsersAsync(int count, CancellationToken ct)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);

        await using var connection = await dataSource.OpenConnectionAsync(ct);

        await using var command = connection.CreateCommand();

        command.CommandText = """
                              TRUNCATE TABLE "Users" CASCADE;

                              INSERT INTO "Users" ("Id", "Name", "Email", "Balance")
                              SELECT
                                  gen_random_uuid(),
                                  'user_' || n,
                                  'user_' || n || '@benchmark.test',
                                  0
                              FROM generate_series(1, @count) AS n;

                              SELECT "Id"
                              FROM "Users"
                              ORDER BY "Name";
                              """;

        command.Parameters.AddWithValue("count", count);

        var userIds = new List<Guid>(count);

        await using var reader = await command.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            userIds.Add(reader.GetGuid(0));
        }

        return userIds;
    }
}