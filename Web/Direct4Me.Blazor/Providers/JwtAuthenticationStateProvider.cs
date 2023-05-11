using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Direct4Me.Blazor.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Direct4Me.Blazor.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public JwtAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await _localStorage.GetItemAsync<string>("accessToken");

        if (string.IsNullOrWhiteSpace(accessToken))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        try
        {
            var userInfo = await _httpClient.GetFromJsonAsync<UserModel>("/api/userinfo");
            if (userInfo == null) throw new ArgumentNullException(nameof(userInfo));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.FirstName),
                new Claim(ClaimTypes.Email, userInfo.Email),
            };

            var identity = new ClaimsIdentity(claims, "jwt");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            await _localStorage.RemoveItemAsync("accessToken");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}