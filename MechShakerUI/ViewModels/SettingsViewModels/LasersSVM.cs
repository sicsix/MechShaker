using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class LasersSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _secondaryFrequency, _secondaryAmplitude, _amplitudeExponent;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Lasers.Volume             = Volume;
        Settings.Lasers.Enabled            = Enabled;
        Settings.Lasers.Frequency          = Frequency;
        Settings.Lasers.SecondaryFrequency = SecondaryFrequency;
        Settings.Lasers.SecondaryAmplitude = SecondaryAmplitude;
        Settings.Lasers.AmplitudeExponent  = AmplitudeExponent;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.Lasers.Volume;
        Enabled            = s.Lasers.Enabled;
        Frequency          = s.Lasers.Frequency;
        SecondaryFrequency = s.Lasers.SecondaryFrequency;
        SecondaryAmplitude = s.Lasers.SecondaryAmplitude;
        AmplitudeExponent  = s.Lasers.AmplitudeExponent;
    }
}