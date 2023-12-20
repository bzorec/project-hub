using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class History
{
    private const string GuideContent = "This is the history page. Here you can see history of postbox unlocks.";
    private const string GuideTitle = "Postbox History";

    [Inject] private IHistoryService? HistoryService { get; set; }
    [Inject] private IJsInteropService? JsInteropService { get; set; }
    [Inject] private IUserService? UserService { get; set; }

    private List<PostboxHistoryEntity> UnlockHistory { get; set; } = new List<PostboxHistoryEntity>();
    private bool ShowPopupGuide { get; set; } = true;
    private string? ErrorMessage { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await LoadUnlockHistory();
    }

    private async Task LoadUnlockHistory()
    {
        try
        {
            var email = await JsInteropService.GetUserEmail();
            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage = "Email is not available.";
                return;
            }

            var user = await UserService.GetUserByEmailAsync(email);
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return;
            }

            UnlockHistory = await HistoryService.GetFullHistoryAsync(user.Id);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error retrieving history: {ex.Message}";
        }
    }

    private void HandleClosePopup() => ShowPopupGuide = false;

    private static string GetStatusText(bool success) => success ? "Success" : "Error";

    private static string GetStatusClass(bool success) => success ? "text-success" : "text-danger";
}