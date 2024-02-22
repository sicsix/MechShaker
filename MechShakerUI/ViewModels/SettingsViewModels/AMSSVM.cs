using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class AMSSVM : SettingsViewModelBase
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
        Settings.AMS.Volume     = Volume;
        Settings.AMS.Enabled    = Enabled;
        Settings.AMS.Frequency  = Frequency;
        Settings.AMS.Length     = Length;
        Settings.AMS.AttackEnd  = AttackEnd;
        Settings.AMS.DecayStart = DecayStart;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume     = s.AMS.Volume;
        Enabled    = s.AMS.Enabled;
        Frequency  = s.AMS.Frequency;
        Length     = s.AMS.Length;
        AttackEnd  = s.AMS.AttackEnd;
        DecayStart = s.AMS.DecayStart;
    }
}