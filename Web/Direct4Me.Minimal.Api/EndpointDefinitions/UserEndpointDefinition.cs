using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Services;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users/{email}", GetUserByEmailAsync);
        app.MapGet("/users", GetAllUsersAsync);
        app.MapPost("/users/signIn/email/{email}/password/{password}", TrySignInUserAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<User?> GetUserByEmailAsync(IUserService service, string email)
    {
        var user = await service.GetUserByEmailAsync(email);

        if (user != null)
            return new User(user.Id, user.Email, user.Password, user.Fullname, user.StatisticsEntity.TotalLogins,
                user.StatisticsEntity.LastModified);

        return null;
    }

    private static async Task<List<User>> GetAllUsersAsync(IUserService service)
    {
        var users = await service.GetAllUsersAsync();

        return users.Select(user => new User(user.Id, user.Email, user.Password, user.Fullname,
            user.StatisticsEntity.TotalLogins, user.StatisticsEntity.LastModified)).ToList();
    }

    private static async Task<IResult> TrySignInUserAsync(IUserService service, string email, string password)
    {
        bool signInResult = await service.TrySignInAsync(email, password);

        return signInResult
            ? Results.Ok("Sign-in successful.")
            : Results.BadRequest("Invalid email or password.");
    }
}