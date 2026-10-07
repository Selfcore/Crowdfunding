using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Domain.Tests;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUser()
    {
        const string name = "  John Doe  ";
        const string email = "  john@example.com  ";

        var user = new User(name, email);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal("john@example.com", user.Email);
        Assert.Equal(0m, user.Balance);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenNameIsEmpty(string name)
    {
        var exception = Assert.Throws<ArgumentException>(() => new User(name, "john@example.com"));

        Assert.Equal("A user must have name", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("test")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    [InlineData("test@example")]
    public void Constructor_ShouldThrow_WhenEmailIsInvalid(string email)
    {
        var exception = Assert.Throws<ArgumentException>(() => new User("John", email));

        Assert.Equal("Invalid email format", exception.Message);
    }

    [Theory]
    [InlineData("john@example.com")]
    [InlineData("john.doe@example.com")]
    [InlineData("john+test@example.co.uk")]
    [InlineData("JOHN@EXAMPLE.COM")]
    public void Constructor_ShouldAcceptValidEmail(string email)
    {
        var exception = Record.Exception(() => new User("John", email));

        Assert.Null(exception);
    }
}