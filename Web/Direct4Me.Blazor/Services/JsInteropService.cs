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
    Task<string> CaptureImageFromCamera();
    Task<bool> AuthenticateImage(byte[] byteArray);
    Task<byte[]> Base64ToByteArray(string base64String);
    Task<byte[]> FaceUnlockEnable(string userModelId);
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

    public async Task<string> CaptureImageFromCamera()
    {
        return await _jsRuntime.InvokeAsync<string>("jsInterop.captureImageFromCamera");
    }

    public async Task<bool> AuthenticateImage(byte[] byteArray)
    {
        return await _jsRuntime.InvokeAsync<bool>("jsInterop.authenticateImage", byteArray);
    }

    public async Task<byte[]> Base64ToByteArray(string base64String)
    {
        return await _jsRuntime.InvokeAsync<byte[]>("jsInterop.base64ToByteArray", base64String);
    }

    public ValueTask OpenModalWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.openModal");
    }

    public ValueTask CloseModalWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.closeModal");
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

    public async Task<byte[]> FaceUnlockEnable(string userModelId)
    {
        return await _jsRuntime.InvokeAsync<byte[]>("faceUnlock.enableFaceUnlock", userModelId);
    }

    public ValueTask CloseModalHistoryWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.closeHistoryModal");
    }

    public ValueTask OpenModalHistoryWindow()
    {
        return _jsRuntime.InvokeVoidAsync("jsInterop.openHistoryModal");
    }
}