using Crowdfunding.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Crowdfunding.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected IServiceProvider Services => _scope.ServiceProvider;
    protected readonly ApiDbContext DbContext; 
    
    protected BaseIntegrationTest(IntegrationTestWebFactory factory)
    {
        _scope = factory.Services.CreateScope();
        
        DbContext = _scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}