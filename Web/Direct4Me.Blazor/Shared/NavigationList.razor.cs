using Direct4Me.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Shared;

public partial class NavigationList
{
    [Parameter] public List<NavListItem> Links { get; set; } = null!;

    [Parameter] public string CssClasses { get; set; } = null!;
}