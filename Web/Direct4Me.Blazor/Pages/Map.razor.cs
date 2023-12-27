using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LeafletMapService.ShowSpinner();

            // Initialize map
            await LeafletMapService.InitMap(46.1512, 14.9955, 9);

            // // Add markers
            // await LeafletMapService.AddMarker(51.505, -0.09);
            // await LeafletMapService.AddMarker(51.515, -0.1);
            //
            // // Draw path
            // var pathPoints = new List<(double Latitude, double Longitude)>
            // {
            //     (51.505, -0.09),
            //     (51.515, -0.1)
            // };
            // await LeafletMapService.DrawPath(pathPoints);

            await LeafletMapService.HideSpinner();
        }
    }
}