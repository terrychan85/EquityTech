using IncidentMauiTaskC.Models;
using IncidentMauiTaskC.Services;
using Newtonsoft.Json;

namespace IncidentMauiTaskC.Tests;

/// <summary>
/// Demonstration class showing how the data transformation works
/// This simulates the mobile form data mismatch scenario
/// </summary>
public static class TransformationDemo
{
    /// <summary>
    /// Demonstrates the field transformation from form model to API payload
    /// </summary>
    public static void RunDemo()
    {
        Console.WriteLine("=== Mobile Data Mismatch & API Transformation Demo ===\n");

        // Create sample form data with user-friendly field names
        var formModel = new IncidentFormModel
        {
            Title = "Mobile App Crash on Login",
            Description = "The mobile application crashes when users attempt to login after the latest update. The crash occurs immediately after entering credentials.",
            Priority = "High",
            Category = "Technical",
            ReporterEmail = "jane.smith@company.com",
            ReporterName = "Jane Smith",
            PhoneNumber = "(555) 987-6543",
            Location = "Remote Work - Home Office",
            IsUrgent = true,
            DeviceInfo = "iPhone 14 Pro",
            OperatingSystem = "iOS 17.1",
            BrowserVersion = "Mobile App v2.1.0",
            ReportedDate = DateTime.Now
        };

        Console.WriteLine("1. ORIGINAL FORM DATA (User-Friendly Field Names):");
        Console.WriteLine("==================================================");
        DisplayFormData(formModel);

        // Transform the data
        var transformationService = new DataTransformationService();
        var apiPayload = transformationService.TransformToApiPayload(formModel);

        Console.WriteLine("\n2. TRANSFORMED API PAYLOAD (API Expected Field Names):");
        Console.WriteLine("=====================================================");
        var jsonPayload = JsonConvert.SerializeObject(apiPayload, Formatting.Indented);
        Console.WriteLine(jsonPayload);

        // Show field mapping
        Console.WriteLine("\n3. FIELD NAME TRANSFORMATIONS:");
        Console.WriteLine("==============================");
        ShowFieldMappings();

        // Validate the payload
        var (isValid, errors) = transformationService.ValidateApiPayload(apiPayload);
        Console.WriteLine("\n4. VALIDATION RESULTS:");
        Console.WriteLine("======================");
        Console.WriteLine($"Is Valid: {isValid}");
        if (!isValid)
        {
            Console.WriteLine("Errors:");
            errors.ForEach(error => Console.WriteLine($"  - {error}"));
        }
        else
        {
            Console.WriteLine("✅ All validations passed!");
        }

        // Simulate API call
        Console.WriteLine("\n5. MOCK API SIMULATION:");
        Console.WriteLine("=======================");
        SimulateApiCall(apiPayload);
    }

    private static void DisplayFormData(IncidentFormModel form)
    {
        Console.WriteLine($"Title: {form.Title}");
        Console.WriteLine($"Description: {form.Description}");
        Console.WriteLine($"Priority: {form.Priority}");
        Console.WriteLine($"Category: {form.Category}");
        Console.WriteLine($"Reporter Name: {form.ReporterName}");
        Console.WriteLine($"Reporter Email: {form.ReporterEmail}");
        Console.WriteLine($"Phone Number: {form.PhoneNumber}");
        Console.WriteLine($"Location: {form.Location}");
        Console.WriteLine($"Is Urgent: {form.IsUrgent}");
        Console.WriteLine($"Device Info: {form.DeviceInfo}");
        Console.WriteLine($"Operating System: {form.OperatingSystem}");
        Console.WriteLine($"Browser Version: {form.BrowserVersion}");
        Console.WriteLine($"Reported Date: {form.ReportedDate}");
    }

    private static void ShowFieldMappings()
    {
        var mappings = new[]
        {
            ("Title", "incident_title"),
            ("Description", "incident_description"),
            ("Priority", "priority_level (with transformation: High → P2)"),
            ("Category", "incident_category (with transformation: Technical → technical)"),
            ("ReporterName", "reporter_full_name"),
            ("ReporterEmail", "reporter_email_address"),
            ("PhoneNumber", "contact_phone (with formatting: (555) 987-6543 → +15559876543)"),
            ("Location", "incident_location"),
            ("IsUrgent", "is_urgent_flag"),
            ("DeviceInfo", "device_information"),
            ("OperatingSystem", "os_version"),
            ("BrowserVersion", "browser_details"),
            ("ReportedDate", "report_timestamp (ISO 8601 format)")
        };

        foreach (var (formField, apiField) in mappings)
        {
            Console.WriteLine($"  {formField,-20} → {apiField}");
        }
    }

    private static void SimulateApiCall(ApiIncidentPayload payload)
    {
        Console.WriteLine("Sending POST request to: https://api.mockincidents.com/api/incidents");
        Console.WriteLine("Content-Type: application/json");
        Console.WriteLine("\nPayload size: " + JsonConvert.SerializeObject(payload).Length + " bytes");
        
        // Simulate response
        Console.WriteLine("\n📤 Sending request...");
        System.Threading.Thread.Sleep(1000); // Simulate network delay
        
        var response = new ApiResponse
        {
            Success = true,
            Message = "Incident submitted successfully",
            IncidentId = $"INC-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

        Console.WriteLine("📥 Response received:");
        Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }
}