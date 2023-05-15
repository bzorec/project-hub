using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Repository.Entities;
using Direct4Me.Repository.Services;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users/{email}", GetAllUsersAsync);
        app.MapGet("/users", GetUserByEmailAsync);
        app.MapPost("/users/signIn/email/{email}/password/{password}", TrySignInUserAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<UserEntity> GetUserByEmailAsync(IUserService service, string email)
    {
        return await service.GetUserByEmailAsync(email);
    }

    private static async Task<List<UserEntity>> GetAllUsersAsync(IUserService service, string email)
    {
        var list = await service.GetAllUsersAsync();

        return list.ToList();
    }

    private static async Task<IResult> TrySignInUserAsync(IUserService service, string email, string password)
    {
        bool signInResult = await service.TrySignInAsync(email, password);

        return signInResult
            ? Results.Ok("Sign-in successful.")
            : Results.BadRequest("Invalid email or password.");
    }
}