using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class FaceRecognition
{
    private const string GuideContent =
        "This is the face unlock page. Use your facial recognition to securely log in to your account.";

    private const string GuideTitle = "Face Unlock Guide";

    private bool ShowPopupGuide { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }

    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private async Task Authenticate()
    {
        try
        {
            // Capture image from camera
            var imageUrl = await JsInteropService.CaptureImageFromCamera();

            // Convert the image URL to a byte array
            var byteArray = await JsInteropService.Base64ToByteArray(imageUrl);

            // Send the byte array to the API endpoint for authentication
            var result = await JsInteropService.AuthenticateImage(byteArray);

            if (result)
                // Authentication successful
                NavigationManager.NavigateTo("/dashboard", true, true);
            else
                // Authentication failed
                ErrorMessage = "Authentication failed. Please try again.";
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }
}