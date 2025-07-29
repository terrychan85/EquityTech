using TaskC_IncidentMAUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace TaskC_IncidentMAUI.Views
{
    public partial class IncidentFormPage : ContentPage
    {
        public IncidentFormPage(IncidentFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Parameterless constructor for XAML instantiation
        public IncidentFormPage()
        {
            InitializeComponent();
            
            // Manually resolve ViewModel from service provider
            if (Application.Current?.Handler?.MauiContext?.Services != null)
            {
                var viewModel = Application.Current.Handler.MauiContext.Services.GetService<IncidentFormViewModel>();
                BindingContext = viewModel;
            }
        }
    }
}