using System;

class Program
{
    static void Main()
    {
        try
        {
            var incident = new Incident(
                title: "Database Down",
                severity: "High",
                dateReported: DateTime.UtcNow.AddHours(-30) // 1.25 days ago
            );

            Console.WriteLine($"Incident: {incident.Title}");
            Console.WriteLine($"Severity: {incident.Severity}");
            Console.WriteLine($"Date Reported: {incident.DateReported}");
            Console.WriteLine($"Urgency: {incident.CalculateUrgency()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
