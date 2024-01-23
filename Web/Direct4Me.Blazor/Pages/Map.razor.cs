using System.Reflection;
using Direct4Me.Blazor.Services;
using Direct4Me.Core.Runner;
using Direct4Me.Core.TravellingSalesmanProblem;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject] private IJavaRunner JavaRunner { get; set; } = null!;

    protected async Task JavaRunnerExample()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var arffPath = Path.Combine(assemblyPath ?? throw new InvalidOperationException(), "Data", "test.arff");

        string args = $"{arffPath} NaiveBayesUpdatable 50";

        var result = await JavaRunner.RunJarAsync(args);

        Console.WriteLine(result);
    }

    protected async Task CalculateOptimalRouteRealOptions()
    {
        await LeafletMapService.ShowSpinner();
        await LeafletMapService.InitBestRealOptionsPathMap(9);
        await LeafletMapService.HideSpinner();
    }

    protected async Task CalculateOptimalRouteRealDistance()
    {
        await LeafletMapService.ShowSpinner();
        await LeafletMapService.InitBestRealDisctancePathMap(9);
        await LeafletMapService.HideSpinner();
    }

    protected async Task CalculateOptimalRouteRealTime()
    {
        await LeafletMapService.ShowSpinner();
        await LeafletMapService.InitBestRealTimePathMap(9);
        await LeafletMapService.HideSpinner();
    }

    protected async Task CalculateOptimalRouteFake()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "eil101.tsp");

        Tour? theBestPath = null;
        var zoomLevel = 9;

        for (var i = 0; i < 30; i++)
        {
            await LeafletMapService.ShowSpinner();
            theBestPath = new Tour(await LeafletMapService.InitBestFakePathMap(zoomLevel, dataPath, theBestPath));
            await LeafletMapService.HideSpinner();
        }

        await JsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", theBestPath?.ToJavascriptObject(), zoomLevel);
    }
}