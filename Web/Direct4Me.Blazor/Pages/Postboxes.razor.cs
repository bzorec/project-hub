using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Postboxes
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private IPostboxService PostboxService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    public List<PostboxEntity> PostboxEntities { get; set; } = new();

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

        var email = await JsInteropService.GetUserEmail();

        if (email == null) return;

        var user = await UserService.GetUserByEmailAsync(email);

        if (firstRender)
        {
            var boxIds = await PostboxService.GetPostboxIdsForUser(user?.Id ?? throw new InvalidOperationException());

            if (!boxIds.Any()) return;

            foreach (var id in boxIds) PostboxEntities.Add(await PostboxService.GetPostboxByIdAsync(id));
        }

        StateHasChanged();
    }
}