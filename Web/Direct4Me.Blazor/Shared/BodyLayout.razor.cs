using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Shared;

public partial class BodyLayout
{
    [Parameter] public RenderFragment Body { get; set; } = null!;
}