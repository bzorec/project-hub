using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class User
{
    private const string GuideContent =
        "This is the profile page. Here you can edit your profile. Face unlock is also available, to enable it click the button and take a selfie.";

    private const string GuideTitle = "Profile guide!";
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJwtService JwtService { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }
    public string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }

    private bool ShowPopupGuide { get; set; } = true;

    public UserEntity UserModel { get; set; } = new UserEntity();


    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            string email = await JsInteropService.GetUserEmail();
            if (email != null) UserModel = await UserService.GetUserByEmailAsync(email) ?? new UserEntity();
        }

        StateHasChanged();
    }

    private async Task OpenUpdateModal()
    {
        try
        {
            await JsInteropService.OpenModalWindow();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task UpdateUser()
    {
        try
        {
            var result = await UserService.UpdateAsync(UserModel);
            if (result)
            {
                SuccessMessage = "User updated successfully.";
                await JsInteropService.CloseModalWindow();
            }
            else
            {
                ErrorMessage = "Error occurred while updating user.";
            }
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }
}

public class UserModel
{
    public string? Email { get; set; }
    public DateTime LastAccess { get; set; }
    public int TotalUnlocks { get; set; }
    public string? Fullname { get; set; }
    public bool FaceUnlock { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}