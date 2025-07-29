using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentMauiTaskC.Models;
using IncidentMauiTaskC.Services;

namespace IncidentMauiTaskC.ViewModels;

/// <summary>
/// ViewModel for the incident form with data binding and command handling
/// </summary>
public partial class IncidentFormViewModel : ObservableObject
{
    private readonly IncidentApiService _apiService;
    private readonly DataTransformationService _transformationService;

    [ObservableProperty]
    private IncidentFormModel _incidentForm = new();

    [ObservableProperty]
    private bool _isSubmitting = false;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isSuccess = false;

    [ObservableProperty]
    private string _lastIncidentId = string.Empty;

    [ObservableProperty]
    private string _transformedPayloadPreview = string.Empty;

    [ObservableProperty]
    private bool _showPreview = false;

    public ObservableCollection<string> PriorityLevels { get; } = new(Models.PriorityLevels.Values);
    public ObservableCollection<string> Categories { get; } = new(Models.IncidentCategories.Values);

    public IncidentFormViewModel(IncidentApiService apiService, DataTransformationService transformationService)
    {
        _apiService = apiService;
        _transformationService = transformationService;
        
        // Initialize with some default values for demo
        IncidentForm.ReportedDate = DateTime.Now;
        
        // Auto-populate device information
        PopulateDeviceInformation();
    }

    /// <summary>
    /// Command to submit the incident form
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSubmitForm))]
    private async Task SubmitFormAsync()
    {
        IsSubmitting = true;
        StatusMessage = "Submitting incident...";
        IsSuccess = false;

        try
        {
            // Validate form data
            var validationResults = ValidateForm();
            if (validationResults.Any())
            {
                StatusMessage = $"Validation errors: {string.Join(", ", validationResults)}";
                return;
            }

            // Submit to mock API (since real API might not be available)
            var (success, message, incidentId) = await _apiService.SubmitIncidentMockAsync(IncidentForm);
            
            IsSuccess = success;
            StatusMessage = message;
            
            if (success)
            {
                LastIncidentId = incidentId;
                StatusMessage = $"✅ {message}\nIncident ID: {incidentId}";
                
                // Clear form after successful submission
                await Task.Delay(2000); // Show success message for 2 seconds
                ClearForm();
            }
            else
            {
                StatusMessage = $"❌ {message}";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"❌ Error: {ex.Message}";
            IsSuccess = false;
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    /// <summary>
    /// Command to preview the transformed payload
    /// </summary>
    [RelayCommand]
    private void PreviewTransformation()
    {
        try
        {
            TransformedPayloadPreview = _apiService.GetTransformedPayloadPreview(IncidentForm);
            ShowPreview = !ShowPreview;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error generating preview: {ex.Message}";
        }
    }

    /// <summary>
    /// Command to clear the form
    /// </summary>
    [RelayCommand]
    private void ClearForm()
    {
        IncidentForm = new IncidentFormModel
        {
            ReportedDate = DateTime.Now
        };
        PopulateDeviceInformation();
        StatusMessage = string.Empty;
        IsSuccess = false;
        LastIncidentId = string.Empty;
        TransformedPayloadPreview = string.Empty;
        ShowPreview = false;
    }

    /// <summary>
    /// Command to auto-fill form with sample data for testing
    /// </summary>
    [RelayCommand]
    private void FillSampleData()
    {
        IncidentForm.Title = "Application Login Issue";
        IncidentForm.Description = "Users are experiencing timeout errors when attempting to log into the application. The error occurs after entering credentials and clicking the login button. No error message is displayed to the user.";
        IncidentForm.Priority = "High";
        IncidentForm.Category = "Technical";
        IncidentForm.ReporterEmail = "john.doe@company.com";
        IncidentForm.ReporterName = "John Doe";
        IncidentForm.PhoneNumber = "(555) 123-4567";
        IncidentForm.Location = "Building A, Floor 3";
        IncidentForm.IsUrgent = true;
        
        StatusMessage = "Sample data loaded for testing";
    }

    private bool CanSubmitForm()
    {
        return !IsSubmitting && 
               !string.IsNullOrWhiteSpace(IncidentForm.Title) &&
               !string.IsNullOrWhiteSpace(IncidentForm.Description) &&
               !string.IsNullOrWhiteSpace(IncidentForm.ReporterEmail) &&
               !string.IsNullOrWhiteSpace(IncidentForm.ReporterName);
    }

    private List<string> ValidateForm()
    {
        var errors = new List<string>();
        var context = new ValidationContext(IncidentForm);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(IncidentForm, context, results, true))
        {
            errors.AddRange(results.Select(r => r.ErrorMessage ?? "Validation error"));
        }

        return errors;
    }

    private void PopulateDeviceInformation()
    {
        try
        {
            // Simulate device information for console application
            IncidentForm.DeviceInfo = "Desktop Console Application";
            IncidentForm.OperatingSystem = Environment.OSVersion.ToString();
            IncidentForm.BrowserVersion = "Console Demo v1.0";
        }
        catch
        {
            // Fallback if device info is not available
            IncidentForm.DeviceInfo = "Unknown Device";
            IncidentForm.OperatingSystem = "Unknown OS";
            IncidentForm.BrowserVersion = "Console Demo v1.0";
        }
    }

    // Property changed notifications for form validation
    partial void OnIncidentFormChanged(IncidentFormModel value)
    {
        SubmitFormCommand.NotifyCanExecuteChanged();
    }
}