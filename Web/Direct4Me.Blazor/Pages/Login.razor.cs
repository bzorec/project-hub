using System.ComponentModel.DataAnnotations;
using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Login
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJwtService JwtService { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }

    private string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }

    public SigninModel LoginModel { get; set; } = new();

    public async Task HandleLogin()
    {
        var signedIn = await UserService.TrySignInAsync(LoginModel.Email, LoginModel.Password);

        if (!signedIn)
        {
            ErrorMessage = "Wrong email or password.";
            return;
        }

        var user = await UserService.GetUserByEmailAsync(LoginModel.Email);
        try
        {
            var jwtToken = JwtService.GenerateJwtToken(LoginModel.Email, user?.Fullname);

            await JsInteropService.SetToken(jwtToken);

            NavigationManager.NavigateTo("/dashboard", true, true);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    public class SigninModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}