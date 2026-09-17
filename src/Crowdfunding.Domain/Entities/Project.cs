namespace Crowdfunding.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Threashold { get; set; }
    public string Status { get; set; }

    private Project() { }

    public Project(string title, string description, decimal threashold)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A project must have title");
        }
        
        if (threashold <= 0)
        {
            throw new ArgumentException("Threashold value must be greater than zero");
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description?.Trim() ?? string.Empty;
        Threashold = threashold;
        Status = "In progress";
    }
}