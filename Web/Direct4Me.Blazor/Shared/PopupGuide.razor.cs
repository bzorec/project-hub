using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Shared;

public partial class PopupGuide
{
    private bool _skipGuide;
    [Parameter] public string Title { get; set; } = null!;

    [Parameter] public string Content { get; set; } = null!;

    [Parameter] public bool IsPopupVisible { get; set; }

    [Parameter] public ElementReference TargetElement { get; set; }

    [Parameter] public EventCallback<bool> SkipGuideChanged { get; set; }

    [Parameter] public EventCallback<bool> ClosePopupRequested { get; set; }

    public bool SkipGuide
    {
        get => _skipGuide;
        set
        {
            _skipGuide = value;
            SkipGuideChanged.InvokeAsync(value);
        }
    }

    private Task ClosePopup()
    {
        return ClosePopupRequested.InvokeAsync(true);
    }
}