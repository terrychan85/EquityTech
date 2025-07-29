using Microsoft.Extensions.Logging;
using IncidentMauiTaskC.Tests;
using IncidentMauiTaskC.Services;

namespace IncidentMauiTaskC;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Mobile Data Mismatch & Mock API Handling Demo ===");
        Console.WriteLine("(.NET MAUI Concepts in Console Application)");
        Console.WriteLine();
        
        // Run the transformation demo to show how data mismatch is handled
        Console.WriteLine("1. Running Data Transformation Demo...\n");
        TransformationDemo.RunDemo();
        
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("2. Interactive API Testing...");
        Console.WriteLine(new string('=', 60));
        
        await RunInteractiveApiTest();
        
        Console.WriteLine("\n\nDemo completed! This showcases how a MAUI mobile app would:");
        Console.WriteLine("✅ Handle form data with user-friendly field names");
        Console.WriteLine("✅ Transform data to match API expected format");
        Console.WriteLine("✅ Submit JSON payload to mock endpoints");
        Console.WriteLine("✅ Handle API response and error scenarios");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
    
    static async Task RunInteractiveApiTest()
    {
        // Set up services like a MAUI app would
        var httpClient = new HttpClient();
        var transformationService = new DataTransformationService();
        var apiService = new IncidentApiService(httpClient, transformationService);
        
        // Test multiple scenarios
        var testScenarios = new[]
        {
            new { 
                Name = "Valid Incident - High Priority",
                FormData = CreateSampleIncident("High", "Application Login Failure", true)
            },
            new { 
                Name = "Valid Incident - Low Priority", 
                FormData = CreateSampleIncident("Low", "Minor UI Glitch", false)
            },
            new { 
                Name = "Invalid Data Test",
                FormData = CreateInvalidIncident()
            }
        };
        
        foreach (var scenario in testScenarios)
        {
            Console.WriteLine($"\n🧪 Testing Scenario: {scenario.Name}");
            Console.WriteLine(new string('-', 40));
            
            try
            {
                var (success, message, incidentId) = await apiService.SubmitIncidentMockAsync(scenario.FormData);
                
                Console.WriteLine($"Result: {(success ? "✅ SUCCESS" : "❌ FAILED")}");
                Console.WriteLine($"Message: {message}");
                if (!string.IsNullOrEmpty(incidentId))
                {
                    Console.WriteLine($"Incident ID: {incidentId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
            }
            
            await Task.Delay(500); // Simulate user interaction delay
        }
    }
    
    static IncidentMauiTaskC.Models.IncidentFormModel CreateSampleIncident(string priority, string title, bool isUrgent)
    {
        return new IncidentMauiTaskC.Models.IncidentFormModel
        {
            Title = title,
            Description = $"Detailed description for {title.ToLower()}. This incident requires attention from the development team.",
            Priority = priority,
            Category = "Technical",
            ReporterEmail = "test.user@company.com",
            ReporterName = "Test User",
            PhoneNumber = "(555) 123-4567",
            Location = "Remote Office",
            IsUrgent = isUrgent,
            DeviceInfo = "iPhone 14 Pro",
            OperatingSystem = "iOS 17.1",
            BrowserVersion = "Mobile App v2.1.0",
            ReportedDate = DateTime.Now
        };
    }
    
    static IncidentMauiTaskC.Models.IncidentFormModel CreateInvalidIncident()
    {
        return new IncidentMauiTaskC.Models.IncidentFormModel
        {
            Title = "", // Invalid - empty title
            Description = "Test", // Invalid - too short
            Priority = "Medium",
            Category = "General",
            ReporterEmail = "invalid-email", // Invalid email format
            ReporterName = "", // Invalid - empty name
            PhoneNumber = "invalid",
            Location = "Test Location",
            IsUrgent = false,
            DeviceInfo = "Test Device",
            OperatingSystem = "Test OS",
            BrowserVersion = "Test Browser",
            ReportedDate = DateTime.Now
        };
    }
}
