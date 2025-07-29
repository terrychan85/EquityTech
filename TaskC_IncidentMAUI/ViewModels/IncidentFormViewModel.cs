using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TaskC_IncidentMAUI.Models;
using TaskC_IncidentMAUI.Services;

namespace TaskC_IncidentMAUI.ViewModels
{
    public partial class IncidentFormViewModel : ObservableObject
    {
        private readonly IIncidentApiService _apiService;

        public IncidentFormViewModel(IIncidentApiService apiService)
        {
            _apiService = apiService;
            FormData = new IncidentFormModel();
            
            // Initialize collections for dropdowns
            PriorityOptions = new ObservableCollection<string> { "Low", "Medium", "High", "Critical" };
            CategoryOptions = new ObservableCollection<string> { "General", "Technical", "Security", "Hardware", "Software" };
            
            ValidationErrors = new ObservableCollection<string>();
        }

        [ObservableProperty]
        private IncidentFormModel formData;

        [ObservableProperty]
        private bool isSubmitting;

        [ObservableProperty]
        private bool isFormValid;

        [ObservableProperty]
        private string submitMessage = string.Empty;

        [ObservableProperty]
        private bool hasSubmitted;

        public ObservableCollection<string> PriorityOptions { get; }
        public ObservableCollection<string> CategoryOptions { get; }
        public ObservableCollection<string> ValidationErrors { get; }

        [RelayCommand]
        private async Task SubmitFormAsync()
        {
            if (!ValidateForm())
            {
                SubmitMessage = "Please fix validation errors before submitting.";
                return;
            }

            IsSubmitting = true;
            SubmitMessage = "Submitting incident...";

            try
            {
                bool success = await _apiService.SubmitIncidentAsync(FormData);
                
                if (success)
                {
                    SubmitMessage = "Incident submitted successfully!";
                    HasSubmitted = true;
                    // Reset form after successful submission
                    ResetForm();
                }
                else
                {
                    SubmitMessage = "Failed to submit incident. Please try again.";
                }
            }
            catch (Exception ex)
            {
                SubmitMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        [RelayCommand]
        private async Task CheckApiStatusAsync()
        {
            try
            {
                string status = await _apiService.GetSubmissionStatusAsync();
                SubmitMessage = $"API Status: {status}";
            }
            catch (Exception ex)
            {
                SubmitMessage = $"Error checking API status: {ex.Message}";
            }
        }

        [RelayCommand]
        private void ResetForm()
        {
            FormData = new IncidentFormModel();
            ValidationErrors.Clear();
            SubmitMessage = string.Empty;
            HasSubmitted = false;
            IsFormValid = false;
        }

        [RelayCommand]
        private void ValidateFormCommand()
        {
            ValidateForm();
        }

        private bool ValidateForm()
        {
            ValidationErrors.Clear();
            var context = new ValidationContext(FormData);
            var results = new List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(FormData, context, results, true);
            
            foreach (var result in results)
            {
                if (result.ErrorMessage != null)
                {
                    ValidationErrors.Add(result.ErrorMessage);
                }
            }

            IsFormValid = isValid && ValidationErrors.Count == 0;
            return IsFormValid;
        }

        partial void OnFormDataChanged(IncidentFormModel value)
        {
            // Auto-validate when form data changes
            ValidateForm();
        }
    }
}