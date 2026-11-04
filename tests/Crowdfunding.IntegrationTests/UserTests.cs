using Crowdfunding.Application.Interfaces;
using Crowdfunding.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Crowdfunding.IntegrationTests;

public class UserTests : BaseIntegrationTest
{
    private readonly IRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UserTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
        _repository = Services.GetRequiredService<IRepository<User>>();
        _unitOfWork = Services.GetRequiredService<IUnitOfWork>();
    }
    
    [Fact]
    public async Task AddAsync_ShouldSaveUserToDatabase()
    {
        // Arrange
        var user = new User("John Doe", "john.doe@example.com");

        // Act
        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var dbUser = await DbContext.Users
            .FindAsync(user.Id);

        Assert.NotNull(dbUser);
        Assert.Equal(user.Id, dbUser.Id);
        Assert.Equal("John Doe", dbUser.Name);
        Assert.Equal(0, dbUser.Balance);
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User("John Doe", "john.doe2@example.com");

        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var dbUser = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(dbUser);
        Assert.Equal(user.Id, dbUser.Id);
        Assert.Equal(user.Name, dbUser.Name);
        Assert.Equal(user.Balance, dbUser.Balance);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var user = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        Assert.Null(user);
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var user = new User("John Doe", "john.doe3@example.com");

        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Act
        await _repository.Delete(user);

        // Assert
        var checkUser = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        
        Assert.Null(checkUser);
    }
    
    [Fact]
    public async Task Delete_ShouldNotAffectOtherUsers()
    {
        // Arrange
        var user1 = new User("John Doe", "john5@example.com");
        var user2 = new User("Jane Doe", "jane5@example.com");

        await _repository.AddAsync(user1);
        await _repository.AddAsync(user2);
        await _unitOfWork.SaveChangesAsync();

        // Act
        await _repository.Delete(user1);

        // Assert
        var deletedUser = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user1.Id);

        var existingUser = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user2.Id);

        Assert.Null(deletedUser);
        Assert.NotNull(existingUser);
    }
}