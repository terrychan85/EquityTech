using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskC_IncidentMAUI.Models;

namespace TaskC_IncidentMAUI.Services
{
    public class IncidentApiService : IIncidentApiService
    {
        private readonly ILogger<IncidentApiService> _logger;
        private readonly MockApiServerService _mockServer;

        public IncidentApiService(ILogger<IncidentApiService> logger, MockApiServerService mockServer)
        {
            _logger = logger;
            _mockServer = mockServer;
        }

        public async Task<(bool Success, string Message)> SubmitIncidentAsync(IncidentFormModel formData)
        {
            try
            {
                _logger.LogInformation("Starting incident submission process...");

                // Transform the form data to match API expectations
                var apiModel = TransformToApiModel(formData);
                
                // Serialize to JSON to show the transformation
                var jsonContent = JsonSerializer.Serialize(apiModel, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Form data transformed to API format: {JsonContent}", jsonContent);

                // Submit to local mock service only (no HTTP calls)
                var mockResult = await _mockServer.ProcessIncidentSubmissionAsync(jsonContent);
                
                if (mockResult.Success)
                {
                    _logger.LogInformation("Incident submitted successfully to mock service");
                    return (true, "Incident submitted successfully! Field transformation completed.");
                }
                else
                {
                    _logger.LogError("Mock service failed: {Error}", mockResult.Response);
                    return (false, $"Submission failed: {mockResult.Response}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting incident");
                return (false, $"Error: {ex.Message}");
            }
        }

        private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
        {
            // Transform field names from form to API expectations
            return new IncidentApiModel
            {
                IncidentTitle = formData.Title,                          // Title → incident_title
                SeverityLevel = MapSeverityToApiFormat(formData.Severity) // Severity → severity_level (with value mapping)
            };
        }

        private static string MapSeverityToApiFormat(string severity)
        {
            // Transform severity values to match API expectations
            return severity.ToLower() switch
            {
                "low" => "sev_low",
                "medium" => "sev_medium", 
                "high" => "sev_high",
                "critical" => "sev_critical",
                _ => "sev_medium"
            };
        }
    }
}