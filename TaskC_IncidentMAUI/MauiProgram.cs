using Microsoft.Extensions.Logging;
using TaskC_IncidentMAUI.Services;
using TaskC_IncidentMAUI.ViewModels;
using TaskC_IncidentMAUI.Views;

namespace TaskC_IncidentMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<MockApiServerService>();
            builder.Services.AddSingleton<IIncidentApiService, IncidentApiService>();

            // Register ViewModels
            builder.Services.AddTransient<IncidentFormViewModel>();

            // Register Views
            builder.Services.AddTransient<IncidentFormPage>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddLogging(configure => configure.AddDebug());

            return builder.Build();
        }
    }
}
