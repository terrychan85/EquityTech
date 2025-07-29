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
            
            // Initialize collections first
            SeverityOptions = new ObservableCollection<string> { "Low", "Medium", "High", "Critical" };
            ValidationErrors = new ObservableCollection<string>();
            
            // Initialize form data after collections are ready
            FormData = new IncidentFormModel();
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

        public ObservableCollection<string> SeverityOptions { get; }
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
                var result = await _apiService.SubmitIncidentAsync(FormData);
                
                if (result.Success)
                {
                    SubmitMessage = result.Message;
                    HasSubmitted = true;
                }
                else
                {
                    SubmitMessage = result.Message;
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
        private void ResetForm()
        {
            FormData = new IncidentFormModel();
            ValidationErrors?.Clear();
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
            // Ensure ValidationErrors is initialized
            if (ValidationErrors == null || FormData == null)
                return false;
                
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
            // Auto-validate when form data changes, but only if everything is initialized
            if (ValidationErrors != null && value != null)
            {
                ValidateForm();
            }
        }
    }
}