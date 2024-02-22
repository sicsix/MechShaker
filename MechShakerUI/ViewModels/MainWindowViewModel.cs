using MechShakerUI.Managers;

namespace MechShakerUI.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public LogViewModel      LogViewModel      { get; private set; }
    public SettingsViewModel SettingsViewModel { get; private set; }

    public MainWindowViewModel(LogViewModel logViewModel, SettingsViewModel settingsViewModel)
    {
        LogViewModel      = logViewModel;
        SettingsViewModel = settingsViewModel;
    }

    public MainWindowViewModel()
    {
        LogViewModel = new LogViewModel(new LoggingManager());
        var settingsManager = new SettingsManager();
        SettingsViewModel = new SettingsViewModel(new EngineManager(settingsManager), settingsManager);
    }
}