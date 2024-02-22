using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class FootstepSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _length, _attackEnd, _decayStart, _speedFactor, _massFactor;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Footsteps.Volume      = Volume;
        Settings.Footsteps.Enabled     = Enabled;
        Settings.Footsteps.Frequency   = Frequency;
        Settings.Footsteps.Length      = Length;
        Settings.Footsteps.AttackEnd   = AttackEnd;
        Settings.Footsteps.DecayStart  = DecayStart;
        Settings.Footsteps.SpeedFactor = SpeedFactor;
        Settings.Footsteps.MassFactor  = MassFactor;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume      = s.Footsteps.Volume;
        Enabled     = s.Footsteps.Enabled;
        Frequency   = s.Footsteps.Frequency;
        Length      = s.Footsteps.Length;
        AttackEnd   = s.Footsteps.AttackEnd;
        DecayStart  = s.Footsteps.DecayStart;
        SpeedFactor = s.Footsteps.SpeedFactor;
        MassFactor  = s.Footsteps.MassFactor;
    }
}