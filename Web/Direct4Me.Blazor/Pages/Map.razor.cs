using Direct4Me.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Direct4Me.Blazor.Pages;

public partial class Map
{
    [Inject] private IJsLeafletMapService LeafletMapService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LeafletMapService.ShowSpinner();

            // Initialize map
            await LeafletMapService.InitBestPathMap(9);

            await LeafletMapService.HideSpinner();
        }
    }
}