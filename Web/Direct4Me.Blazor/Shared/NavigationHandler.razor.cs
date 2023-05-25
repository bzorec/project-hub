using Direct4Me.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Shared;

public partial class NavigationHandler
{
    [Parameter] public NavigationType NavigationType { get; set; }
}