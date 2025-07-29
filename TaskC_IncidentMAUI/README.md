# Task C: Mobile Data Mismatch & Mock API Handling (.NET MAUI)

## Overview

This .NET MAUI application demonstrates handling mobile form data submission with field name transformation and mock API integration. The solution addresses the challenge of data mismatches between client-side form fields and server-side API expectations.

## 🎯 Challenge Requirements Met

### ✅ MAUI Screen with Form Inputs
- **IncidentFormPage.xaml**: Comprehensive form with various input types
- **Input Controls**: Entry, Editor, Picker, DatePicker, CheckBox
- **Validation**: Real-time validation with error display
- **Modern UI**: Responsive design with Frame containers and proper styling

### ✅ JSON Payload Submission to Mock Endpoint
- **External API**: Uses JSONPlaceholder (https://jsonplaceholder.typicode.com) for real HTTP requests
- **Mock Server**: Local `MockApiServerService` that simulates API processing
- **HTTP Client**: Configured HttpClient with proper error handling
- **Async Operations**: Proper async/await patterns throughout

### ✅ Field Name Transformation
- **Client Model**: `IncidentFormModel` with user-friendly field names
- **API Model**: `IncidentApiModel` with transformed field names
- **Transformation Logic**: 
  - `Title` → `incident_title`
  - `ReporterName` → `reporter_full_name`
  - `Email` → `contact_email`
  - `Priority` → `priority_level` (with value mapping)
  - `Category` → `incident_category` (with value mapping)
  - And more...

## 🏗️ Architecture

### Project Structure
```
TaskC_IncidentMAUI/
├── Models/
│   ├── IncidentFormModel.cs     # Client-side form model
│   └── IncidentApiModel.cs      # API-expected model with JsonPropertyName attributes
├── Services/
│   ├── IIncidentApiService.cs   # Service interface
│   ├── IncidentApiService.cs    # HTTP client service with transformation
│   └── MockApiServerService.cs  # Local mock API server
├── ViewModels/
│   └── IncidentFormViewModel.cs # MVVM ViewModel with CommunityToolkit
├── Views/
│   ├── IncidentFormPage.xaml    # Form UI
│   └── IncidentFormPage.xaml.cs # Code-behind
├── Converters/
│   └── BooleanConverters.cs     # Value converters for XAML bindings
└── App.xaml / MauiProgram.cs    # DI configuration
```

### Key Features

#### 1. Data Transformation Pipeline
```csharp
// Client Form → API Model transformation
private static IncidentApiModel TransformToApiModel(IncidentFormModel formData)
{
    return new IncidentApiModel
    {
        IncidentTitle = formData.Title,                    // Title → incident_title
        ReporterFullName = formData.ReporterName,          // ReporterName → reporter_full_name
        ContactEmail = formData.Email,                     // Email → contact_email
        PriorityLevel = MapPriorityToApiFormat(formData.Priority), // "High" → "priority_high"
        // ... more transformations
    };
}
```

#### 2. Field Value Mapping
```csharp
// Priority transformation
"High" → "priority_high"
"Medium" → "priority_medium"
"Low" → "priority_low"
"Critical" → "priority_critical"

// Category transformation  
"Technical" → "cat_technical"
"Security" → "cat_security"
"Hardware" → "cat_hardware"
// ... etc
```

#### 3. Mock API Processing
The `MockApiServerService` demonstrates:
- JSON payload parsing
- Server-side validation
- Response generation
- Error handling
- Field name validation

## 🚀 How to Run

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 with MAUI workload OR VS Code with C# extension
- Android emulator/device or iOS simulator (for mobile testing)

### Build & Run
```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run on specific platform
dotnet run --framework net9.0-android    # Android
dotnet run --framework net9.0-ios        # iOS  
dotnet run --framework net9.0-maccatalyst # macOS
dotnet run --framework net9.0-windows10.0.19041.0 # Windows
```

## 📱 User Experience

### Form Features
1. **Required Field Validation**: Real-time validation with visual feedback
2. **Email Validation**: Proper email format checking
3. **Dropdown Selections**: Priority and Category pickers
4. **Date Selection**: DatePicker for incident date
5. **Optional Fields**: Location and urgent flag
6. **API Status Check**: Button to verify API connectivity
7. **Loading States**: ActivityIndicator during submission
8. **Success/Error Messages**: Clear user feedback

### Field Transformation Demo
The app logs show the transformation process:
```json
// Original form data (client-side)
{
  "title": "Server Down",
  "reporterName": "John Doe", 
  "email": "john@company.com",
  "priority": "High"
}

// Transformed API payload
{
  "incident_title": "Server Down",
  "reporter_full_name": "John Doe",
  "contact_email": "john@company.com", 
  "priority_level": "priority_high"
}
```

## 🔧 Technical Implementation

### Dependency Injection
```csharp
// MauiProgram.cs
builder.Services.AddHttpClient<IIncidentApiService, IncidentApiService>();
builder.Services.AddSingleton<MockApiServerService>();
builder.Services.AddSingleton<IIncidentApiService, IncidentApiService>();
builder.Services.AddTransient<IncidentFormViewModel>();
```

### MVVM Pattern
- **ObservableObject**: Base class from CommunityToolkit.Mvvm
- **RelayCommand**: Command binding for button actions
- **ObservableProperty**: Auto-generated property change notifications
- **Data Binding**: Two-way binding between View and ViewModel

### Error Handling
- Client-side validation with DataAnnotations
- Network error handling with try-catch
- API response validation
- User-friendly error messages

## 🧪 Testing the Implementation

### Test Scenarios
1. **Valid Submission**: Fill all required fields and submit
2. **Validation Errors**: Try submitting with empty required fields
3. **Invalid Email**: Test email validation
4. **API Status**: Check connectivity to external API
5. **Field Transformation**: Observe logs for field name changes
6. **Mock API**: See local processing results

### Expected Behavior
- Form validates in real-time
- Successful submissions show confirmation message
- Field names are transformed correctly in JSON payload
- Mock API processes and validates transformed data
- External API receives properly formatted requests

## 📋 API Contract Demonstration

### Input (Client Form)
```csharp
public class IncidentFormModel
{
    public string Title { get; set; }
    public string ReporterName { get; set; } 
    public string Email { get; set; }
    public string Priority { get; set; }
    // ... more fields
}
```

### Output (API Expected)
```csharp
public class IncidentApiModel
{
    [JsonPropertyName("incident_title")]
    public string IncidentTitle { get; set; }
    
    [JsonPropertyName("reporter_full_name")]
    public string ReporterFullName { get; set; }
    
    [JsonPropertyName("contact_email")]
    public string ContactEmail { get; set; }
    
    [JsonPropertyName("priority_level")]
    public string PriorityLevel { get; set; }
    // ... more transformed fields
}
```

This implementation successfully demonstrates handling data mismatches between mobile form fields and API expectations through proper field transformation, validation, and mock API integration.