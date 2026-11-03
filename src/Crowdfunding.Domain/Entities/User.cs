using System.Text.RegularExpressions;
using Crowdfunding.Domain.Value_Objects;

namespace Crowdfunding.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Email Email { get; set; }
    public decimal Balance { get; set; }
    
    private User() { }

    public User(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A user must have name");
        }
        
        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = Email.Create(email);
        Balance = 0;
    }
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than zero");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be greater than zero");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient balance");

        Balance -= amount;
    }
}