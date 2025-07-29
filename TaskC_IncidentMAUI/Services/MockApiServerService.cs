using System.Text.Json;
using TaskC_IncidentMAUI.Models;

namespace TaskC_IncidentMAUI.Services
{
    public class MockApiServerService
    {
        private readonly ILogger<MockApiServerService> _logger;
        private readonly List<IncidentApiModel> _submittedIncidents;

        public MockApiServerService(ILogger<MockApiServerService> logger)
        {
            _logger = logger;
            _submittedIncidents = new List<IncidentApiModel>();
        }

        public async Task<(bool Success, string Response)> ProcessIncidentSubmissionAsync(string jsonPayload)
        {
            try
            {
                _logger.LogInformation("Mock API received payload: {Payload}", jsonPayload);

                // Simulate API processing delay
                await Task.Delay(500);

                // Parse the transformed JSON payload
                var incidentData = JsonSerializer.Deserialize<IncidentApiModel>(jsonPayload);
                
                if (incidentData == null)
                {
                    return (false, "Invalid JSON payload received");
                }

                // Validate required fields (demonstrating API-side validation)
                var validationResult = ValidateApiModel(incidentData);
                if (!validationResult.IsValid)
                {
                    return (false, $"Validation failed: {validationResult.ErrorMessage}");
                }

                // Store the incident (simulate database storage)
                _submittedIncidents.Add(incidentData);

                var responseData = new
                {
                    success = true,
                    incident_id = Guid.NewGuid().ToString(),
                    message = "Incident submitted successfully",
                    received_fields = new
                    {
                        incident_title = incidentData.IncidentTitle,
                        severity_level = incidentData.SeverityLevel
                    },
                    field_transformation_demo = new
                    {
                        original_field_names = "Title, Severity",
                        transformed_field_names = "incident_title, severity_level",
                        transformation_successful = true
                    },
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                string response = JsonSerializer.Serialize(responseData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Mock API processed successfully. Response: {Response}", response);
                return (true, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing incident submission in mock API");
                return (false, $"Server error: {ex.Message}");
            }
        }

        public List<IncidentApiModel> GetAllSubmittedIncidents()
        {
            return _submittedIncidents.ToList();
        }

        public int GetTotalSubmissionCount()
        {
            return _submittedIncidents.Count;
        }

        private static (bool IsValid, string ErrorMessage) ValidateApiModel(IncidentApiModel model)
        {
            if (string.IsNullOrWhiteSpace(model.IncidentTitle))
                return (false, "incident_title is required");

            if (string.IsNullOrWhiteSpace(model.SeverityLevel))
                return (false, "severity_level is required");

            return (true, string.Empty);
        }
    }
}