using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class PostboxEndpointDefinition : IEndpointDefinition
{
    private readonly Logger<PostboxEndpointDefinition> _logger;

    public PostboxEndpointDefinition()
    {
    }

    public PostboxEndpointDefinition(Logger<PostboxEndpointDefinition> logger)
    {
        _logger = logger;
    }

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/postboxes", GetAllAsync);
        app.MapPost("/postboxes", AddAsync);
        app.MapPut("/postboxes/{guid}/update", UpdateAsync);
        app.MapDelete("/postboxes/{guid}/delete", DeleteAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private async Task<IResult> UpdateAsync(IPostboxService service, string guid, Postbox model)
    {
        try
        {
            await service.UpdateAsync(guid);
            return Results.Ok("Deleting postbox succeeded.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while deleting postbox: {Message}", e.Message);
            return Results.BadRequest("Error occured while deleting postbox.");
        }
    }

    private async Task<IResult> DeleteAsync(IPostboxService service, string guid)
    {
        try
        {
            await service.DeleteAsync(guid);
            return Results.Ok("Deleting postbox succeeded.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while deleting postbox: {Message}", e.Message);
            return Results.BadRequest("Error occured while deleting postbox.");
        }
    }

    private static async Task<IResult> AddAsync(IPostboxService service, Postbox model)
    {
        try
        {
            await service.AddAsync(new PostboxEntity
            {
                PostBoxId = model.PostboxId,
                UserId = model.UserGuid
            });

            return Results.Ok("Postbox added successfully");
        }
        catch (Exception e)
        {
            return Results.BadRequest($"Error occured while adding postbox: {e.Message}");
        }
    }

    private async Task<List<Postbox>> GetAllAsync(
        IPostboxService service,
        [FromQuery] string? userId,
        [FromQuery] string? postboxId)
    {
        try
        {
            var postboxes = await service.GetAllAsync(userId, postboxId);

            return postboxes.Select(box => new Postbox(
                box.Id,
                box.PostBoxId,
                box.UserId,
                box.StatisticsEntity.TotalUnlocks,
                box.StatisticsEntity.LastModified)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while retriving postboxes: {Message}", e.Message);
            return new List<Postbox>();
        }
    }
}