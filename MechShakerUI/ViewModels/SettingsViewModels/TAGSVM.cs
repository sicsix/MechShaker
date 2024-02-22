using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class TAGSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _secondaryFrequency, _secondaryAmplitude;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.TAG.Volume             = Volume;
        Settings.TAG.Enabled            = Enabled;
        Settings.TAG.Frequency          = Frequency;
        Settings.TAG.SecondaryFrequency = SecondaryFrequency;
        Settings.TAG.SecondaryAmplitude = SecondaryAmplitude;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.TAG.Volume;
        Enabled            = s.TAG.Enabled;
        Frequency          = s.TAG.Frequency;
        SecondaryFrequency = s.TAG.SecondaryFrequency;
        SecondaryAmplitude = s.TAG.SecondaryAmplitude;
    }
}