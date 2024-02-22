using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MeleeSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _hitFrequency,
                  _hitLength,
                  _hitAttackEnd,
                  _hitDecayStart,
                  _swingFrequency,
                  _swingLength,
                  _swingAttackEnd,
                  _swingDecayStart,
                  _swingAmplitude,
                  _massFactor;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Melee.Volume          = Volume;
        Settings.Melee.Enabled         = Enabled;
        Settings.Melee.HitFrequency    = HitFrequency;
        Settings.Melee.HitLength       = HitLength;
        Settings.Melee.HitAttackEnd    = HitAttackEnd;
        Settings.Melee.HitDecayStart   = HitDecayStart;
        Settings.Melee.SwingFrequency  = SwingFrequency;
        Settings.Melee.SwingLength     = SwingLength;
        Settings.Melee.SwingAttackEnd  = SwingAttackEnd;
        Settings.Melee.SwingDecayStart = SwingDecayStart;
        Settings.Melee.SwingAmplitude  = SwingAmplitude;
        Settings.Melee.MassFactor      = MassFactor;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume          = s.Melee.Volume;
        Enabled         = s.Melee.Enabled;
        HitFrequency    = s.Melee.HitFrequency;
        HitLength       = s.Melee.HitLength;
        HitAttackEnd    = s.Melee.HitAttackEnd;
        HitDecayStart   = s.Melee.HitDecayStart;
        SwingFrequency  = s.Melee.SwingFrequency;
        SwingLength     = s.Melee.SwingLength;
        SwingAttackEnd  = s.Melee.SwingAttackEnd;
        SwingDecayStart = s.Melee.SwingDecayStart;
        SwingAmplitude  = s.Melee.SwingAmplitude;
        MassFactor      = s.Melee.MassFactor;
    }
}