using System.Diagnostics.CodeAnalysis;
using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users", GetAllAsync);
        app.MapPost("/users", AddAsync);
        app.MapPost("/users/login", LoginAsync);
        app.MapPut("/users/{guid}/update", UpdateAsync);
        app.MapDelete("/users/{guid}/delete", DeleteAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<IResult> LoginAsync(IUserService service, User model)
    {
        return await service.TrySignInAsync(model.Email, model.Password)
            ? Results.Ok("Signed in.")
            : Results.BadRequest("Something went wrong.");
    }

    private static async Task<IResult> UpdateAsync(IUserService service, string guid, User model)
    {
        var entity = model.MapUserToUserEntity();

        if (entity == null)
            return Results.BadRequest("Error occured while adding user.");

        return await service.UpdateAsync(entity)
            ? Results.Ok("Updating user succeeded.")
            : Results.BadRequest("Error occured while Updating user.");
    }

    private static async Task<IResult> DeleteAsync(IUserService service, string guid)
    {
        return await service.DeleteAsync(guid)
            ? Results.Ok("Deleting user succeeded.")
            : Results.BadRequest("Error occured while deleting user.");
    }

    private static async Task<IResult> AddAsync(IUserService service, User model)
    {
        var entity = model.MapUserToUserEntity();

        if (entity == null)
            return Results.BadRequest("Error occured while adding user.");

        return await service.AddAsync(entity)
            ? Results.Ok("Adding new user succeeded.")
            : Results.BadRequest("Error occured while adding user.");
    }


    private static async Task<List<User>> GetAllAsync(
        IUserService service,
        [FromQuery] string? firstname,
        [FromQuery] string? lastname,
        [FromQuery] DateTime? lastAccessed)
    {
        var users = await service.GetAllAsync(firstname, lastname, lastAccessed);

        return users != null && users.Any()
            ? users.Select(user => new User(
                user.Id,
                user.Email,
                user.Password,
                user.Fullname,
                user.StatisticsEntity.TotalLogins,
                user.StatisticsEntity.LastModified)).ToList()
            : new List<User>();
    }
}