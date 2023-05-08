using Direct4Me.Blazor.Models;

namespace Direct4Me.Blazor.Data;

public class UserService
{
    public async Task<bool> RegisterUserAsync(UserModel user)
    {
        await Task.CompletedTask;

        return false;
    }

    public async Task<UserModel> AuthenticateAsync(string loginEmail, string loginPassword)
    {
        await Task.CompletedTask;

        return new UserModel(Guid.NewGuid(), "dummy", "dummy", "dummy", "dummy");
    }

    public async Task<bool> IsAuthenticated()
    {
        await Task.CompletedTask;

        return true;
    }
}