using Newtonsoft.Json;

namespace IncidentMauiTaskC.Models;

/// <summary>
/// Model representing the expected API payload structure with transformed field names
/// </summary>
public class ApiIncidentPayload
{
    [JsonProperty("incident_title")]
    public string IncidentTitle { get; set; } = string.Empty;

    [JsonProperty("incident_description")]
    public string IncidentDescription { get; set; } = string.Empty;

    [JsonProperty("priority_level")]
    public string PriorityLevel { get; set; } = string.Empty;

    [JsonProperty("incident_category")]
    public string IncidentCategory { get; set; } = string.Empty;

    [JsonProperty("reporter_email_address")]
    public string ReporterEmailAddress { get; set; } = string.Empty;

    [JsonProperty("reporter_full_name")]
    public string ReporterFullName { get; set; } = string.Empty;

    [JsonProperty("contact_phone")]
    public string ContactPhone { get; set; } = string.Empty;

    [JsonProperty("incident_location")]
    public string IncidentLocation { get; set; } = string.Empty;

    [JsonProperty("report_timestamp")]
    public string ReportTimestamp { get; set; } = string.Empty;

    [JsonProperty("is_urgent_flag")]
    public bool IsUrgentFlag { get; set; } = false;

    [JsonProperty("device_information")]
    public string DeviceInformation { get; set; } = string.Empty;

    [JsonProperty("os_version")]
    public string OsVersion { get; set; } = string.Empty;

    [JsonProperty("browser_details")]
    public string BrowserDetails { get; set; } = string.Empty;

    [JsonProperty("submission_id")]
    public string SubmissionId { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("api_version")]
    public string ApiVersion { get; set; } = "1.0";
}

/// <summary>
/// API response model
/// </summary>
public class ApiResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("incident_id")]
    public string IncidentId { get; set; } = string.Empty;

    [JsonProperty("errors")]
    public List<string> Errors { get; set; } = new();

    [JsonProperty("timestamp")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
}