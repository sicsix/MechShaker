using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MASCSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _secondaryFrequency, _secondaryAmplitude, _gaugeExponent;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.MASC.Volume             = Volume;
        Settings.MASC.Enabled            = Enabled;
        Settings.MASC.Frequency          = Frequency;
        Settings.MASC.SecondaryFrequency = SecondaryFrequency;
        Settings.MASC.SecondaryAmplitude = SecondaryAmplitude;
        Settings.MASC.GaugeExponent      = GaugeExponent;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.MASC.Volume;
        Enabled            = s.MASC.Enabled;
        Frequency          = s.MASC.Frequency;
        SecondaryFrequency = s.MASC.SecondaryFrequency;
        SecondaryAmplitude = s.MASC.SecondaryAmplitude;
        GaugeExponent      = s.MASC.GaugeExponent;
    }
}