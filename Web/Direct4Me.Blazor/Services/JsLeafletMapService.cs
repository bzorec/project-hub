using Direct4Me.Core.TravellingSalesmanProblem;
using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Services;

public interface IJsLeafletMapService
{
    Task InitMap(double latitude, double longitude, int zoomLevel);
    Task AddMarker(double latitude, double longitude);
    Task DrawPath(List<(double Latitude, double Longitude)> pathPoints);
    Task ShowSpinner();
    Task HideSpinner();

    Task<Tour?> InitBestFakePathMap(int zoomLevel, string dataPath, Tour? theBestPath);
    Task InitBestRealPathMap(int zoomLevel);
}

public class JsLeafletMapService : IJsLeafletMapService
{
    private readonly IJSRuntime _jsRuntime;

    public JsLeafletMapService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitMap(double latitude, double longitude, int zoomLevel)
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.initMap", latitude, longitude, zoomLevel);
    }

    public async Task<Tour?> InitBestFakePathMap(int zoomLevel, string dataPath, Tour? theBestPath)
    {
        var eilTsp = new TspAlgorithm(dataPath, 10000);
        var ga = new GeneticAlgorithm(100, 0.8, 0.1);
        var bestPath = ga.Execute(eilTsp);
        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestPath?.ToJavascriptObject(), zoomLevel);

        theBestPath ??= bestPath;
        if (bestPath?.Distance <= theBestPath?.Distance)
        {
            theBestPath = new Tour(bestPath);
        }

        return theBestPath;
    }

    public async Task InitBestRealPathMap(int zoomLevel)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "eil101.tsp");

        Tour? theBestPath = null;
        for (var i = 0; i < 100; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 50000);
            var ga = new GeneticAlgorithm(100, 0.8, 0.1);
            var bestPath = ga.Execute(eilTsp);

            theBestPath ??= bestPath;

            if (bestPath?.Distance <= theBestPath?.Distance)
            {
                theBestPath = new Tour(bestPath);
            }
        }

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", theBestPath?.ToJavascriptObject(), zoomLevel);
    }


    public async Task AddMarker(double latitude, double longitude)
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.addMarker", latitude, longitude);
    }

    public async Task DrawPath(List<(double Latitude, double Longitude)> pathPoints)
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.drawPath", pathPoints);
    }

    public async Task ShowSpinner()
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.showSpinner");
    }

    public async Task HideSpinner()
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.hideSpinner");
    }
}