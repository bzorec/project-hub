using Direct4Me.Blazor.Services;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Dashboard
{
    [Inject] private IJsInteropService JsInteropService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IPostboxService PostboxService { get; set; }
    [Inject] private IUserService UserService { get; set; }

    public string? ErrorMessage { get; set; }

    public string? UserName { get; set; }

    public int NfcUnlocksTotal { get; set; }
    public int NfcUnlocksToday { get; set; }
    public int NfcUnlocksWeek { get; set; }
    public int NfcUnlocksMonth { get; set; }

    public int QrUnlocksTotal { get; set; }
    public int QrUnlocksToday { get; set; }
    public int QrUnlocksWeek { get; set; }
    public int QrUnlocksMonth { get; set; }

    public int DefaultLoginTotal { get; set; }
    public int DefaultLoginDaily { get; set; }
    public int DefaultLoginWeekly { get; set; }
    public int DefaultLoginMonthly { get; set; }

    public int FaceLoginTotal { get; set; }
    public int FaceLoginDaily { get; set; }
    public int FaceLoginWeekly { get; set; }
    public int FaceLoginMonthly { get; set; }

    public List<PostboxEntity> PostboxEntities { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        //TODO:bzorec fix bug on state change the table add column this not godd
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

        if (user != null)
        {
            DefaultLoginTotal = user.StatisticsEntity.DefaultLoginCount;
            DefaultLoginDaily = user.StatisticsEntity.DailyStatistics.DefaultLoginCount;
            DefaultLoginWeekly = user.StatisticsEntity.WeeklyStatistics.DefaultLoginCount;
            DefaultLoginMonthly = user.StatisticsEntity.MonthlyStatistics.DefaultLoginCount;

            FaceLoginTotal = user.StatisticsEntity.FaceLoginCount;
            FaceLoginDaily = user.StatisticsEntity.DailyStatistics.FaceLoginCount;
            FaceLoginWeekly = user.StatisticsEntity.WeeklyStatistics.FaceLoginCount;
            FaceLoginMonthly = user.StatisticsEntity.MonthlyStatistics.FaceLoginCount;
            UserName = user.Fullname;
        }

        StateHasChanged();

        NfcUnlocksTotal = PostboxEntities.Sum(i => i.StatisticsEntity.NfcUnlock);
        NfcUnlocksToday = PostboxEntities.Sum(i => i.StatisticsEntity.DailyStatistics.NfcUnlock);
        NfcUnlocksWeek = PostboxEntities.Sum(i => i.StatisticsEntity.WeeklyStatistics.NfcUnlock);
        NfcUnlocksMonth = PostboxEntities.Sum(i => i.StatisticsEntity.MonthlyStatistics.NfcUnlock);

        QrUnlocksTotal = PostboxEntities.Sum(i => i.StatisticsEntity.QrCodeUnlock);
        QrUnlocksToday = PostboxEntities.Sum(i => i.StatisticsEntity.DailyStatistics.QrCodeUnlock);
        QrUnlocksWeek = PostboxEntities.Sum(i => i.StatisticsEntity.WeeklyStatistics.QrCodeUnlock);
        QrUnlocksMonth = PostboxEntities.Sum(i => i.StatisticsEntity.MonthlyStatistics.QrCodeUnlock);

        if (firstRender)
        {
            var boxIds = await PostboxService.GetPostboxIdsForUser(user?.Id ?? throw new InvalidOperationException());

            if (!boxIds.Any()) return;

            foreach (var id in boxIds) PostboxEntities.Add(await PostboxService.GetPostboxByIdAsync(id));
        }
    }
}