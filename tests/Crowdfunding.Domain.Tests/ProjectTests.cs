using Crowdfunding.Domain.Entities;
using Crowdfunding.Domain.Enums;

namespace Crowdfunding.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void Constructor_ShouldCreateProjectInProgress()
    {
        // Arrange
        
        var authorId = Guid.NewGuid();
        string title = "Test Project";
        string description = "Something description...";
        decimal threshold = 10000m;
        
        // Act
        
        var project = new Project(authorId, title, description, threshold);
        
        // Assert
        
        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal(authorId, project.AuthorId);
        Assert.Equal(title, project.Title);
        Assert.Equal(threshold, project.Threshold);
        Assert.Equal(description, project.Description);
        Assert.Equal(ProjectStatus.InProgress, project.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-25)]
    [InlineData(-100)]
    public void Constructor_ShouldThrow_WhenThresholdIsNotPositiveOrZero(decimal threshold)
    {
        // Arrange
        
        var authorId = Guid.NewGuid();
        string title = "Test Project";
        string description = "Something description...";
        
        // Act
        
        var exception = Assert.Throws<ArgumentException>(() => new Project(authorId, title, description, threshold));

        // Assert
        
        Assert.Equal("Threshold value must be greater than zero", exception.Message);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenTitleIsEmpty(string title)
    {
        // Arrange
        
        var authorId = Guid.NewGuid();
        string description = "Something description...";
        decimal threshold = 1000m;
        
        // Act
        
        var exception = Assert.Throws<ArgumentException>(() => new Project(authorId, title, description, threshold));

        // Assert
        
        Assert.Equal("A project must have title", exception.Message);
    }
}