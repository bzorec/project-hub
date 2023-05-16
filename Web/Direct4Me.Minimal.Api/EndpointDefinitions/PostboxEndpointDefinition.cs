using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Services;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class PostboxEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/postboxes/{userId}", GetAllPostboxesAsync);
        app.MapPut("/postboxes/add/boxId/{postBoxId}/userId/{userId}/", AddPostBoxAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<IResult> AddPostBoxAsync(IPostboxService service, string postBoxId, string userId)
    {
        try
        {
            await service.AddPostBoxAsync(postBoxId, userId);
            return Results.Ok("Postbox added successfully");
        }
        catch (Exception e)
        {
            return Results.BadRequest($"Error occured while adding postbox: {e.Message}");
        }
    }

    private static async Task<List<Postbox>> GetAllPostboxesAsync(IPostboxService service, string userId)
    {
        var postboxes = await service.GetPostboxesByUserIdAsync(userId);

        return postboxes.Select(box => new Postbox(box.Id, box.PostBoxId, box.StatisticsEntity.TotalUnlocks,
            box.StatisticsEntity.LastModified)).ToList();
    }
}