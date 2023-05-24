using System.ComponentModel.DataAnnotations;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Register
{
    private const string GuideContent =
        "This is the sign-up page. Fill out the required information to create a new account.";

    private const string GuideTitle = "Sign Up Guide";
    private bool ShowPopupGuide { get; set; } = true;
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    public RegistrationModel RegisterModel { get; set; } = new();

    private string? ErrorMessage { get; set; }

    public bool IsLoading { get; set; }

    public bool IsSuccess { get; set; }


    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

    private async Task HandleRegistration()
    {
        try
        {
            IsSuccess = await UserService.TrySignUpAsync(new UserEntity
            {
                Email = RegisterModel.RegisterEmail,
                Password = RegisterModel.RegisterPassword.Trim(),
                FirstName = RegisterModel.RegisterFirstName,
                LastName = RegisterModel.RegisterLastName
            });

            if (!IsSuccess) ErrorMessage = "User already exists.";

            NavigationManager.NavigateTo("/login", true, true);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    public class RegistrationModel
    {
        [Required] public string RegisterEmail { get; set; } = null!;

        [Required] public string RegisterPassword { get; set; } = null!;

        [Required] public string RegisterFirstName { get; set; } = null!;

        [Required] public string RegisterLastName { get; set; } = null!;
    }
}