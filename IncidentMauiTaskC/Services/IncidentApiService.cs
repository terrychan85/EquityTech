using System.Text;
using Newtonsoft.Json;
using IncidentMauiTaskC.Models;

namespace IncidentMauiTaskC.Services;

/// <summary>
/// Service for handling API communication with proper error handling and retry logic
/// </summary>
public class IncidentApiService
{
    private readonly HttpClient _httpClient;
    private readonly DataTransformationService _transformationService;
    private readonly string _baseUrl;

    public IncidentApiService(HttpClient httpClient, DataTransformationService transformationService)
    {
        _httpClient = httpClient;
        _transformationService = transformationService;
        _baseUrl = "https://api.mockincidents.com"; // Mock API endpoint
        
        // Configure HttpClient
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "IncidentMauiApp/1.0");
    }

    /// <summary>
    /// Submits incident form to API with field transformation
    /// </summary>
    public async Task<(bool Success, string Message, string IncidentId)> SubmitIncidentAsync(IncidentFormModel formModel)
    {
        try
        {
            // Transform form data to API payload
            var apiPayload = _transformationService.TransformToApiPayload(formModel);
            
            // Validate transformed payload
            var (isValid, errors) = _transformationService.ValidateApiPayload(apiPayload);
            if (!isValid)
            {
                return (false, $"Validation failed: {string.Join(", ", errors)}", string.Empty);
            }

            // Serialize payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(apiPayload, Formatting.Indented);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Log the transformed payload for debugging
            System.Diagnostics.Debug.WriteLine("Transformed API Payload:");
            System.Diagnostics.Debug.WriteLine(jsonPayload);

            // Make API call with retry logic
            var response = await MakeApiCallWithRetryAsync(content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                
                return apiResponse?.Success == true 
                    ? (true, apiResponse.Message, apiResponse.IncidentId)
                    : (false, apiResponse?.Message ?? "Unknown error occurred", string.Empty);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, $"API call failed: {response.StatusCode} - {errorContent}", string.Empty);
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Network error: {ex.Message}", string.Empty);
        }
        catch (TaskCanceledException)
        {
            return (false, "Request timeout - please try again", string.Empty);
        }
        catch (JsonException ex)
        {
            return (false, $"Data format error: {ex.Message}", string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Unexpected error: {ex.Message}", string.Empty);
        }
    }

    /// <summary>
    /// Makes API call with retry logic for better reliability
    /// </summary>
    private async Task<HttpResponseMessage> MakeApiCallWithRetryAsync(StringContent content)
    {
        const int maxRetries = 3;
        var delay = TimeSpan.FromSeconds(1);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/incidents", content);
                
                // If we get a temporary error, retry
                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                    response.StatusCode == System.Net.HttpStatusCode.RequestTimeout ||
                    (int)response.StatusCode >= 500)
                {
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(delay);
                        delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2); // Exponential backoff
                        continue;
                    }
                }

                return response;
            }
            catch (HttpRequestException) when (attempt < maxRetries)
            {
                await Task.Delay(delay);
                delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2);
            }
        }

        // If all retries failed, make one final attempt
        return await _httpClient.PostAsync($"{_baseUrl}/api/incidents", content);
    }

    /// <summary>
    /// Mock API call for testing when real endpoint is not available
    /// </summary>
    public async Task<(bool Success, string Message, string IncidentId)> SubmitIncidentMockAsync(IncidentFormModel formModel)
    {
        try
        {
            // Transform form data to API payload
            var apiPayload = _transformationService.TransformToApiPayload(formModel);
            
            // Validate transformed payload
            var (isValid, errors) = _transformationService.ValidateApiPayload(apiPayload);
            if (!isValid)
            {
                return (false, $"Validation failed: {string.Join(", ", errors)}", string.Empty);
            }

            // Log the transformed payload
            var jsonPayload = JsonConvert.SerializeObject(apiPayload, Formatting.Indented);
            System.Diagnostics.Debug.WriteLine("Mock API - Transformed Payload:");
            System.Diagnostics.Debug.WriteLine(jsonPayload);

            // Simulate API delay
            await Task.Delay(1000);

            // Simulate occasional API failures for testing
            var random = new Random();
            if (random.Next(100) < 10) // 10% chance of failure
            {
                return (false, "Mock API error: Service temporarily unavailable", string.Empty);
            }

            // Simulate successful response
            var mockIncidentId = $"INC-{DateTime.Now:yyyyMMdd}-{random.Next(1000, 9999)}";
            
            return (true, "Incident submitted successfully", mockIncidentId);
        }
        catch (Exception ex)
        {
            return (false, $"Mock API error: {ex.Message}", string.Empty);
        }
    }

    /// <summary>
    /// Test the transformation without making an API call
    /// </summary>
    public string GetTransformedPayloadPreview(IncidentFormModel formModel)
    {
        try
        {
            var apiPayload = _transformationService.TransformToApiPayload(formModel);
            return JsonConvert.SerializeObject(apiPayload, Formatting.Indented);
        }
        catch (Exception ex)
        {
            return $"Error transforming data: {ex.Message}";
        }
    }
}