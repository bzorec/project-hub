using Direct4Me.Repository.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor;

public partial class App : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [Inject] private IUserService UserService { get; set; } = null!;

    public bool IsLoggedIn { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        RedirectToLogin();
    }

    private void RedirectToLogin()
    {
        Navigation.NavigateTo(!IsLoggedIn ? "/login" : "/dashboard");
    }
}