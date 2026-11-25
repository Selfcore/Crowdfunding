namespace Crowdfunding.Application.Events;

public sealed record ProjectCreatedEvent(
    Guid ProjectId,
    Guid AuthorId,
    string Title,
    decimal Threshold,
    DateTime CreatedAt);