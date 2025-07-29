using System.ComponentModel.DataAnnotations;

namespace TaskC_IncidentMAUI.Models
{
    public class IncidentFormModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Severity must be selected")]
        public string Severity { get; set; } = "Medium";
    }
}