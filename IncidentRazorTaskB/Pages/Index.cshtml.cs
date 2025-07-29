using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class IncidentsModel : PageModel
{
    public List<Incident> Incidents { get; set; }

    public void OnGet()
    {
        // Sample data
        Incidents = new List<Incident>
        {
            new Incident { Title = "Server Down", Description = "Main server is not responding.", Severity = "High" },
            new Incident { Title = "Login Slow", Description = "User login is slow.", Severity = "Medium" },
            new Incident { Title = "UI Glitch", Description = "Minor UI misalignment.", Severity = "Low" }
        };
    }

    public string GetSeverityClass(string severity)
    {
        return severity?.ToLower() switch
        {
            "low" => "severity-low",
            "medium" => "severity-medium",
            "high" => "severity-high",
            _ => string.Empty
        };
    }

    public class Incident
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }
} 