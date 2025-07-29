using System.Text.Json.Serialization;

namespace TaskC_IncidentMAUI.Models
{
    public class IncidentApiModel
    {
        [JsonPropertyName("incident_title")]
        public string IncidentTitle { get; set; } = string.Empty;

        [JsonPropertyName("severity_level")]
        public string SeverityLevel { get; set; } = string.Empty;
    }
}