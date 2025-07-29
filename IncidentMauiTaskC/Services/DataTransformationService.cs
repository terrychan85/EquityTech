using IncidentMauiTaskC.Models;

namespace IncidentMauiTaskC.Services;

/// <summary>
/// Service responsible for transforming form data to API payload format
/// This demonstrates how to handle field name mismatches and format differences
/// </summary>
public class DataTransformationService
{
    /// <summary>
    /// Transforms form model to API payload with proper field mapping
    /// </summary>
    /// <param name="formModel">The form data model</param>
    /// <returns>Transformed API payload</returns>
    public ApiIncidentPayload TransformToApiPayload(IncidentFormModel formModel)
    {
        if (formModel == null)
            throw new ArgumentNullException(nameof(formModel));

        return new ApiIncidentPayload
        {
            // Transform field names from user-friendly to API expected format
            IncidentTitle = formModel.Title,
            IncidentDescription = formModel.Description,
            PriorityLevel = TransformPriorityLevel(formModel.Priority),
            IncidentCategory = TransformCategory(formModel.Category),
            ReporterEmailAddress = formModel.ReporterEmail,
            ReporterFullName = formModel.ReporterName,
            ContactPhone = FormatPhoneNumber(formModel.PhoneNumber),
            IncidentLocation = formModel.Location,
            ReportTimestamp = FormatTimestamp(formModel.ReportedDate),
            IsUrgentFlag = formModel.IsUrgent,
            DeviceInformation = formModel.DeviceInfo,
            OsVersion = formModel.OperatingSystem,
            BrowserDetails = formModel.BrowserVersion
        };
    }

    /// <summary>
    /// Transforms priority level to API expected format
    /// </summary>
    private string TransformPriorityLevel(string priority)
    {
        return priority?.ToUpperInvariant() switch
        {
            "LOW" => "P4",
            "MEDIUM" => "P3",
            "HIGH" => "P2", 
            "CRITICAL" => "P1",
            _ => "P3" // Default to medium
        };
    }

    /// <summary>
    /// Transforms category to API expected format
    /// </summary>
    private string TransformCategory(string category)
    {
        return category?.ToLowerInvariant().Replace(" ", "_") ?? "general";
    }

    /// <summary>
    /// Formats phone number for API
    /// </summary>
    private string FormatPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        // Remove all non-digit characters
        var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
        
        // Format as international if it looks like a US number
        if (digitsOnly.Length == 10)
            return $"+1{digitsOnly}";
        
        if (digitsOnly.Length == 11 && digitsOnly.StartsWith("1"))
            return $"+{digitsOnly}";
            
        return phoneNumber; // Return as-is if format is unclear
    }

    /// <summary>
    /// Formats timestamp for API (ISO 8601 format)
    /// </summary>
    private string FormatTimestamp(DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    /// <summary>
    /// Validates the transformed payload before sending to API
    /// </summary>
    public (bool IsValid, List<string> Errors) ValidateApiPayload(ApiIncidentPayload payload)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(payload.IncidentTitle))
            errors.Add("Incident title is required");

        if (string.IsNullOrWhiteSpace(payload.IncidentDescription))
            errors.Add("Incident description is required");

        if (string.IsNullOrWhiteSpace(payload.ReporterEmailAddress))
            errors.Add("Reporter email is required");

        if (string.IsNullOrWhiteSpace(payload.ReporterFullName))
            errors.Add("Reporter name is required");

        // Validate email format
        if (!string.IsNullOrWhiteSpace(payload.ReporterEmailAddress) && 
            !IsValidEmail(payload.ReporterEmailAddress))
            errors.Add("Invalid email format");

        return (errors.Count == 0, errors);
    }

    private bool IsValidEmail(string email)
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