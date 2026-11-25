using System.Text.Json;
using Confluent.Kafka;
using Crowdfunding.Application.Dtos.Reward;
using Crowdfunding.Application.Events;
using Crowdfunding.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crowdfunding.BackgroundWorkers;

public class ProjectOnboardingConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProjectOnboardingConsumer> _logger;
    
    private const string TopicName = "project-created";
    private const string GroupId = "crowdfunding-project-onboarding-group";

    public ProjectOnboardingConsumer(IConfiguration configuration, 
        IServiceScopeFactory scopeFactory, ILogger<ProjectOnboardingConsumer> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
    } 
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
    }
    
    private async Task ConsumeLoop(CancellationToken stoppingToken)
    {
        var bootstrapServers = _configuration.GetConnectionString("Kafka")
                               ?? throw new InvalidOperationException("Kafka connection string is missing.");

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(TopicName);

        _logger.LogInformation("ProjectOnboardingConsumer started listening on '{Topic}'", TopicName);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var @event = JsonSerializer.Deserialize<ProjectCreatedEvent>(result.Message.Value);

                    if (@event is not null)
                    {
                        await ProcessProjectOnboardingAsync(@event, stoppingToken);
                    }

                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                    await Task.Delay(2000, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Failed to onboard project from Kafka event");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Коректне завершення при зупинці контейнера
        }
        finally
        {
            consumer.Close();
        }
    }
    
    private async Task ProcessProjectOnboardingAsync(ProjectCreatedEvent @event, CancellationToken ct)
    {
        _logger.LogInformation(
            "[ONBOARDING WORKER] Processing new project '{Title}' ({ProjectId})...",
            @event.Title, @event.ProjectId);

        using var scope = _scopeFactory.CreateScope();
        var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();

        var starterAmount = Math.Max(10m, Math.Round(@event.Threshold * 0.05m, 0));
        var vipAmount = Math.Max(50m, Math.Round(@event.Threshold * 0.20m, 0));

        await projectService.CreateReward(
            @event.ProjectId,
            new CreateRewardRequest
            {
                Title = "Early Bird Supporter (Auto-generated)",
                Amount = starterAmount
            },
            ct);

        await projectService.CreateReward(
            @event.ProjectId,
            new CreateRewardRequest
            {
                Title = "VIP Backer Pack (Auto-generated)",
                Amount = vipAmount,
            },
            ct);

        var details = await projectService.GetDetails(@event.ProjectId, ct);

        _logger.LogInformation(
            "[ONBOARDING WORKER] Created {RewardsCount} starter rewards and warmed up Redis cache for '{Title}'!",
            details.Rewards.Count, @event.Title);
    }
}