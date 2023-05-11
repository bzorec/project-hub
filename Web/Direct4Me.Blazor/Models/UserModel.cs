namespace Direct4Me.Blazor.Models;

public record UserModel(Guid Id, string Email, string Password, string FirstName, string LastName,
    UserRole Role = UserRole.Normal,
    string? Token = "")
{
    public string? Token { get; set; } = Token;
}