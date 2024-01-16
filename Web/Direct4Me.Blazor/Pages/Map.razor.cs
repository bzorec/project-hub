using Direct4Me.Blazor.Services;
using Direct4Me.Core.TravellingSalesmanProblem;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    protected async Task CalculateOptimalRouteReal()
    {
        await LeafletMapService.ShowSpinner();
        await LeafletMapService.InitBestRealPathMap(9);
        await LeafletMapService.HideSpinner();
    }

    protected async Task CalculateOptimalRouteFake()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "eil101.tsp");

        Tour? theBestPath = null;
        var zoomLevel = 9;

        for (var i = 0; i < 100; i++)
        {
            await LeafletMapService.ShowSpinner();
            theBestPath = await LeafletMapService.InitBestFakePathMap(zoomLevel, dataPath, theBestPath);
            await LeafletMapService.HideSpinner();
        }

        await JsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", theBestPath?.ToJavascriptObject(), zoomLevel);
    }
}