namespace Crowdfunding.Application.Dtos.User;

public sealed class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}