using Direct4Me.Core.Runner;
using Direct4Me.Core.TravellingSalesmanProblem;
using Direct4Me.Repository.Entities;
using Microsoft.JSInterop;
using IRouteHandler = Direct4Me.Core.Handler.IRouteHandler;

namespace Direct4Me.Blazor.Services;

public interface IJsLeafletMapService
{
    Task InitMap(double latitude, double longitude, int zoomLevel);
    Task AddMarker(double latitude, double longitude);
    Task DrawPath(List<(double Latitude, double Longitude)> pathPoints);
    Task ShowSpinner();
    Task HideSpinner();

    Task<Tour?> InitBestFakePathMap(int zoomLevel, string dataPath, Tour? theBestPath);
    Task InitBestRealDisctancePathMap(int zoomLevel);

    Task InitBestRealOptionsPathMap(int zoomLevel);
    Task InitBestRealTimePathMap(int zoomLevel);

    Task<Tour?> InitBestRealOptionsPathAiVersionMap(IRouteHandler handler, IJavaRunner runner,
        RouteEntity route,
        string basePath, string dataPath, int zoomLevel);

    Task<Tour?> InitBestRealOptionsPathMap(RouteEntity route, int zoomLevel);
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

    public async Task InitBestRealOptionsPathMap(int zoomLevel)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var bestBest = new Tour(0);

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 1000, true);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            if (bestBest.Distance > bestPath?.Distance)
            {
                bestBest = new Tour(bestPath);
            }
        }

        var cities = bestBest.Path.Skip(10).Take(10);
        bestBest.Path = cities.ToList();

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestBest?.ToJavascriptObject(), zoomLevel);
    }

    public async Task<Tour?> InitBestRealOptionsPathMap(RouteEntity route, int zoomLevel)
    {
        var bestBest = new Tour(0);

        route.Postboxes.RemoveAt(4);
        route.Postboxes.RemoveAt(6);

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(route, route.DistanceMatrix, 1000);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            if (bestBest.Distance > bestPath?.Distance)
            {
                bestBest = new Tour(bestPath);
            }
        }

        var cities = bestBest.Path.Skip(10).Take(10);
        bestBest.Path = cities.ToList();

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestBest?.ToJavascriptObject(), zoomLevel);

        return bestBest;
    }

    public async Task<Tour?> InitBestRealOptionsPathAiVersionMap(IRouteHandler handler, IJavaRunner runner,
        RouteEntity route,
        string basePath, string dataPath, int zoomLevel)
    {
        // TODO:bzorec generate json
        handler.SaveToJson(route.Postboxes.Select(i => i.PostBoxId).ToList(), basePath, dataPath, "path.json");

        // TODO:bzorec javaRunner
        await runner.RunJarAsync(dataPath);

        // TODO:bzorec get output json
        // TODO:bzorec remove boxes with ids number less the 1
        var bestBest = new Tour(0);

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(route, route.DistanceMatrix, 1000);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            if (bestBest.Distance > bestPath?.Distance)
            {
                bestBest = new Tour(bestPath);
            }
        }

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestBest?.ToJavascriptObject(), zoomLevel);

        return bestBest;
    }


    public async Task InitBestRealDisctancePathMap(int zoomLevel)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var bestBest = new Tour(0);

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 1000, true);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            if (bestBest.Distance > bestPath?.Distance)
            {
                bestBest = new Tour(bestPath);
            }
        }

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestBest?.ToJavascriptObject(), zoomLevel);
    }

    public async Task InitBestRealTimePathMap(int zoomLevel)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "complete_data.json");

        var bestBest = new Tour(0);

        for (var i = 0; i < 30; i++)
        {
            var eilTsp = new TspAlgorithm(dataPath, 1000, true, true);
            var ga = new GeneticAlgorithm(100, 0.8, 0.8);
            var bestPath = ga.Execute(eilTsp);

            if (bestBest.Distance > bestPath?.Distance)
            {
                bestBest = new Tour(bestPath);
            }
        }

        await _jsRuntime.InvokeVoidAsync("jsInterop.initBestPathMap", bestBest?.ToJavascriptObject(), zoomLevel);
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