using System;

public class Incident
{
    public string Title { get; }
    public string Severity { get; }
    public DateTime DateReported { get; }

    public Incident(string title, string severity, DateTime dateReported)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(severity))
            throw new ArgumentException("Severity is required.", nameof(severity));
        if (!IsValidSeverity(severity))
            throw new ArgumentException("Severity must be Low, Medium, or High.", nameof(severity));

        Title = title;
        Severity = severity;
        DateReported = dateReported;
    }

    public string CalculateUrgency()
    {
        var age = (DateTime.UtcNow - DateReported).TotalDays;

        if (Severity.Equals("High", StringComparison.OrdinalIgnoreCase))
        {
            if (age >= 1) return "Immediate";
            return "High";
        }
        if (Severity.Equals("Medium", StringComparison.OrdinalIgnoreCase))
        {
            if (age >= 3) return "High";
            return "Medium";
        }
        // Low severity
        if (age >= 7) return "Medium";
        return "Low";
    }

    private static bool IsValidSeverity(string severity)
    {
        return severity.Equals("Low", StringComparison.OrdinalIgnoreCase)
            || severity.Equals("Medium", StringComparison.OrdinalIgnoreCase)
            || severity.Equals("High", StringComparison.OrdinalIgnoreCase);
    }
} 