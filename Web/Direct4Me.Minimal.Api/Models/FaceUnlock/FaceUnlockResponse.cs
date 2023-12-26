using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models.FaceUnlock;

public record FaceUnlockResponse(
    [Required] string Guid,
    [Required] string Email,
    [Required] string Fullname,
    [Required] int TotalLogins,
    [Required] DateTime LastAccess);