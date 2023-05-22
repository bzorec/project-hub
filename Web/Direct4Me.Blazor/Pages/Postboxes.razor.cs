using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Postboxes
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private IPostboxService PostboxService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    public List<PostboxEntity> PostboxEntities { get; set; } = new();

    public string? ErrorMessage { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            var isAuth = await JsInteropService.IsUserAuthenticated();
            if (!isAuth) NavigationManager.NavigateTo("/login", true, true);
        }
        catch (Exception)
        {
            NavigationManager.NavigateTo("/login");
        }

        var email = await JsInteropService.GetUserEmail();

        if (email == null) return;

        var user = await UserService.GetUserByEmailAsync(email);

        if (firstRender)
        {
            var boxIds = await PostboxService.GetPostboxIdsForUser(user?.Id ?? throw new InvalidOperationException());

            if (!boxIds.Any()) return;

            foreach (var id in boxIds) PostboxEntities.Add(await PostboxService.GetPostboxByIdAsync(id));
        }

        StateHasChanged();
    }

    private async Task CallApiOnClick(int boxId)
    {
        // Create the payload
        var payload = new
        {
            deliveryId = 0,
            boxId = boxId,
            tokenFormat = 5,
            isMultibox = false,
            addAccessLog = true
        };

        // Convert the payload to JSON
        var payloadJson = JsonSerializer.Serialize(payload);

        // Create the HTTP content with the JSON payload
        var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

        // Add the Bearer token to the request headers
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "<your-token>");

        // Make the POST request to the API
        HttpResponseMessage response =
            await HttpClient.PostAsync("https://api-d4me-stage.direct4.me/sandbox/v1/Access/openbox", content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            ErrorMessage = $"Error occured while unlocking postbox[{boxId}]: {response.ReasonPhrase}";
            return;
        }

        var json = await response.Content.ReadFromJsonAsync<Direct4MeResponseModel>();

        if (json == null)
        {
            ErrorMessage = $"Error occured while unlocking postbox[{boxId}]";
            return;
        }

        // Check if the API call was successful
        if (!response.IsSuccessStatusCode || json.Result != 0)
        {
            ErrorMessage = $"{json.ErrorNumber} {json.ValidationErrors}";
            return;
        }

        byte[] mp3Bytes = Convert.FromBase64String(json.Data);

        // Call the JavaScript function to play the audio and close the animation
        await JsInteropService.PlayMp3FromResponse(mp3Bytes);
    }
}

[Serializable]
internal class Direct4MeResponseModel
{
    public int Result { get; set; }

    public string Message { get; set; }

    public List<string>? ValidationErrors { get; set; }

    public string ErrorNumber { get; set; }

    public string Data { get; set; }
}