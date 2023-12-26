using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models.FaceUnlock;

public record FaceUnlockRequest([Required] string Email, [Required] string Base64Image);