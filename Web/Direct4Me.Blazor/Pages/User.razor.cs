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
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IJwtService JwtService { get; set; } = null!;
    [Inject] private IJsInteropService JsInteropService { get; set; } = null!;
    public string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }

    private bool ShowPopupGuide { get; set; } = true;

    public UserEntity UserModel { get; set; } = new();

    private async Task EnableFaceUnlock()
    {
        try
        {
            await JsInteropService.FaceUnlockEnable(UserModel.Id);
            UserModel.IsFaceUnlock = true;
            await UserService.UpdateAsync(UserModel);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var email = await JsInteropService.GetUserEmail() ?? throw new InvalidOperationException();
            if (email != null) UserModel = await UserService.GetUserByEmailAsync(email) ?? new UserEntity();
        }

        StateHasChanged();
    }

    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
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

    public void NavigateTo()
    {
        NavigationManager.NavigateTo("/facerecognition");
    }
}