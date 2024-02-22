using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class FlamersSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _secondaryFrequency, _secondaryAmplitude, _tertiaryFrequency, _tertiaryAmplitude;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Flamers.Volume             = Volume;
        Settings.Flamers.Enabled            = Enabled;
        Settings.Flamers.Frequency          = Frequency;
        Settings.Flamers.SecondaryFrequency = SecondaryFrequency;
        Settings.Flamers.SecondaryAmplitude = SecondaryAmplitude;
        Settings.Flamers.TertiaryFrequency  = TertiaryFrequency;
        Settings.Flamers.TertiaryAmplitude  = TertiaryAmplitude;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.Flamers.Volume;
        Enabled            = s.Flamers.Enabled;
        Frequency          = s.Flamers.Frequency;
        SecondaryFrequency = s.Flamers.SecondaryFrequency;
        SecondaryAmplitude = s.Flamers.SecondaryAmplitude;
        TertiaryFrequency  = s.Flamers.TertiaryFrequency;
        TertiaryAmplitude  = s.Flamers.TertiaryAmplitude;
    }
}