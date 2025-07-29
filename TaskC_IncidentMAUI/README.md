# Task C: Mobile Data Mismatch & Mock API Handling (.NET MAUI) - Simplified

## Overview

This simplified .NET MAUI application demonstrates handling mobile form data submission with field name transformation using a local mock service. The solution focuses on the core challenge of data mapping between client-side form fields and expected API field names.

## 🎯 Challenge Requirements Met

### ✅ MAUI Screen with Form Inputs (Title and Severity only)
- **IncidentFormPage.xaml**: Clean, focused form with two input fields
- **Input Controls**: Entry for Title, Picker for Severity
- **Validation**: Real-time validation with error display
- **Modern UI**: Simple, responsive design with clear transformation hints

### ✅ JSON Payload Submission to Mock Endpoint (Local only)
- **Local Mock Service**: `MockApiServerService` simulates API processing
- **No HTTP Calls**: Pure local processing for demonstration
- **Field Transformation**: Shows exact mapping from form to API format

### ✅ Field Name Transformation
- **Client Fields**: `Title`, `Severity` (wrong field names)
- **API Fields**: `incident_title`, `severity_level` (correct field names)
- **Value Mapping**: Severity values transformed (e.g., "High" → "sev_high")

## 🏗️ Simplified Architecture

### Project Structure
```
TaskC_IncidentMAUI/
├── Models/
│   ├── IncidentFormModel.cs     # Form model (Title, Severity)
│   └── IncidentApiModel.cs      # API model (incident_title, severity_level)
├── Services/
│   ├── IIncidentApiService.cs   # Service interface
│   ├── IncidentApiService.cs    # Local service with transformation
│   └── MockApiServerService.cs  # Local mock API processor
├── ViewModels/
│   └── IncidentFormViewModel.cs # MVVM ViewModel
├── Views/
│   ├── IncidentFormPage.xaml    # Simple form UI
│   └── IncidentFormPage.xaml.cs # Code-behind
└── Converters/
    └── BooleanConverters.cs     # Value converters
```

## 🔄 Field Transformation Demo

### Before (Form Data - Wrong Field Names)
```json
{
  "title": "Database Connection Error",
  "severity": "High"
}
```

### After (API Payload - Correct Field Names)
```json
{
  "incident_title": "Database Connection Error",
  "severity_level": "sev_high"
}
```

### Transformation Mappings

#### Field Names
| Form Field | API Field Name | Transformation |
|------------|----------------|----------------|
| `Title` | `incident_title` | Direct mapping with name change |
| `Severity` | `severity_level` | Name change + value mapping |

#### Severity Value Mapping
| Form Value | API Value |
|------------|-----------|
| "Low" | "sev_low" |
| "Medium" | "sev_medium" |
| "High" | "sev_high" |
| "Critical" | "sev_critical" |

## 🚀 How It Works

### 1. User Input
- User enters incident title (using wrong field name)
- User selects severity level from dropdown

### 2. Field Transformation
```csharp
private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
{
    return new IncidentApiModel
    {
        IncidentTitle = formData.Title,                          // Title → incident_title
        SeverityLevel = MapSeverityToApiFormat(formData.Severity) // Severity → severity_level + value mapping
    };
}
```

### 3. Mock API Processing
- Local mock service receives transformed JSON
- Validates required fields using correct API field names
- Returns success response showing transformation worked

## 📱 User Experience

### Form Features
1. **Title Input**: Entry field with placeholder showing it's the "wrong" field name
2. **Severity Picker**: Dropdown with Low/Medium/High/Critical options
3. **Transformation Hints**: UI shows what fields will be transformed to
4. **Validation**: Real-time validation with error display
5. **Submit to Mock**: Button clearly indicates local mock service
6. **Response Display**: Shows transformation success message

### Visual Transformation Guide
The UI includes hints showing the transformation:
- "Title → incident_title"
- "Severity → severity_level (with value mapping)"
- "Example: 'High' becomes 'sev_high'"

## 🔧 Technical Implementation

### Key Components

#### 1. Form Model (Wrong Field Names)
```csharp
public class IncidentFormModel
{
    public string Title { get; set; } = string.Empty;     // Wrong name
    public string Severity { get; set; } = "Medium";      // Wrong name
}
```

#### 2. API Model (Correct Field Names)
```csharp
public class IncidentApiModel
{
    [JsonPropertyName("incident_title")]
    public string IncidentTitle { get; set; }     // Correct name
    
    [JsonPropertyName("severity_level")]
    public string SeverityLevel { get; set; }     // Correct name
}
```

#### 3. Local Mock Service
- No HTTP calls, pure local processing
- Demonstrates field validation using correct API names
- Shows transformation success in response

## 🧪 Testing the Implementation

### Test Flow
1. Enter incident title (e.g., "Server Down")
2. Select severity (e.g., "High")
3. Click "Submit to Mock API"
4. Observe transformation in logs and response

### Expected Results
- Form validates correctly
- Data transforms: Title → incident_title, High → sev_high
- Mock service processes with correct field names
- Success message shows transformation completed

## 📋 Simplified Dependencies

### Required NuGet Packages
- `Microsoft.Maui.Controls` - MAUI framework
- `CommunityToolkit.Mvvm` - MVVM helpers
- `System.Text.Json` - JSON serialization
- `Microsoft.Extensions.Logging.Debug` - Logging

### Removed Dependencies
- No HttpClient packages (local mock only)
- No external API calls
- Minimal dependencies for focused demo

This simplified implementation demonstrates the core concept of field transformation between mobile forms and API expectations using a clean, focused approach with just two fields and local processing.