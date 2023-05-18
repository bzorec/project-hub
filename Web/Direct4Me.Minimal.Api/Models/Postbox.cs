using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models;

[Serializable]
public record Postbox(
    string Guid,
    [Required] int PostboxId,
    [Required] string UserGuid,
    int TotalUnlocks,
    DateTime LastAccess
);