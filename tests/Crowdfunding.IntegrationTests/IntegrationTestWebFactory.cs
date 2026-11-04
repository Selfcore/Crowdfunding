using Crowdfunding.Infrastructure.Postgres;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Crowdfunding.IntegrationTests;

public class IntegrationTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    public IntegrationTestWebFactory()
    {
        _dbContainer = new PostgreSqlBuilder().WithImage("postgres:17-alpine")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("crowdfunding_db")
            .Build();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor =  services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApiDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        var migrationSql = await File.ReadAllTextAsync("migration.sql");
        
        await _dbContainer.ExecScriptAsync(migrationSql);
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}