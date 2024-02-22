using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class DropshipSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _flightFrequency,
                  _flightAmplitude,
                  _flightSecondaryFrequency,
                  _flightSecondaryAmplitude,
                  _flightTertiaryFrequency,
                  _flightTertiaryAmplitude,
                  _flightTransitionOff,
                  _landedFrequency,
                  _landedAmplitude,
                  _landedLength,
                  _landedAttackEnd,
                  _landedDecayStart,
                  _turntableFrequency,
                  _turntableAmplitude,
                  _turntableSecondaryFrequency,
                  _turntableSecondaryAmplitude,
                  _turntableTransitionTime;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Dropship.Volume                      = Volume;
        Settings.Dropship.Enabled                     = Enabled;
        Settings.Dropship.FlightFrequency             = FlightFrequency;
        Settings.Dropship.FlightAmplitude             = FlightAmplitude;
        Settings.Dropship.FlightSecondaryFrequency    = FlightSecondaryFrequency;
        Settings.Dropship.FlightSecondaryAmplitude    = FlightSecondaryAmplitude;
        Settings.Dropship.FlightTertiaryFrequency     = FlightTertiaryFrequency;
        Settings.Dropship.FlightTertiaryAmplitude     = FlightTertiaryAmplitude;
        Settings.Dropship.FlightTransitionOff         = FlightTransitionOff;
        Settings.Dropship.LandedFrequency             = LandedFrequency;
        Settings.Dropship.LandedAmplitude             = LandedAmplitude;
        Settings.Dropship.LandedLength                = LandedLength;
        Settings.Dropship.LandedAttackEnd             = LandedAttackEnd;
        Settings.Dropship.LandedDecayStart            = LandedDecayStart;
        Settings.Dropship.TurntableFrequency          = TurntableFrequency;
        Settings.Dropship.TurntableAmplitude          = TurntableAmplitude;
        Settings.Dropship.TurntableSecondaryFrequency = TurntableSecondaryFrequency;
        Settings.Dropship.TurntableSecondaryAmplitude = TurntableSecondaryAmplitude;
        Settings.Dropship.TurntableTransitionTime     = TurntableTransitionTime;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume                      = s.Dropship.Volume;
        Enabled                     = s.Dropship.Enabled;
        FlightFrequency             = s.Dropship.FlightFrequency;
        FlightAmplitude             = s.Dropship.FlightAmplitude;
        FlightSecondaryFrequency    = s.Dropship.FlightSecondaryFrequency;
        FlightSecondaryAmplitude    = s.Dropship.FlightSecondaryAmplitude;
        FlightTertiaryFrequency     = s.Dropship.FlightTertiaryFrequency;
        FlightTertiaryAmplitude     = s.Dropship.FlightTertiaryAmplitude;
        FlightTransitionOff         = s.Dropship.FlightTransitionOff;
        LandedFrequency             = s.Dropship.LandedFrequency;
        LandedAmplitude             = s.Dropship.LandedAmplitude;
        LandedLength                = s.Dropship.LandedLength;
        LandedAttackEnd             = s.Dropship.LandedAttackEnd;
        LandedDecayStart            = s.Dropship.LandedDecayStart;
        TurntableFrequency          = s.Dropship.TurntableFrequency;
        TurntableAmplitude          = s.Dropship.TurntableAmplitude;
        TurntableSecondaryFrequency = s.Dropship.TurntableSecondaryFrequency;
        TurntableSecondaryAmplitude = s.Dropship.TurntableSecondaryAmplitude;
        TurntableTransitionTime     = s.Dropship.TurntableTransitionTime;
    }
}