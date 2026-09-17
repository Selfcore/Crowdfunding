using System.Text.RegularExpressions;

namespace Crowdfunding.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    
    private User() { }

    public User(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A user must have name");
        }

        email = email.Trim();        
        
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format");
        }
        
        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email;
        Balance = 0;
    }
    
    private static bool IsValidEmail(string email) 
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
}