using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using Direct4Me.Core.Auth;
using Direct4Me.Minimal.Api.Infrastructure;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;
using Direct4Me.Minimal.Api.Models;
using Direct4Me.Minimal.Api.Models.FaceUnlock;
using Direct4Me.Minimal.Api.Models.Login;
using Direct4Me.Repository.Services.Interfaces;
using ImageCompressorDecompressor;
using Microsoft.AspNetCore.Mvc;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users", GetAllAsync);
        app.MapGet("/user", GetAsync);
        app.MapPost("/users", AddAsync);
        app.MapPost("/users/login", LoginAsync);
        app.MapPost("/users/login/face-login", FaceLoginAsync);
        app.MapPut("/users/{guid}/update", UpdateAsync);
        app.MapDelete("/users/{guid}/delete", DeleteAsync);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    private static async Task<IResult> FaceLoginAsync(
        IUserService service,
        HttpClient httpClient,
        FaceUnlockRequest faceUnlock,
        CancellationToken token = default)
    {
        var user = await service.GetUserByEmailAsync(faceUnlock.Email, token);
        if (user is { IsFaceUnlock: false }) return Results.Forbid();

        var byteImage = Convert.FromBase64String(faceUnlock.Base64Image);

        //Decompress image using our own ulta decompresser 9000
        Bitmap bmp = byteImage.Decompress();

        using var imageStream = new MemoryStream();
        bmp.Save(imageStream, ImageFormat.Png);

        //reset position of image stream bak to zerooooo 0
        imageStream.Position = 0;

        //using var imageStream = new MemoryStream(byteImage);
        var content = new MultipartFormDataContent();
        var imageContent = new StreamContent(imageStream);
        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        content.Add(imageContent, "image", $"{Guid.NewGuid()}.png");

        var response = await httpClient.PostAsync("http://localhost:8000/imgAuthenticate", content, token);
        if (!response.IsSuccessStatusCode) return Results.BadRequest("Something went wrong.");

        var jsonResponse = await response.Content.ReadAsStringAsync(token);
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(jsonResponse);

        if (result is not { Confidence: >= 1 }) return Results.Forbid();

        return user != null
            ? Results.Ok(new FaceUnlockResponse(user.Id,
                user.Email,
                user.Fullname,
                user.StatisticsEntity.TotalLogins,
                user.StatisticsEntity.LastModified))
            : Results.Forbid();
    }

    private static async Task<IResult> LoginAsync(IUserService service, LoginRequest model,
        CancellationToken token = default)
    {
        var signedId = await service.TrySignInAsync(model.Email, model.Password, token);

        if (!signedId) return Results.BadRequest("Something went wrong.");

        var user = await service.GetUserByEmailAsync(model.Email, token);

        if (user == null)
        {
            return Results.BadRequest();
        }

        return Results.Ok(new LoginResponse(user.Id,
            user.Email,
            user.Fullname,
            user.StatisticsEntity.TotalLogins,
            user.StatisticsEntity.LastModified,
            user.IsFaceUnlock));
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

        return users.Any()
            ? users.Select(user => new User(
                user.Id,
                user.Email,
                user.Password,
                user.Fullname,
                user.StatisticsEntity.TotalLogins,
                user.StatisticsEntity.LastModified)).ToList()
            : new List<User>();
    }

    private static async Task<UserSimple?> GetAsync(
        IUserService service,
        [FromQuery] string? id,
        [FromQuery] string? email,
        [FromQuery] string? firstname,
        [FromQuery] string? lastname,
        [FromQuery] DateTime? lastAccessed)
    {
        var user = await service.GetAsync(id, email, firstname, lastname, lastAccessed);

        return new UserSimple(user.Id,
            user.Email,
            user.Fullname,
            user.StatisticsEntity.TotalLogins,
            user.StatisticsEntity.LastModified,
            user.IsFaceUnlock);
    }
}