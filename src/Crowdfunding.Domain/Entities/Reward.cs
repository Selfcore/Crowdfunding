namespace Crowdfunding.Domain.Entities;

public class Reward
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; private set; }
    public decimal Amount { get; private set; }

    private Reward() { }

    public Reward(Guid projectId, string title, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A reward must have title");
        }
        
        if (amount <= 0)
            throw new ArgumentException("Reward amount must be greater than zero.");

        Id = Guid.NewGuid();
        ProjectId = projectId;
        Title = title;
        Amount = amount;
    }
    
    public void Update(string title, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Reward title is required");

        if (amount <= 0)
            throw new ArgumentException("Reward amount must be greater than zero");

        Title = title.Trim();
        Amount = amount;
    }
}