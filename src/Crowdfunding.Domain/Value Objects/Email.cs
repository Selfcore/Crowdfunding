using System.Text.RegularExpressions;

namespace Crowdfunding.Domain.Value_Objects;

public class Email
{
    public string Address { get; private set; }

    public string Host { get; private set; }

    public string User { get; private set; }
    
    private Email() { }

    private Email(string email)
    {
        var parts = email.Split('@');

        Address = email;
        User = parts[0];
        Host = parts[1];
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }
        
        email = email.Trim();

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return new Email(email);
    }
    
    public override string ToString()
    {
        return this.Address;
    }
}