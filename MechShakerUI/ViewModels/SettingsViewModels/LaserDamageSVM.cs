using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class LaserDamageSVM : SettingsViewModelBase
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
        Settings.LaserDamage.Volume             = Volume;
        Settings.LaserDamage.Enabled            = Enabled;
        Settings.LaserDamage.Frequency          = Frequency;
        Settings.LaserDamage.SecondaryFrequency = SecondaryFrequency;
        Settings.LaserDamage.SecondaryAmplitude = SecondaryAmplitude;
        Settings.LaserDamage.AmplitudeExponent  = AmplitudeExponent;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.LaserDamage.Volume;
        Enabled            = s.LaserDamage.Enabled;
        Frequency          = s.LaserDamage.Frequency;
        SecondaryFrequency = s.LaserDamage.SecondaryFrequency;
        SecondaryAmplitude = s.LaserDamage.SecondaryAmplitude;
        AmplitudeExponent  = s.LaserDamage.AmplitudeExponent;
    }
}