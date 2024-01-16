using Direct4Me.Blazor.Services;
using Direct4Me.Core.TravellingSalesmanProblem;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;

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
        for (var i = 0; i < 100; i++)
        {
            await LeafletMapService.ShowSpinner();
            theBestPath = await LeafletMapService.InitBestFakePathMap(9, dataPath, theBestPath);
            await LeafletMapService.HideSpinner();
        }
    }
}