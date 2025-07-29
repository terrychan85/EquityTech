# Task C: Mobile Data Mismatch & Mock API Handling (.NET MAUI)

## Project Overview

This .NET MAUI application demonstrates how to handle mobile form data transformation when field names and formats don't match the expected API structure. The project showcases a real-world scenario where a mobile form's data fails at the API due to mismatched field names or formats.

## 🎯 Challenge Objectives

- ✅ Build a MAUI screen with comprehensive form inputs
- ✅ Submit JSON payload to a mock endpoint
- ✅ Transform field names to match expected API keys (e.g., `incident_title`)
- ✅ Handle field format transformations (priority levels, phone numbers, dates)
- ✅ Implement proper validation and error handling
- ✅ Demonstrate field mapping and data transformation

## 🏗️ Project Structure

```
IncidentMauiTaskC/
├── Models/
│   ├── IncidentFormModel.cs          # User-friendly form model
│   └── ApiIncidentPayload.cs         # API-expected payload model
├── Services/
│   ├── DataTransformationService.cs  # Field transformation logic
│   └── IncidentApiService.cs         # API communication service
├── ViewModels/
│   └── IncidentFormViewModel.cs      # MVVM ViewModel with commands
├── Views/
│   ├── IncidentFormPage.xaml         # Main form UI
│   └── IncidentFormPage.xaml.cs      # Form code-behind
├── Converters/
│   └── ValueConverters.cs            # XAML value converters
├── Tests/
│   └── TransformationDemo.cs         # Demonstration of transformations
└── Resources/
    └── Styles/                       # MAUI styling resources
```

## 🔄 Data Transformation Examples

### Field Name Transformations

| Form Field (User-Friendly) | API Field (Expected) | Transformation |
|----------------------------|---------------------|----------------|
| `Title` | `incident_title` | Direct mapping |
| `Description` | `incident_description` | Direct mapping |
| `Priority` | `priority_level` | Value transformation: "High" → "P2" |
| `Category` | `incident_category` | Format transformation: "User Interface" → "user_interface" |
| `ReporterName` | `reporter_full_name` | Direct mapping |
| `ReporterEmail` | `reporter_email_address` | Direct mapping |
| `PhoneNumber` | `contact_phone` | Format transformation: "(555) 123-4567" → "+15551234567" |
| `Location` | `incident_location` | Direct mapping |
| `IsUrgent` | `is_urgent_flag` | Direct mapping |
| `DeviceInfo` | `device_information` | Direct mapping |
| `OperatingSystem` | `os_version` | Direct mapping |
| `BrowserVersion` | `browser_details` | Direct mapping |
| `ReportedDate` | `report_timestamp` | Format transformation: DateTime → ISO 8601 string |

### Priority Level Transformations

```csharp
"Low"      → "P4"
"Medium"   → "P3"
"High"     → "P2"
"Critical" → "P1"
```

### Phone Number Formatting

```csharp
"(555) 123-4567"  → "+15551234567"
"555-123-4567"    → "+15551234567"
"1-555-123-4567"  → "+15551234567"
```

## 🚀 Key Features

### 1. Comprehensive Form UI
- User-friendly input fields with validation
- Dropdown selections for Priority and Category
- Auto-population of device information
- Real-time form validation
- Visual feedback for form state

### 2. Data Transformation Service
- Automatic field name mapping
- Value format transformations
- Phone number standardization
- Date formatting to ISO 8601
- Validation before API submission

### 3. Mock API Integration
- Simulated API endpoints for testing
- Retry logic with exponential backoff
- Comprehensive error handling
- Success/failure response simulation

### 4. MVVM Architecture
- Clean separation of concerns
- Data binding with INotifyPropertyChanged
- Command pattern for user interactions
- Observable collections for dropdown data

## 🧪 Testing the Application

### Option 1: Run the Transformation Demo

The application starts with a console demonstration:

```bash
dotnet run
```

This will show:
1. Original form data with user-friendly field names
2. Transformed API payload with expected field names
3. Field mapping transformations
4. Validation results
5. Mock API simulation

### Option 2: Use the MAUI Form

