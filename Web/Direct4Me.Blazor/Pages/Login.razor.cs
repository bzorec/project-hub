using System.ComponentModel.DataAnnotations;
using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Login
{
    private const string GuideContent =
        "This is the login page. Enter your email address and password to sign in. Face unlock is also available.";

    private const string GuideTitle = "Welcome!";
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IJwtService JwtService { get; set; } = null!;
    [Inject] private IJsInteropService JsInteropService { get; set; } = null!;
    [Inject] private ILogger<Login> Logger { get; set; } = null!;

    private string? ErrorMessage { get; set; }
    private bool IsLoading { get; set; }

    public SigninModel LoginModel { get; set; } = new();

    private bool ShowPopupGuide { get; set; } = true;

    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

    public async Task HandleLogin()
    {
        IsLoading = true;
        ErrorMessage = null;

        if (!await UserService.TrySignInAsync(LoginModel.Email, LoginModel.Password))
        {
            ErrorMessage = "Incorrect email or password. Please try again.";
            IsLoading = false;
            return;
        }

        await ProcessSuccessfulLogin();
    }

    private async Task ProcessSuccessfulLogin()
    {
        try
        {
            var user = await UserService.GetUserByEmailAsync(LoginModel.Email);
            Logger.LogInformation("Generating JWT token for user: {Email}", user?.Email);
            if (user != null)
            {
                var jwtToken = JwtService.GenerateJwtToken(user.Email, user.Fullname);

                await JsInteropService.SetToken(jwtToken);
            }

            Logger.LogInformation("JWT token generated and set successfully");

            NavigationManager.NavigateTo("/dashboard", true, true);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during login process for user: {Email}", LoginModel.Email);

            ErrorMessage = "An error occurred during login. Please try again later.";
        }
        finally
        {
            IsLoading = false;
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