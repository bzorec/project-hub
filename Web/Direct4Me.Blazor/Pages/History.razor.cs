using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class History
{
    private const string GuideContent =
        "This is the history page. Here you can see history of postbox unlocks.";

    private const string GuideTitle = "Postbox History";
    [Inject] private IHistoryService HistoryService { get; set; }
    private List<PostboxHistoryEntity> UnlockHistory { get; set; } = new();
    public string? ErrorMessage { get; set; }
    private bool ShowPopupGuide { get; set; } = true;

    [Inject] public IJsInteropService JsInteropService { get; set; }
    [Inject] public IUserService UserService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) UnlockHistory = await GetUnlockHistory();

        StateHasChanged();
    }

    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

    private async Task<List<PostboxHistoryEntity>> GetUnlockHistory()
    {
        var email = await JsInteropService.GetUserEmail();
        var user = await UserService.GetUserByEmailAsync(email ?? string.Empty);
        return await HistoryService.GetFullHistoryAsync(user?.Id ?? throw new InvalidOperationException());
    }
}