using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models;

[Serializable]
public record User(
    [Required] string Guid,
    [Required] string Email,
    [Required] string Password,
    [Required] string Fullname,
    [Required] int TotalLogins,
    [Required] DateTime LastAccess
);