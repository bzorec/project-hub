namespace Direct4Me.Minimal.Api.Models;

public class History
{
    public string Guid { get; set; } = null!;
    public bool Success { get; set; }
    public string? Type { get; set; }
    public string BoxId { get; set; } = null!;
    public string UserName { get; set; } = null!;
}