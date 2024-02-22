using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class PoweringSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _startFrequency,
                  _endFrequency,
                  _startAmplitude,
                  _secondaryFrequencyFactor,
                  _secondaryAmplitude,
                  _initDelay,
                  _initLength,
                  _poweringUpDelay,
                  _poweringUpLength,
                  _shuttingDownDelay,
                  _shuttingDownLength;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Powering.Volume                   = Volume;
        Settings.Powering.Enabled                  = Enabled;
        Settings.Powering.StartFrequency           = StartFrequency;
        Settings.Powering.EndFrequency             = EndFrequency;
        Settings.Powering.StartAmplitude           = StartAmplitude;
        Settings.Powering.SecondaryFrequencyFactor = SecondaryFrequencyFactor;
        Settings.Powering.SecondaryAmplitude       = SecondaryAmplitude;
        Settings.Powering.InitDelay                = InitDelay;
        Settings.Powering.InitLength               = InitLength;
        Settings.Powering.PoweringUpDelay          = PoweringUpDelay;
        Settings.Powering.PoweringUpLength         = PoweringUpLength;
        Settings.Powering.ShuttingDownDelay        = ShuttingDownDelay;
        Settings.Powering.ShuttingDownLength       = ShuttingDownLength;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume                   = s.Powering.Volume;
        Enabled                  = s.Powering.Enabled;
        StartFrequency           = s.Powering.StartFrequency;
        EndFrequency             = s.Powering.EndFrequency;
        StartAmplitude           = s.Powering.StartAmplitude;
        SecondaryFrequencyFactor = s.Powering.SecondaryFrequencyFactor;
        SecondaryAmplitude       = s.Powering.SecondaryAmplitude;
        InitDelay                = s.Powering.InitDelay;
        InitLength               = s.Powering.InitLength;
        PoweringUpDelay          = s.Powering.PoweringUpDelay;
        PoweringUpLength         = s.Powering.PoweringUpLength;
        ShuttingDownDelay        = s.Powering.ShuttingDownDelay;
        ShuttingDownLength       = s.Powering.ShuttingDownLength;
    }
}