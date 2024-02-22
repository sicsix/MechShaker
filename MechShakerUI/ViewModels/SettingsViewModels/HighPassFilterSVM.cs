using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class HighPassFilterSVM : SettingsViewModelBase
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
        Settings.HighPassFilter.Frequency = Frequency;
        Settings.HighPassFilter.Enabled   = Enabled;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Frequency = s.HighPassFilter.Frequency;
        Enabled   = s.HighPassFilter.Enabled;
    }
}