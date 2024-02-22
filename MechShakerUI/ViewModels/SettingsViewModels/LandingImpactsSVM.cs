using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class LandingImpactsSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _length, _attackEnd, _decayStart;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.LandingImpacts.Volume     = Volume;
        Settings.LandingImpacts.Enabled    = Enabled;
        Settings.LandingImpacts.Frequency  = Frequency;
        Settings.LandingImpacts.Length     = Length;
        Settings.LandingImpacts.AttackEnd  = AttackEnd;
        Settings.LandingImpacts.DecayStart = DecayStart;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume     = s.LandingImpacts.Volume;
        Enabled    = s.LandingImpacts.Enabled;
        Frequency  = s.LandingImpacts.Frequency;
        Length     = s.LandingImpacts.Length;
        AttackEnd  = s.LandingImpacts.AttackEnd;
        DecayStart = s.LandingImpacts.DecayStart;
    }
}