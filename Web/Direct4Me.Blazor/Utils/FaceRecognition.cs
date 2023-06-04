using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Direct4Me.Blazor.Utils
{
    public interface IFaceRecognitionService
    {
        Task<bool> RecognizeFace(string userEmail, Stream faceImage);
        Task<bool> SetupFaceRecognition(IEnumerable<Stream> faceImages);
    }

    public class FaceRecognitionService : IFaceRecognitionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FaceRecognitionService> _logger;

        public FaceRecognitionService(ILogger<FaceRecognitionService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<bool> RecognizeFace(string userEmail, Stream faceImage)
        {
            try
            {
                var content = new MultipartFormDataContent();
                var imageContent = new StreamContent(faceImage);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                content.Add(imageContent, "image", $"{Guid.NewGuid()}.png");

                var timeoutCancellationTokenSource = new CancellationTokenSource();
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), timeoutCancellationTokenSource.Token);

                var responseTask =
                    _httpClient.PostAsync("/imgAuthenticate", content, timeoutCancellationTokenSource.Token);

                // Wait for either the response or the timeout
                var completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == responseTask)
                {
                    // Response received within the timeout period
                    var response = await responseTask;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse =
                            await response.Content.ReadAsStringAsync(timeoutCancellationTokenSource.Token);
                        var result = JsonSerializer.Deserialize<AuthenticationResponse>(jsonResponse);

                        return result != null && result.UserId == userEmail;
                    }

                    _logger.LogError("Face recognition failed with status code: {StatusCode}", response.StatusCode);
                    return false;
                }

                // Timeout occurred
                timeoutCancellationTokenSource.Cancel();
                _logger.LogError("Face recognition request timed out");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while recognizing the face");
                return false;
            }
        }

        public async Task<bool> SetupFaceRecognition(IEnumerable<Stream> faceImages)
        {
            try
            {
                foreach (var faceImage in faceImages)
                {
                    var content = new MultipartFormDataContent();
                    var imageContent = new StreamContent(faceImage);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                    content.Add(imageContent, "image", $"{Guid.NewGuid()}.png");

                    var response = await _httpClient.PostAsync("/uploadImage", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Face recognition setup failed with status code: {StatusCode}",
                            response.StatusCode);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting up face recognition");
                return false;
            }
        }
    }

    public class AuthenticationResponse
    {
        [JsonPropertyName("user_id")] public string UserId { get; set; }

        [JsonPropertyName("confidence")] public float Confidence { get; set; }
    }
}