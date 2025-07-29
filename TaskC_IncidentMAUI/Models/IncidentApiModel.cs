using System.Text.Json.Serialization;

namespace TaskC_IncidentMAUI.Models
{
    public class IncidentApiModel
    {
        [JsonPropertyName("incident_title")]
        public string IncidentTitle { get; set; } = string.Empty;

        [JsonPropertyName("incident_description")]
        public string IncidentDescription { get; set; } = string.Empty;

        [JsonPropertyName("reporter_full_name")]
        public string ReporterFullName { get; set; } = string.Empty;

        [JsonPropertyName("contact_email")]
        public string ContactEmail { get; set; } = string.Empty;

        [JsonPropertyName("priority_level")]
        public string PriorityLevel { get; set; } = string.Empty;

        [JsonPropertyName("incident_category")]
        public string IncidentCategory { get; set; } = string.Empty;

        [JsonPropertyName("reported_date")]
        public string ReportedDate { get; set; } = string.Empty;

        [JsonPropertyName("incident_location")]
        public string IncidentLocation { get; set; } = string.Empty;

        [JsonPropertyName("is_urgent_flag")]
        public bool IsUrgentFlag { get; set; } = false;
    }
}