using System.ComponentModel.DataAnnotations;

namespace IncidentMauiTaskC.Models;

/// <summary>
/// Model representing the incident form with user-friendly field names
/// </summary>
public class IncidentFormModel
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Priority { get; set; } = "Medium";

    [Required]
    public string Category { get; set; } = "General";

    [Required]
    [EmailAddress]
    public string ReporterEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string ReporterName { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime ReportedDate { get; set; } = DateTime.Now;

    public bool IsUrgent { get; set; } = false;

    // Additional form fields that might need transformation
    public string DeviceInfo { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public string BrowserVersion { get; set; } = string.Empty;
}

/// <summary>
/// Available priority levels
/// </summary>
public static class PriorityLevels
{
    public static readonly List<string> Values = new()
    {
        "Low",
        "Medium", 
        "High",
        "Critical"
    };
}

/// <summary>
/// Available incident categories
/// </summary>
public static class IncidentCategories
{
    public static readonly List<string> Values = new()
    {
        "General",
        "Technical",
        "Security",
        "Performance",
        "User Interface",
        "Data Issue",
        "Network",
        "Hardware"
    };
}