using TaskC_IncidentMAUI.ViewModels;

namespace TaskC_IncidentMAUI.Views
{
    public partial class IncidentFormPage : ContentPage
    {
        public IncidentFormPage(IncidentFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}