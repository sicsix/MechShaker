using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class ExplosionDamageSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _attackEnd, _decayStart, _maxLength, _amplitudeExponent, _lengthExponent;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.ExplosionDamage.Volume            = Volume;
        Settings.ExplosionDamage.Enabled           = Enabled;
        Settings.ExplosionDamage.Frequency         = Frequency;
        Settings.ExplosionDamage.AttackEnd         = AttackEnd;
        Settings.ExplosionDamage.DecayStart        = DecayStart;
        Settings.ExplosionDamage.MaxLength         = MaxLength;
        Settings.ExplosionDamage.AmplitudeExponent = AmplitudeExponent;
        Settings.ExplosionDamage.LengthExponent    = LengthExponent;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume            = s.ExplosionDamage.Volume;
        Enabled           = s.ExplosionDamage.Enabled;
        Frequency         = s.ExplosionDamage.Frequency;
        AttackEnd         = s.ExplosionDamage.AttackEnd;
        DecayStart        = s.ExplosionDamage.DecayStart;
        MaxLength         = s.ExplosionDamage.MaxLength;
        AmplitudeExponent = s.ExplosionDamage.AmplitudeExponent;
        LengthExponent    = s.ExplosionDamage.LengthExponent;
    }
}