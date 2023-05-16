using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services;
using Direct4Me.Repository.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    private readonly ILogger<UserEndpointDefinition> _logger;

    public UserEndpointDefinition()
    {
    }

    public UserEndpointDefinition(ILogger<UserEndpointDefinition> logger)
    {
        _logger = logger;
    }

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

    private async Task<IResult> LoginAsync(IUserService service, User model)
    {
        try
        {
            var result = await service.TrySignInAsync(model.Email, model.Password);
            return result ? Results.Ok("Signed in.") : Results.BadRequest("Something went wrong.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Message}", e.Message);
            return Results.BadRequest("Error occured while signing in.");
        }
    }

    private async Task<IResult> UpdateAsync(IUserService service, string guid, User model)
    {
        try
        {
            var split = model.Fullname.Split(' ',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            await service.UpdateAsync(new UserEntity
            {
                Id = guid,
                Password = model.Password,
                FirstName = split.First(),
                LastName = split.Last(),
                Email = model.Email
            });

            return Results.Ok("Updating user succeeded.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Message}", e.Message);
            return Results.BadRequest("Error occured while Updating user.");
        }
    }

    private async Task<IResult> DeleteAsync(IUserService service, string guid)
    {
        try
        {
            await service.DeleteAsync(guid);
            return Results.Ok("Deleting user succeeded.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while deleting user: {Message}", e.Message);
            return Results.BadRequest("Error occured while deleting user.");
        }
    }

    private async Task<IResult> AddAsync(IUserService service, User model)
    {
        try
        {
            var split = model.Fullname.Split(' ',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var result = await service.AddAsync(new UserEntity
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = split.First(),
                LastName = split.Last()
            });
            return Results.Ok("Adding new user succeeded.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while adding user: {Message}", e.Message);
            return Results.BadRequest("Error occured while adding user.");
        }
    }


    private async Task<List<User>> GetAllAsync(
        IUserService service,
        [FromQuery] string email,
        [FromQuery] string name)
    {
        try
        {
            var users = await service.GetAllAsync();

            return users.Select(user => new User(
                user.Id,
                user.Email,
                user.Password,
                user.Fullname,
                user.StatisticsEntity.TotalLogins,
                user.StatisticsEntity.LastModified)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured when retriving users with filter: [{Filter}]; Exception: {Message}",
                email.IsNullOrEmpty() && name.IsNullOrEmpty() ? "none" : $"email:{email}, name:{name}", e.Message);

            return new List<User>();
        }
    }
}