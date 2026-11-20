namespace Crowdfunding.Application.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
}