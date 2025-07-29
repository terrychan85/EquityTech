# Field Transformation Demonstration - Simplified

## Overview
This document demonstrates how client-side form fields (with wrong field names) are transformed to match API expectations (correct field names) in our simplified MAUI application.

## Field Mappings

### Basic Field Name Transformations

| Client Form Field | API Field Name | Type | Transformation |
|-------------------|----------------|------|----------------|
| `Title` | `incident_title` | string | Direct mapping with name change |
| `Severity` | `severity_level` | string | Name change + value mapping |

### Severity Value Transformations

| Form Value | API Value |
|------------|-----------|
| "Low" | "sev_low" |
| "Medium" | "sev_medium" |
| "High" | "sev_high" |
| "Critical" | "sev_critical" |

## Sample Transformation

### Input (Form Data - Wrong Field Names)
```json
{
  "title": "Database Connection Error",
  "severity": "High"
}
```

### Output (API Payload - Correct Field Names)
```json
{
  "incident_title": "Database Connection Error",
  "severity_level": "sev_high"
}
```

## Implementation Details

### Transformation Method
```csharp
private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
{
    return new IncidentApiModel
    {
        IncidentTitle = formData.Title,                          // Title → incident_title
        SeverityLevel = MapSeverityToApiFormat(formData.Severity) // Severity → severity_level + value mapping
    };
}

private static string MapSeverityToApiFormat(string severity)
{
    return severity.ToLower() switch
    {
        "low" => "sev_low",
        "medium" => "sev_medium", 
        "high" => "sev_high",
        "critical" => "sev_critical",
        _ => "sev_medium"
    };
}
```

### JSON Serialization Attributes
```csharp
public class IncidentApiModel
{
    [JsonPropertyName("incident_title")]
    public string IncidentTitle { get; set; }
    
    [JsonPropertyName("severity_level")]
    public string SeverityLevel { get; set; }
}
```

## Mock API Processing

### Local Mock Service Response
```json
{
  "success": true,
  "incident_id": "123e4567-e89b-12d3-a456-426614174000",
  "message": "Incident submitted successfully",
  "received_fields": {
    "incident_title": "Database Connection Error",
    "severity_level": "sev_high"
  },
  "field_transformation_demo": {
    "original_field_names": "Title, Severity",
    "transformed_field_names": "incident_title, severity_level",
    "transformation_successful": true
  },
  "timestamp": "2024-01-15T10:30:00.000Z"
}
```

## UI Transformation Hints

The form UI includes visual hints to show users the transformation:

- **Title Field**: "Enter incident title (wrong field name)"
  - Hint: "This will be transformed to 'incident_title'"

- **Severity Field**: "Severity Level *"
  - Hint: "This will be transformed to 'severity_level' with value mapping"

- **Info Box**: 
  - "Title → incident_title"
  - "Severity → severity_level (with value mapping)"
  - "Example: 'High' becomes 'sev_high'"

## Validation

### Client-Side Validation (Wrong Field Names)
- Title: Required, max 100 characters
- Severity: Required, must be selected

### Server-Side Validation (Correct Field Names)
- incident_title: Required field validation
- severity_level: Required field validation

## Error Handling
- Form validation prevents submission with empty fields
- Mock service validates using correct API field names
- Clear error messages for validation failures
- Success message confirms transformation completed

## Key Benefits

1. **Clear Demonstration**: Simple two-field example shows concept clearly
2. **No External Dependencies**: Local mock service, no HTTP calls needed
3. **Visual Learning**: UI hints show transformation in real-time
4. **Complete Flow**: From wrong field names to correct API format
5. **Validation Example**: Shows both client and server-side validation

This simplified transformation demonstrates the core concept of handling field name mismatches between mobile forms and backend APIs using a focused, easy-to-understand approach.