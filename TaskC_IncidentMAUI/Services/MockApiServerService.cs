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
                await Task.Delay(1000);

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
                incidentData.ReportedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                _submittedIncidents.Add(incidentData);

                var responseData = new
                {
                    success = true,
                    incident_id = Guid.NewGuid().ToString(),
                    message = "Incident submitted successfully",
                    received_fields = new
                    {
                        incident_title = incidentData.IncidentTitle,
                        incident_description = incidentData.IncidentDescription,
                        reporter_full_name = incidentData.ReporterFullName,
                        contact_email = incidentData.ContactEmail,
                        priority_level = incidentData.PriorityLevel,
                        incident_category = incidentData.IncidentCategory,
                        reported_date = incidentData.ReportedDate,
                        incident_location = incidentData.IncidentLocation,
                        is_urgent_flag = incidentData.IsUrgentFlag
                    },
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                string response = JsonSerializer.Serialize(responseData, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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

            if (string.IsNullOrWhiteSpace(model.IncidentDescription))
                return (false, "incident_description is required");

            if (string.IsNullOrWhiteSpace(model.ReporterFullName))
                return (false, "reporter_full_name is required");

            if (string.IsNullOrWhiteSpace(model.ContactEmail))
                return (false, "contact_email is required");

            if (!IsValidEmail(model.ContactEmail))
                return (false, "contact_email must be a valid email address");

            if (string.IsNullOrWhiteSpace(model.PriorityLevel))
                return (false, "priority_level is required");

            if (string.IsNullOrWhiteSpace(model.IncidentCategory))
                return (false, "incident_category is required");

            return (true, string.Empty);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}