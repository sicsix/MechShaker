using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class LowPassFilterSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _frequency;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.LowPassFilter.Frequency = Frequency;
        Settings.LowPassFilter.Enabled   = Enabled;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Frequency = s.LowPassFilter.Frequency;
        Enabled   = s.LowPassFilter.Enabled;
    }
}