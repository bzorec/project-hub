using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models.Login;

public record LoginResponse(
    [Required] string Guid,
    [Required] string Email,
    [Required] string Fullname,
    [Required] int TotalLogins,
    [Required] DateTime LastAccess,
    [Required] bool IsFaceUnlockEnabled
);