using System.ComponentModel.DataAnnotations;

namespace TaskC_IncidentMAUI.Models
{
    public class IncidentFormModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reporter name is required")]
        [StringLength(50, ErrorMessage = "Reporter name cannot exceed 50 characters")]
        public string ReporterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority must be selected")]
        public string Priority { get; set; } = "Medium";

        [Required(ErrorMessage = "Category must be selected")]
        public string Category { get; set; } = "General";

        public DateTime DateReported { get; set; } = DateTime.Now;

        public string Location { get; set; } = string.Empty;

        public bool IsUrgent { get; set; } = false;
    }
}