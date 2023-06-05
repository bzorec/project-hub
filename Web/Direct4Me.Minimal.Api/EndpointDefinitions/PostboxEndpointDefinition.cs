using System.Diagnostics.CodeAnalysis;
using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PostboxEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/postboxes", GetAllAsync);
        app.MapGet("/postboxes/history/user/{userGuid}", GetPostboxHistoryForUserAsync);
        app.MapGet("/postboxes/history/box/{boxId}", GetPostboxHistoryForSingleAsync);

        app.MapPost("/postboxes", AddAsync);
        app.MapPost("/postboxes/history", AddLogAsync);

        app.MapPut("/postboxes/{guid}/update", UpdateAsync);

        app.MapDelete("/postboxes/{guid}/delete", DeleteAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<IResult> AddLogAsync(IHistoryService service, IPostboxService postboxService,
        History model)
    {
        var entity = model.MapHistoryToPostboxHistoryEntity();

        try
        {
            await postboxService.LogBoxUnlockAsync(entity.PostboxId, entity.Type ?? throw new
                InvalidOperationException(), entity.Success);

            await service.LogHistoryAsync(entity);
            return Results.Ok("History log added successfully");
        }
        catch (Exception e)
        {
            return Results.BadRequest("Error occured while logging history.");
        }
    }

    private static async Task<List<PostboxHistoryEntity>> GetPostboxHistoryForSingleAsync(IHistoryService service,
        string boxId
    )
    {
        return await service.GetFullHistorySingleAsync(boxId);
    }

    private static async Task<List<PostboxHistoryEntity>> GetPostboxHistoryForUserAsync(IHistoryService service,
        string userGuid)
    {
        return await service.GetFullHistoryAsync(userGuid);
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
        [FromQuery] string? postboxId,
        [FromQuery] bool? isFromAccessList)
    {
        var postboxes = await service.GetAllAsync(userId, postboxId);

        if (userId == null || isFromAccessList != true)
            return postboxes != null && postboxes.Any()
                ? postboxes.Select(box => new Postbox(
                    box.Id,
                    box.PostBoxId,
                    box.UserId,
                    box.StatisticsEntity.TotalUnlocks,
                    box.StatisticsEntity.LastModified)).ToList()
                : new List<Postbox>();

        var otherIds = await service.GetOtherPostboxIdsForUser(userId);

        List<PostboxEntity> otherBoxes = new();
        foreach (var id in otherIds) otherBoxes.Add(await service.GetPostboxByIdAsync(id));

        postboxes.AddRange(otherBoxes.Distinct());

        return postboxes.Any()
            ? postboxes.Select(box => new Postbox(
                box.Id,
                box.PostBoxId,
                box.UserId,
                box.StatisticsEntity.TotalUnlocks,
                box.StatisticsEntity.LastModified)).ToList()
            : new List<Postbox>();
    }
}