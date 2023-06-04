using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Direct4Me.Blazor.Services;

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
        private readonly IJsInteropService _jsInterop;
        private readonly ILogger<FaceRecognitionService> _logger;

        public FaceRecognitionService(ILogger<FaceRecognitionService> logger, HttpClient httpClient,
            IJsInteropService jsInterop)
        {
            _logger = logger;
            _httpClient = httpClient;
            _jsInterop = jsInterop;
        }

        public async Task<bool> RecognizeFace(string userEmail, Stream faceImage)
        {
            var list = new List<string>
            {
                "Cucek01",
                "Bezjak01"
            };
            var user = userEmail switch
            {
                "mcucek@gmail.com" => list.First(),
                "abezjak@gmail.com" => list.Last(),
                _ => list.First()
            };

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

                        return result != null && result.UserId == user;
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
            var user = await _jsInterop.GetUserEmail();

            try
            {
                foreach (var faceImage in faceImages)
                {
                    var content = new MultipartFormDataContent();
                    var imageContent = new StreamContent(faceImage);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                    content.Add(imageContent, "image", $"{Guid.NewGuid()}.png");
                    content.Add(new StringContent(user), "userId"); // Add the userId parameter

                    var response = await _httpClient.PostAsync("/uploadImage", content);

                    if (response.IsSuccessStatusCode) continue;

                    _logger.LogError("Face recognition setup failed with status code: {StatusCode}",
                        response.StatusCode);
                    return false;
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