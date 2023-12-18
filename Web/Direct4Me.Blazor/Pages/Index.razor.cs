using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Index : ComponentBase
{
    [Inject] private IJsInteropService? JsInteropService { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            var isAuth = await JsInteropService.IsUserAuthenticated();
            if (!isAuth) NavigationManager.NavigateTo("/login", true, true);
        }
        catch (Exception)
        {
            NavigationManager.NavigateTo("/login");
        }
    }
}