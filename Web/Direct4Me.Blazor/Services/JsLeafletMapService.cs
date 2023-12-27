using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Services;

public interface IJsLeafletMapService
{
    Task InitMap(double latitude, double longitude, int zoomLevel);
    Task AddMarker(double latitude, double longitude);
    Task DrawPath(List<(double Latitude, double Longitude)> pathPoints);
    Task ShowSpinner();
    Task HideSpinner();
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