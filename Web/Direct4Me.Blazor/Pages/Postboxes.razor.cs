using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using static System.Convert;

namespace Direct4Me.Blazor.Pages;

public partial class Postboxes
{
    private const string GuideContent =
        "This is the postboxes page. Here you can access and manage your postboxes. Blue cards are owners postboxes. Red ones where given access from other user.";

    private const string GuideTitle = "Postboxes Guide";
    private bool ShowPopupGuide { get; set; } = true;
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private IPostboxService PostboxService { get; set; }
    [Inject] private IHistoryService HistoryService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    public List<PostboxEntity> PostboxEntities { get; set; } = new();
    public List<PostboxEntity> PostboxOtherEntities { get; set; } = new();

    public string? ErrorMessage { get; set; }

    private List<UserEntity> UserList { get; set; } = new();
    private string SelectedUser { get; set; }
    public string SelectedBoxId { get; set; } = string.Empty;

    private List<(string, string)> UserStringList { get; set; } = new();


    private bool IsEmpty { get; set; }
    public string? SuccessMessage { get; set; }

    protected override void OnInitialized()
    {
        UserList = new List<UserEntity>();
    }

    private void HandleClosePopup(bool value)
    {
        ShowPopupGuide = false;
    }

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

            foreach (var id in boxIds) PostboxEntities.Add(await PostboxService.GetPostboxByIdAsync(id));

            boxIds = await PostboxService.GetOtherPostboxIdsForUser(user?.Id ?? throw new InvalidOperationException());
            StateHasChanged();

            if (!boxIds.Any()) return;

            foreach (var id in boxIds) PostboxOtherEntities.Add(await PostboxService.GetPostboxByIdAsync(id));
        }

        StateHasChanged();
    }

    private async Task CallApiOnClick(int boxId)
    {
        var historyLog = new PostboxHistoryEntity
        {
            Date = DateTime.Now,
            UserName = await JsInteropService.GetUserName() ?? "",
            PostboxId = boxId.ToString(),
            Success = false,
            CreatedOn = DateTime.Now,
            ModifiedOn = null,
            Type = "QR Code"
        };

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

            historyLog.Success = false;
            await HistoryService.LogHistoryAsync(historyLog);

            return;
        }

        var json = await response.Content.ReadFromJsonAsync<Direct4MeResponseModel>();

        if (json == null)
        {
            ErrorMessage = $"Error occured while unlocking postbox[{boxId}]";
            historyLog.Success = false;
            await HistoryService.LogHistoryAsync(historyLog);
            return;
        }

        // Check if the API call was successful
        if (!response.IsSuccessStatusCode || json.Result != 0)
        {
            ErrorMessage = $"{json.ErrorNumber} {json.ValidationErrors}";
            historyLog.Success = false;
            await HistoryService.LogHistoryAsync(historyLog);
            return;
        }

        byte[] mp3Bytes = FromBase64String(json.Data);

        // Call the JavaScript function to play the audio and close the animation
        await JsInteropService.PlayMp3FromResponse(mp3Bytes);

        historyLog.Success = true;
        await HistoryService.LogHistoryAsync(historyLog);
    }

    private async Task OpenGrantAccessModal(int postboxPostBoxId)
    {
        SelectedBoxId = postboxPostBoxId.ToString();

        UserList = await UserService.GetAllAsync(null, null, null);

        IsEmpty = UserList.Count == 0;
        UserStringList = UserList.Select(i => (Value: i.Id, Title: $"{i.Fullname} - {i.Email}")).ToList();

        StateHasChanged();

        try
        {
            await JsInteropService.OpenModalHistoryWindow();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task GrantAccess(string seletedBoxId)
    {
        try
        {
            var entity = PostboxEntities.FirstOrDefault(i => i.PostBoxId == ToInt32(SelectedBoxId));
            var user = await UserService.GetUserByIdAsync(SelectedUser);

            entity.AccessList ??= new List<string>();

            if (entity?.UserId == user.Id || entity.AccessList.Contains(user.Id))
            {
                await JsInteropService.CloseModalHistoryWindow();
                ErrorMessage = "You already have access.";

                return;
            }

            if (entity != null)
            {
                entity.AccessList?.Add(SelectedUser);
                await PostboxService.UpdateAsync(entity);
            }

            await JsInteropService.CloseModalHistoryWindow();

            SuccessMessage = "Access given.";
        }
        catch (Exception e)
        {
            ErrorMessage = $"{e.Message}";
        }
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