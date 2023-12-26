using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models.Login;

public record LoginRequest(
    [Required] string Email,
    [Required] string Password
);