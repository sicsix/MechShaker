using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MechShakerUI.Managers;
using MechShakerUI.ViewModels;
using MechShakerUI.Views;
using Serilog;

namespace MechShakerUI;

public class App : Application
{
    private const string Version = "1.00";

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var loggingManager = new LoggingManager();
            var logViewModel   = new LogViewModel(loggingManager);

            Logging.At(this).Information("MechShaker {Version} by sicsix", Version);

            var settingsManager = new SettingsManager();
            var engineManager   = new EngineManager(settingsManager);

            desktop.ShutdownRequested += (_, _) =>
            {
                engineManager.Dispose();
                Log.CloseAndFlush();
            };

            settingsManager.Initalise();
            engineManager.Initialise();
            var settingsViewModel = new SettingsViewModel(engineManager, settingsManager);
            settingsViewModel.Initialise();

            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(logViewModel, settingsViewModel) };
        }

        base.OnFrameworkInitializationCompleted();
    }
}