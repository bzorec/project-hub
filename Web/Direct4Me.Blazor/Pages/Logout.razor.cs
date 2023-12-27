using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Logout
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IJsInteropService JsInteropService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JsInteropService.Logout();
        NavigationManager.NavigateTo("/login", true, true);
    }
}