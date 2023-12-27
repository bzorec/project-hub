using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsGoogleMapsService GoogleMapsService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            double latitude = 40.7128;
            double longitude = -74.0060;
            int zoomLevel = 8;

            await GoogleMapsService.Init(latitude, longitude, zoomLevel);
        }
    }
}