using System.Diagnostics.CodeAnalysis;
using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PostboxEndpointDefinition : IEndpointDefinition
{
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

    private static async Task<IResult> UpdateAsync(IPostboxService service, string guid, Postbox model)
    {
        var entity = model.MapPostboxToPostboxEntity();

        if (entity == null) return Results.BadRequest("Error occured while updating postbox.");

        return await service.UpdateAsync(entity)
            ? Results.Ok("Deleting postbox succeeded.")
            : Results.BadRequest("Error occured while deleting postbox.");
    }

    private static async Task<IResult> DeleteAsync(IPostboxService service, string guid)
    {
        return await service.DeleteAsync(guid)
            ? Results.Ok("Deleting postbox succeeded.")
            : Results.BadRequest("Error occured while deleting postbox.");
    }

    private static async Task<IResult> AddAsync(IPostboxService service, Postbox model)
    {
        var entity = model.MapPostboxToPostboxEntity();

        if (entity == null) return Results.BadRequest("Error occured while adding postbox.");

        return await service.AddAsync(entity)
            ? Results.Ok("Postbox added successfully")
            : Results.BadRequest("Error occured while adding postbox.");
    }

    private static async Task<List<Postbox>> GetAllAsync(
        IPostboxService service,
        [FromQuery] string? userId,
        [FromQuery] string? postboxId)
    {
        var postboxes = await service.GetAllAsync(userId, postboxId);

        return postboxes != null && postboxes.Any()
            ? postboxes.Select(box => new Postbox(
                box.Id,
                box.PostBoxId,
                box.UserId,
                box.StatisticsEntity.TotalUnlocks,
                box.StatisticsEntity.LastModified)).ToList()
            : new List<Postbox>();
    }
}