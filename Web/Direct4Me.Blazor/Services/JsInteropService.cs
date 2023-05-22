using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Services;

public class JsInteropService : IJsInteropService
{
    private readonly IJSRuntime _jsRuntime;

    public JsInteropService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask<string> GetToken()
    {
        return _jsRuntime.InvokeAsync<string>("jsInterop.getToken");
    }

    public ValueTask<bool> IsUserAuthenticated()
    {
        return _jsRuntime.InvokeAsync<bool>("jsInterop.isAuthenticated");
    }

    public ValueTask<string?> GetUserName()
    {
        return _jsRuntime.InvokeAsync<string?>("jsInterop.getUsername");
    }

    public ValueTask<string?> GetUserEmail()
    {
        return _jsRuntime.InvokeAsync<string?>("jsInterop.getEmail");
    }

    public ValueTask PlayMp3FromResponse(byte[] mp3)
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.playAndCloseAnimation", mp3);
    }

    public ValueTask Logout()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.removeToken");
    }

    public ValueTask SetToken(string token)
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.setToken", token);
    }
}

public interface IJsInteropService
{
    ValueTask SetToken(string token);
    ValueTask<string> GetToken();
    ValueTask Logout();
    ValueTask<bool> IsUserAuthenticated();
    ValueTask<string?> GetUserName();
    ValueTask<string?> GetUserEmail();
    ValueTask PlayMp3FromResponse(byte[] mp3);
}