using System.Text.Json.Serialization;

namespace Direct4Me.Core.Auth;

public class AuthenticationResponse
{
    [JsonPropertyName("user_id")] public string UserId { get; set; }

    [JsonPropertyName("confidence")] public float Confidence { get; set; }
}