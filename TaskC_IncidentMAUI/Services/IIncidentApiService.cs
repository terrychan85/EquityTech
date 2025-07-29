using TaskC_IncidentMAUI.Models;

namespace TaskC_IncidentMAUI.Services
{
    public interface IIncidentApiService
    {
        Task<bool> SubmitIncidentAsync(IncidentFormModel formData);
        Task<string> GetSubmissionStatusAsync();
    }
}