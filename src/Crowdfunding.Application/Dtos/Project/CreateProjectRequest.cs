namespace Crowdfunding.Application.Dtos.Project;

public class CreateProjectRequest
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Threshold { get; set; }
}