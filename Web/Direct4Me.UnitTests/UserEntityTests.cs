using Direct4Me.Repository.Entities;

namespace Direct4Me.UnitTests;

public class UserEntityTests
{
    [Fact]
    public void IsAlreadyHashed_ValidHash_ReturnsTrue()
    {
        // Arrange
        var userEntity = new UserEntity();
        var hashedPassword = userEntity.HashPassword("admin");

        // Act
        var result = UserEntity.IsAlreadyHashed(Convert.ToBase64String(hashedPassword));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAlreadyHashed_NonHashedPassword_ReturnsFalse()
    {
        // Arrange
        var userEntity = new UserEntity();
        var nonHashedPassword = "Abcd1234%";

        // Act
        var result = UserEntity.IsAlreadyHashed(nonHashedPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsAlreadyHashed_InvalidHash_ReturnsFalse()
    {
        // Arrange
        var userEntity = new UserEntity();
        var invalidHash = "InvalidHash123";

        // Act
        var result = UserEntity.IsAlreadyHashed(invalidHash);

        // Assert
        Assert.False(result);
    }
}