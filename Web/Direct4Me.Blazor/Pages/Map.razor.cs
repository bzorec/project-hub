using Direct4Me.Blazor.Services;
using Direct4Me.Core.Handler;
using Direct4Me.Core.Runner;
using Direct4Me.Core.TravellingSalesmanProblem;
using Direct4Me.Repository.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using IRouteHandler = Direct4Me.Core.Handler.IRouteHandler;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject] private IJavaRunner JavaRunner { get; set; } = null!;
    [Inject] private IJsInteropService JsInteropService { get; set; } = null!;

    [Inject] private IRouteHandler RouteHandler { get; set; } = null!;

    private List<EstimetDelivery> EstimetDelivery { get; set; } = new List<EstimetDelivery>
    {
        new EstimetDelivery(),
        new EstimetDelivery()
    };

    private OptimizedPackagesInfo OptimizedPackagesInfo { get; set; } =
        new OptimizedPackagesInfo(new List<PackageEntity>(), new List<PackageEntity>())
        {
            PackageId = 1
        };

    // Method to open Estimated Delivery Modal
    private async Task OpenEstimatedDeliveryModal()
    {
        await JsInteropService.OpenEstimatedDeliveryModal();
    }

    // Method to open Optimized Packages Modal
    private async Task CloseOptimizedPackagesModal()
    {
        await JsInteropService.CloseOptimizedPackagesModal();
    }
    
    private async Task CloseEstimatedDeliveryModal()
    {
        await JsInteropService.CloseEstimatedDeliveryModal();
    }

    // Method to open Optimized Packages Modal
    private async Task OpenOptimizedPackagesModal()
    {
        await JsInteropService.OpenOptimizedPackagesModal();
    }

    protected async Task JavaRunnerExample()
    {
        var route = RouteHandler.GenerateMockRoute();

        var packages = RouteHandler.GeneratePackagesForRoute(route);
        var optimizedPackages = RouteHandler.OptimizePackages(packages, route);

        OptimizedPackagesInfo = new OptimizedPackagesInfo(packages, optimizedPackages);

        await LeafletMapService.InitBestRealTimePathMap(9);
        var aiOptimizedTour = await LeafletMapService.InitBestRealOptionsPathAiVersionMap(9, true);

        EstimetDelivery = RouteHandler.GetEstimetDelivery(aiOptimizedTour);
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

internal class OptimizedPackagesInfo
{
    public OptimizedPackagesInfo(List<PackageEntity> packages, List<PackageEntity> optimizedPackages)
    {
    }

    public object PackageId { get; set; }
}