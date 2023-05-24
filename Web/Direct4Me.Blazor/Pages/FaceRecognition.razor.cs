namespace Direct4Me.Blazor.Pages;

public partial class FaceRecognition
{
    private const string GuideContent =
        "This is the face unlock page. Use your facial recognition to securely log in to your account.";

    private const string GuideTitle = "Face Unlock Guide";

    private bool ShowPopupGuide { get; set; } = true;

    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         _video = await JSRuntime.InvokeAsync<IJSObjectReference>("eval", "document.getElementById('camera-stream');");
    //         _button = await JSRuntime.InvokeAsync<IJSObjectReference>("eval", "document.getElementById('take-picture');");
    //
    //         await JSRuntime.InvokeVoidAsync("navigator.mediaDevices.getUserMedia", new {video = true});
    //         await _video.InvokeVoidAsync("play");
    //
    //         await _button.InvokeVoidAsync("addEventListener", "click", DotNetObjectReference.Create(this));
    //     }
    // }
    //
    // [JSInvokable]
    // public async Task TakePicture()
    // {
    //     var canvas = await JSRuntime.InvokeAsync<IJSObjectReference>("eval", new[] {"document.createElement('canvas');"});
    //     await canvas.InvokeVoidAsync("setAttribute", "width", await _video.InvokeAsync<int>("getVideoWidth"));
    //     await canvas.InvokeVoidAsync("setAttribute", "height", await _video.InvokeAsync<int>("getVideoHeight"));
    //
    //     var ctx = await canvas.InvokeAsync<IJSObjectReference>("getContext", "2d");
    //     await ctx.InvokeVoidAsync("drawImage", _video, 0, 0, await _video.InvokeAsync<int>("getVideoWidth"), await _video.InvokeAsync<int>("getVideoHeight"));
    //
    //     var dataURL = await canvas.InvokeAsync<string>("toDataURL");
    //
    //     await JSRuntime.InvokeAsync<void>("localStorage.setItem", "faceRecognitionData", dataURL);
    //
    //     await JSRuntime.InvokeAsync<void>("stream.getTracks().forEach", new[] {DotNetObjectReference.Create(this)});
    //     await Navigation.NavigateTo("/login");
    // }
    public string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }


    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }
}