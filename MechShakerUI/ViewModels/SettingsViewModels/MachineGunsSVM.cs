using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MachineGunsSVM : SettingsViewModelBase
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
        Settings.MachineGuns.Volume     = Volume;
        Settings.MachineGuns.Enabled    = Enabled;
        Settings.MachineGuns.Frequency  = Frequency;
        Settings.MachineGuns.Length     = Length;
        Settings.MachineGuns.AttackEnd  = AttackEnd;
        Settings.MachineGuns.DecayStart = DecayStart;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume     = s.MachineGuns.Volume;
        Enabled    = s.MachineGuns.Enabled;
        Frequency  = s.MachineGuns.Frequency;
        Length     = s.MachineGuns.Length;
        AttackEnd  = s.MachineGuns.AttackEnd;
        DecayStart = s.MachineGuns.DecayStart;
    }
}