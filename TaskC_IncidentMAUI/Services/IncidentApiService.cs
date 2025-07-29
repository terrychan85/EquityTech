using System.Text;
using System.Text.Json;
using TaskC_IncidentMAUI.Models;

namespace TaskC_IncidentMAUI.Services
{
    public class IncidentApiService : IIncidentApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IncidentApiService> _logger;
        private readonly MockApiServerService _mockServer;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com"; // Mock API endpoint

        public IncidentApiService(HttpClient httpClient, ILogger<IncidentApiService> logger, MockApiServerService mockServer)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mockServer = mockServer;
        }

        public async Task<bool> SubmitIncidentAsync(IncidentFormModel formData)
        {
            try
            {
                // Transform the form data to match API expectations
                var apiModel = TransformToApiModel(formData);
                
                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(apiModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                _logger.LogInformation("Submitting incident data: {JsonContent}", jsonContent);

                // Test with mock API first to demonstrate field transformation
                var mockResult = await _mockServer.ProcessIncidentSubmissionAsync(jsonContent);
                _logger.LogInformation("Mock API Result - Success: {Success}, Response: {Response}", 
                    mockResult.Success, mockResult.Response);

                // Also submit to real endpoint for demonstration
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/posts", content);

                if (response.IsSuccessStatusCode && mockResult.Success)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("External API submitted successfully. Response: {Response}", responseContent);
                    return true;
                }
                else if (mockResult.Success)
                {
                    // Even if external API fails, consider it success if mock API worked
                    _logger.LogInformation("Mock API processing succeeded. Field transformation validated.");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to submit incident. External API Status: {StatusCode}, Mock API Success: {MockSuccess}", 
                        response.StatusCode, mockResult.Success);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting incident");
                return false;
            }
        }

        public async Task<string> GetSubmissionStatusAsync()
        {
            try
            {
                // Check mock server status
                int mockSubmissions = _mockServer.GetTotalSubmissionCount();
                
                // Check external API status
                var response = await _httpClient.GetAsync($"{BaseUrl}/posts/1");
                
                if (response.IsSuccessStatusCode)
                {
                    return $"API is available. Mock server has processed {mockSubmissions} submissions.";
                }
                else
                {
                    return $"External API returned status: {response.StatusCode}. Mock server has {mockSubmissions} submissions.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking API status");
                return $"Unable to connect to external API. Mock server has {_mockServer.GetTotalSubmissionCount()} submissions.";
            }
        }

        private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
        {
            // Transform field names and formats to match API expectations
            return new IncidentApiModel
            {
                IncidentTitle = formData.Title,
                IncidentDescription = formData.Description,
                ReporterFullName = formData.ReporterName,
                ContactEmail = formData.Email,
                PriorityLevel = MapPriorityToApiFormat(formData.Priority),
                IncidentCategory = MapCategoryToApiFormat(formData.Category),
                ReportedDate = formData.DateReported.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IncidentLocation = formData.Location,
                IsUrgentFlag = formData.IsUrgent
            };
        }

        private static string MapPriorityToApiFormat(string priority)
        {
            // Transform priority values to match API expectations
            return priority.ToLower() switch
            {
                "low" => "priority_low",
                "medium" => "priority_medium", 
                "high" => "priority_high",
                "critical" => "priority_critical",
                _ => "priority_medium"
            };
        }

        private static string MapCategoryToApiFormat(string category)
        {
            // Transform category values to match API expectations
            return category.ToLower() switch
            {
                "general" => "cat_general",
                "technical" => "cat_technical",
                "security" => "cat_security",
                "hardware" => "cat_hardware",
                "software" => "cat_software",
                _ => "cat_general"
            };
        }
    }
}