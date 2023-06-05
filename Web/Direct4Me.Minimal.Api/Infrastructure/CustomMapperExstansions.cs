using Direct4Me.Minimal.Api.Models;
using Direct4Me.Repository.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Direct4Me.Minimal.Api.Infrastructure;

public static class CustomMapperExstansions
{
    public static UserEntity? MapUserToUserEntity(this User model)
    {
        if (model.Fullname.IsNullOrEmpty()) return null;

        var split = model.Fullname.Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new UserEntity
        {
            Email = model.Email,
            Password = model.Password,
            FirstName = split.First(),
            LastName = split.Last()
        };
    }

    public static PostboxEntity MapPostboxToPostboxEntity(this Postbox postbox)
    {
        return new PostboxEntity
        {
            PostBoxId = postbox.PostboxId,
            Id = postbox.Guid,
            UserId = postbox.UserGuid
        };
    }

    public static PostboxHistoryEntity MapHistoryToPostboxHistoryEntity(this History history)
    {
        return new PostboxHistoryEntity
        {
            Date = DateTime.Now,
            UserName = history.UserName,
            PostboxId = history.BoxId,
            Type = history.Type,
            Success = history.Success,
            Id = history.Guid
        };
    }
}