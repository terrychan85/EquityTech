# Field Transformation Demonstration

## Overview
This document demonstrates how client-side form fields are transformed to match API expectations.

## Field Mappings

### 1. Basic Field Name Transformations

| Client Form Field | API Field Name | Type | Notes |
|-------------------|----------------|------|-------|
| `Title` | `incident_title` | string | Direct mapping |
| `Description` | `incident_description` | string | Direct mapping |
| `ReporterName` | `reporter_full_name` | string | Field name change |
| `Email` | `contact_email` | string | Field name change |
| `Location` | `incident_location` | string | Direct mapping with prefix |
| `DateReported` | `reported_date` | string | Date formatting applied |
| `IsUrgent` | `is_urgent_flag` | boolean | Field name change |

### 2. Value Transformations

#### Priority Mapping
| Form Value | API Value |
|------------|-----------|
| "Low" | "priority_low" |
| "Medium" | "priority_medium" |
| "High" | "priority_high" |
| "Critical" | "priority_critical" |

#### Category Mapping
| Form Value | API Value |
|------------|-----------|
| "General" | "cat_general" |
| "Technical" | "cat_technical" |
| "Security" | "cat_security" |
| "Hardware" | "cat_hardware" |
| "Software" | "cat_software" |

## Sample Transformation

### Input (Form Data)
```json
{
  "title": "Database Connection Error",
  "description": "Unable to connect to the production database server",
  "reporterName": "Jane Smith",
  "email": "jane.smith@company.com",
  "priority": "High",
  "category": "Technical",
  "dateReported": "2024-01-15T10:30:00",
  "location": "Data Center Room 3",
  "isUrgent": true
}
```

### Output (API Payload)
```json
{
  "incident_title": "Database Connection Error",
  "incident_description": "Unable to connect to the production database server",
  "reporter_full_name": "Jane Smith",
  "contact_email": "jane.smith@company.com",
  "priority_level": "priority_high",
  "incident_category": "cat_technical",
  "reported_date": "2024-01-15T10:30:00.000Z",
  "incident_location": "Data Center Room 3",
  "is_urgent_flag": true
}
```

## Implementation Details

### Transformation Method
```csharp
private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
{
    return new IncidentApiModel
    {
        IncidentTitle = formData.Title,
        IncidentDescription = formData.Description,
        ReporterFullName = formData.ReporterName,
        ContactEmail = formData.Email,
        PriorityLevel = MapPriorityToApiFormat(formData.Priority),
        IncidentCategory = MapCategoryToApiFormat(formData.Category),
        ReportedDate = formData.DateReported.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
        IncidentLocation = formData.Location,
        IsUrgentFlag = formData.IsUrgent
    };
}
```

### JSON Serialization Attributes
```csharp
public class IncidentApiModel
{
    [JsonPropertyName("incident_title")]
    public string IncidentTitle { get; set; }
    
    [JsonPropertyName("reporter_full_name")]
    public string ReporterFullName { get; set; }
    
    // ... other properties with JsonPropertyName attributes
}
```

## Validation
- Client-side validation ensures form completeness
- Server-side validation (mock API) validates transformed data
- Email format validation applied
- Required field validation enforced

## Error Handling
- Transformation errors logged
- API errors handled gracefully  
- User-friendly error messages displayed
- Retry mechanism available

This transformation layer ensures seamless integration between the mobile form and backend API, regardless of field naming conventions.