using TaskC_IncidentMAUI.Models;

namespace TaskC_IncidentMAUI.Services
{
    public interface IIncidentApiService
    {
        Task<(bool Success, string Message)> SubmitIncidentAsync(IncidentFormModel formData);
    }
}