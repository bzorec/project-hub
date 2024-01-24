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

    private List<EstimetDelivery> EstimetDelivery { get; set; } = new()
    {
        new EstimetDelivery(),
        new EstimetDelivery()
    };

    private OptimizedPackagesInfo OptimizedPackagesInfo { get; set; }

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

    private async Task JavaRunnerExample()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dataPath = Path.Combine(basePath, "Data", "packages.json");
        var dataPath3 = Path.Combine(basePath, "Data", "delivery.json");

        var route = RouteHandler.GenerateMockRoute();

        var packages = RouteHandler.GeneratePackagesForRoute(route, basePath, dataPath);
        List<PackageEntity> optimizedPackages;
        try
        {
            optimizedPackages = RouteHandler.OptimizePackages(JavaRunner, packages, basePath, dataPath);
        }
        catch (Exception e)
        {
            optimizedPackages = MockOptimizedPackages(packages.Count);
        }

        // Update the route with postboxes that have optimized packages
        // UpdateRouteWithOptimizedPackages(route, optimizedPackages);

        OptimizedPackagesInfo = new OptimizedPackagesInfo(packages, optimizedPackages);

        var aiOptimizedTour = await LeafletMapService.InitBestRealOptionsPathAiVersionMap(route, 9);
        try
        {
            EstimetDelivery = RouteHandler.GetEstimetDelivery(JavaRunner, aiOptimizedTour, dataPath3);
        }
        catch (Exception e)
        {
            EstimetDelivery = MockEstimetDelivery(route.Postboxes.Count);
        }
    }

    private List<PackageEntity> MockOptimizedPackages(int packageCount)
    {
        var rand = new Random();
        var mockPackages = new List<PackageEntity>();

        for (int i = 0; i < packageCount; i++)
        {
            mockPackages.Add(new PackageEntity
            {
                Id = Guid.NewGuid().ToString(),
                PackageId = rand.Next(1000, 9999),
                PostBoxId = rand.Next(1, 11),
                // ... other properties ...
            });
        }

        return mockPackages;
    }

    private List<EstimetDelivery> MockEstimetDelivery(int postboxCount)
    {
        var rand = new Random();
        var mockDeliveries = new List<EstimetDelivery>();

        for (int i = 1; i <= postboxCount; i++)
        {
            mockDeliveries.Add(new EstimetDelivery
            {
                PostBoxId = i,
                EstimatedDeliveryTime = rand.Next(1, 24) // Random hour of the day
            });
        }

        return mockDeliveries;
    }

    private void UpdateRouteWithOptimizedPackages(RouteEntity route, List<PackageEntity> optimizedPackages)
    {
        var remainingPostBoxIds = new HashSet<int>(optimizedPackages.Select(p => p.PostBoxId));
        route.Postboxes = route.Postboxes.Where(postbox => remainingPostBoxIds.Contains(postbox.PostBoxId)).ToList();
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
    public List<PackageSummary> PackageSummaries { get; private set; } = new List<PackageSummary>();

    public OptimizedPackagesInfo(List<PackageEntity> packages, List<PackageEntity> optimizedPackages)
    {
        var optimizedPackageIds = new HashSet<string>(optimizedPackages.Select(p => p.Id));

        foreach (var package in packages)
        {
            var summary = new PackageSummary
            {
                PackageId = package.PackageId,
                PostBoxId = package.PostBoxId,
                Status = optimizedPackageIds.Contains(package.Id) ? "Retained" : "Removed",
                Summary = optimizedPackageIds.Contains(package.Id)
                    ? $"Package {package.PackageId} in PostBox {package.PostBoxId} was retained."
                    : $"Package {package.PackageId} in PostBox {package.PostBoxId} was removed."
            };

            PackageSummaries.Add(summary);
        }
    }
}

internal class PackageSummary
{
    public int PackageId { get; set; }
    public int PostBoxId { get; set; }
    public string Status { get; set; } // "Retained" or "Removed"
    public string Summary { get; set; }
}