After the demo, the MAUI application launches with:
- **Fill Sample Data**: Populates the form with test data
- **Preview Payload**: Shows the transformed JSON payload
- **Submit Incident**: Sends data to mock API
- **Clear Form**: Resets all fields

## 📱 Form Features

### Required Fields
- Incident Title (5-100 characters)
- Description (10-500 characters)
- Priority Level (dropdown)
- Category (dropdown)
- Reporter Name (2-50 characters)
- Reporter Email (valid email format)

### Optional Fields
- Phone Number (with format validation)
- Location
- Urgent flag (checkbox)

### Auto-Generated Fields
- Device Information (auto-detected)
- Operating System (auto-detected)
- Application Version
- Submission timestamp

## 🔧 Configuration

### API Endpoint Configuration
```csharp
// In IncidentApiService.cs
private readonly string _baseUrl = "https://api.mockincidents.com";
```

### Mock API Behavior
- 10% chance of simulated failure for testing
- 1-second delay to simulate network latency
- Generates realistic incident IDs
- Returns proper success/error responses

## 🛡️ Error Handling

### Validation Levels
1. **Client-side Validation**: Form field validation using data annotations
2. **Transformation Validation**: Ensures all required fields are properly transformed
3. **API Validation**: Server-side validation simulation
4. **Network Error Handling**: Timeout, retry, and connection error management

### Error Scenarios Handled
- Missing required fields
- Invalid email formats
- Network connectivity issues
- API timeout errors
- Server-side validation failures
- JSON serialization errors

## 🎨 UI/UX Features

### Modern Design
- Clean, professional interface
- Bootstrap-inspired color scheme
- Responsive layout for different screen sizes
- Visual feedback for form states

### User Experience
- Clear field labels with required indicators
- Helpful placeholder text
- Real-time validation feedback
- Success/error status messages
- Loading states during submission

## 🔄 Data Flow

1. **User Input**: User fills out the form with user-friendly field names
2. **Form Validation**: Client-side validation using data annotations
3. **Data Transformation**: Transform form model to API payload model
4. **API Validation**: Validate transformed payload before sending
5. **API Submission**: Send JSON payload to mock endpoint
6. **Response Handling**: Process success/error responses
7. **User Feedback**: Display results to user

## 🧰 Technologies Used

- **.NET 8.0**: Latest .NET framework
- **MAUI**: Cross-platform UI framework
- **MVVM Pattern**: Model-View-ViewModel architecture
- **CommunityToolkit.Mvvm**: MVVM helpers and source generators
- **Newtonsoft.Json**: JSON serialization with custom field mapping
- **Data Annotations**: Form validation
- **Dependency Injection**: Service registration and resolution

## 📋 Installation & Setup

1. **Prerequisites**:
   - .NET 8.0 SDK
   - MAUI workload (if available on your platform)

2. **Build the Project**:
   ```bash
   cd IncidentMauiTaskC
   dotnet restore
   dotnet build
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```

## 🔍 Code Highlights

### Data Transformation Example
```csharp
public ApiIncidentPayload TransformToApiPayload(IncidentFormModel formModel)
{
    return new ApiIncidentPayload
    {
        IncidentTitle = formModel.Title,
        PriorityLevel = TransformPriorityLevel(formModel.Priority),
        ContactPhone = FormatPhoneNumber(formModel.PhoneNumber),
        ReportTimestamp = FormatTimestamp(formModel.ReportedDate),
        // ... other field mappings
    };
}
```

### JSON Field Mapping
```csharp
[JsonProperty("incident_title")]
public string IncidentTitle { get; set; }

[JsonProperty("priority_level")]
public string PriorityLevel { get; set; }
```

## 📊 Sample Transformation Output

### Input (Form Data)
```json
{
  "Title": "Mobile App Crash on Login",
  "Priority": "High",
  "PhoneNumber": "(555) 123-4567",
  "ReportedDate": "2024-01-15T10:30:00"
}
```

### Output (API Payload)
```json
{
  "incident_title": "Mobile App Crash on Login",
  "priority_level": "P2",
  "contact_phone": "+15551234567",
  "report_timestamp": "2024-01-15T10:30:00Z"
}
```

This project demonstrates professional handling of data transformation challenges commonly encountered in mobile applications when integrating with legacy or external APIs that expect different field formats and naming conventions.