using System.Security.Authentication;
using Direct4Me.Blazor.Models;
using Direct4Me.Blazor.Services;
using Direct4Me.Blazor.Utils;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Shared;

public partial class NavigationMenu : ComponentBase
{
    private List<NavListItem> _navigationListFaceSignIn = new()
    {
        new NavListItem {Name = "(Sign in)", Href = "/login"},
        new NavListItem {Name = "(Sign up)", Href = "/register"}
    };

    private List<NavListItem> _navigationListLogin = new()
    {
        new NavListItem {Name = "(Sign up)", Href = "/register"},
        new NavListItem {Name = "Sign in (Face recognition)", Href = "/facerecognition"}
    };

    private List<NavListItem> _navigationListMain =
        new()
        {
            new NavListItem {Name = "(Dashboard)", Href = "/dashboard"},
            new NavListItem {Name = "(Postboxes)", Href = "/postboxes"},
            new NavListItem {Name = "(History)", Href = "/history"}
        };

    private List<NavListItem> _navigationListRegister = new()
    {
        new NavListItem {Name = "(Sign in)", Href = "/login"},
        new NavListItem {Name = "Sign in (Face recognition)", Href = "/facerecognition"}
    };

    private List<NavListItem> _navigationListSignOut = new()
    {
        new() {Name = "Welcome", Href = "/user"},
        new() {Name = "(Sign out)", Href = "/logout"}
    };

    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private IJwtService JwtService { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private IUserService Service { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }

    [Parameter] public NavigationType NavigationType { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        Username = await JsInteropService.GetUserName();
        Email = await JsInteropService.GetUserEmail();

        if (NavigationType == NavigationType.Default && Username.IsNullOrEmpty())
        {
            await JsInteropService.Logout();

            throw new AuthenticationException();
        }

        _navigationListSignOut = new List<NavListItem>
        {
            new() {Name = $"Welcome, {Username}", Href = "/user"},
            new() {Name = "(Sign out)", Href = "/logout"}
        };
        StateHasChanged(); // Refresh the component's state

        if (firstRender)
            if ((Email ?? string.Empty).IsAdmin())
                _navigationListMain.Add(new NavListItem {Name = "(Users)", Href = "/users"});
    }
}