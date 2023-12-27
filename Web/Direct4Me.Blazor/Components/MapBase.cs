using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Components;

public class MapBase : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    protected bool IsPostman { get; set; } = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("initMap");
            await LoadPostboxLocations();
        }
    }

    protected Task LoadPostboxLocations()
    {
        return Task.CompletedTask;
    }

    protected Task CalculateOptimalRoute()
    {
        return Task.CompletedTask;
    }
}