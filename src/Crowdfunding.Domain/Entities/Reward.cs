namespace Crowdfunding.Domain.Entities;

public class Reward
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }

    private Reward() { }

    public Reward(string title, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A reward must have title");
        }
        
        if (amount <= 0)
            throw new ArgumentException("Reward amount must be greater than zero.");

        Id = Guid.NewGuid();
        Title = title;
        Amount = amount;
    }
}