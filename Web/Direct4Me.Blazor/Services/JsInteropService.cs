using Microsoft.JSInterop;

namespace Direct4Me.Blazor.Services;

public interface IJsInteropService
{
    ValueTask SetToken(string token);
    ValueTask OpenModalWindow();
    ValueTask<string> GetToken();
    ValueTask Logout();
    ValueTask<bool> IsUserAuthenticated();
    ValueTask<string?> GetUserName();
    ValueTask<string?> GetUserEmail();
    ValueTask PlayMp3FromResponse(byte[] mp3);
    ValueTask CloseModalWindow();
    Task<byte[]> FaceUnlockEnable();
    ValueTask CloseModalHistoryWindow();
    ValueTask OpenModalHistoryWindow();
}

public class JsInteropService : IJsInteropService
{
    private readonly IJSRuntime _jsRuntime;

    public JsInteropService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask OpenModalWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.openModal");
    }

    public ValueTask CloseModalWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.closeModal");
    }

    public async Task<byte[]> FaceUnlockEnable()
    {
        return await _jsRuntime.InvokeAsync<byte[]>("faceUnlock.enableFaceUnlock");
    }

    public ValueTask CloseModalHistoryWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.closeHistoryModal");
    }

    public ValueTask OpenModalHistoryWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.openHistoryModal");
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