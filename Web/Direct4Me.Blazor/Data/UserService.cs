using Direct4Me.Blazor.Models;
using Direct4Me.Blazor.Providers;

namespace Direct4Me.Blazor.Data;

public interface IUserService
{
    Task<bool> RegisterUserAsync(UserModel user);

    Task<UserModel> AuthenticateAsync(string loginEmail, string loginPassword);

    Task<bool> IsAuthenticated();
}

public class UserService : IUserService
{
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public UserService(IJwtTokenProvider jwtTokenProvider)
    {
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<bool> RegisterUserAsync(UserModel user)
    {
        await Task.CompletedTask;

        return false;
    }

    public async Task<UserModel> AuthenticateAsync(string loginEmail, string loginPassword)
    {
        var user = new UserModel(Guid.NewGuid(), loginEmail, loginPassword, "dummy", "dummy");

        user.Token = await _jwtTokenProvider.GenerateJwtTokenAsync(user);

        return user;
    }

    public async Task<bool> IsAuthenticated()
    {
        await Task.CompletedTask;

        return true;
    }
}