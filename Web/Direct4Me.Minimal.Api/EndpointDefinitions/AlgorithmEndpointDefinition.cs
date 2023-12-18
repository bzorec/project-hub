using Direct4Me.Core.ImageProccessing.Interfaces;
using Direct4Me.Minimal.Api.Infrastructure.Interfaces;

namespace Direct4Me.Minimal.Api.EndpointDefinitions;

public class AlgorithmEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/algorithm/img/process", ProcessImage);
    }

    private static async Task<IResult> ProcessImage(IImageProcessingService imageProcessingService,
        IPythonAiService pythonAiService,
        IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0) return Results.BadRequest("No image file provided.");

        try
        {
            var compressedImage = await imageProcessingService.CompressImageAsync(imageFile);
            var aiResult = await pythonAiService.ProcessImageAsync(compressedImage);

            return Results.Ok(aiResult);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public void DefineServices(IServiceCollection services) => services
        .AddTransient<IImageProcessingService>()
        .AddTransient<IPythonAiService>();
}