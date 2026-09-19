using Crowdfunding.Domain.Enums;

namespace Crowdfunding.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Threshold { get; set; }
    public ProjectStatus Status { get; private set; }

    private Project() { }

    public Project(Guid authorId, string title, string description, decimal threshold)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentException("Project must have an author");
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A project must have title");
        }
        
        if (threshold <= 0)
        {
            throw new ArgumentException("Threshold value must be greater than zero");
        }

        Id = Guid.NewGuid();
        AuthorId = authorId;
        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Threshold = threshold;
        Status = ProjectStatus.InProgress;
    }
    
    public void Complete()
    {
        if (Status != ProjectStatus.InProgress)
            throw new InvalidOperationException("Only an active project can be completed.");

        Status = ProjectStatus.Completed;
    }
    
    public void Cancel()
    {
        if (Status != ProjectStatus.InProgress)
            throw new InvalidOperationException("Only an active project can be cancelled.");

        Status = ProjectStatus.Cancelled;
    }
}