using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Services;

public interface IJsGoogleMapsService
{
    Task Init(double latitude, double longitude, int zoomLevel);
}

public class JsGoogleMapsService : IJsGoogleMapsService
{
    private readonly IJSRuntime _jsRuntime;

    public JsGoogleMapsService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task Init(double latitude, double longitude, int zoomLevel)
    {
        await _jsRuntime.InvokeVoidAsync("jsInterop.initGoogleMap");
    }
}