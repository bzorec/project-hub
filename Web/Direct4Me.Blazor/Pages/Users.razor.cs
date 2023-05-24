using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Users
{
    private const string GuideContent =
        "This is the users page. As an admin, you can view and manage user accounts and permissions.";

    private const string GuideTitle = "Users Guide";
    private bool ShowPopupGuide { get; set; } = true;

    private UserEntity UserModel { get; set; } = new();

    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IPostboxService PostboxService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public string? UserName { get; set; }
    public List<UserEntity>? UserList { get; set; }


    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

    protected override void OnInitialized()
    {
        UserList = new List<UserEntity>();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        UserName = await JsInteropService.GetUserName();
        UserList = await UserService.GetAllAsync(null, null, null);
    }

    private async Task DeleteUser(string userId)
    {
        try
        {
            var result = await UserService.DeleteAsync(userId);
            if (result) SuccessMessage = "User deleted successfully.";
            else ErrorMessage = "Error occured while deleting user.";
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    private async Task OpenUpdateModal(string userId)
    {
        UserModel = GetUserById(userId);
        try
        {
            await JsInteropService.OpenModalWindow();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private UserEntity GetUserById(string userId)
    {
        return (UserList ?? new List<UserEntity>()).FirstOrDefault(user => user.Id == userId);
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