using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Direct4Me.Blazor.Models;
using Microsoft.IdentityModel.Tokens;

namespace Direct4Me.Blazor.Providers;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IConfiguration _config;

    public JwtTokenProvider(IConfiguration config)
    {
        _config = config;
    }

    public Task<string> GenerateJwtTokenAsync(UserModel user)
    {
        if (!user.Token.IsNullOrEmpty())
            return Task.FromResult(user.Token)!;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FirstName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new
            InvalidOperationException()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}

public interface IJwtTokenProvider
{
    Task<string> GenerateJwtTokenAsync(UserModel user);
}