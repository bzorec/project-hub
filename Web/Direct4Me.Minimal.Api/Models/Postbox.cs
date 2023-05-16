using System.ComponentModel.DataAnnotations;

namespace Direct4Me.Minimal.Api.Models;

[Serializable]
internal record Postbox(
    [Required] string Guid,
    [Required] int PostboxId,
    [Required] int TotalUnlocks,
    [Required] DateTime LastAccess
);