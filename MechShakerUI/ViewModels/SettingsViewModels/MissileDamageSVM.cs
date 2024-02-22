using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MissileDamageSVM : SettingsViewModelBase
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
        Settings.MissileDamage.Volume            = Volume;
        Settings.MissileDamage.Enabled           = Enabled;
        Settings.MissileDamage.Frequency         = Frequency;
        Settings.MissileDamage.AttackEnd         = AttackEnd;
        Settings.MissileDamage.DecayStart        = DecayStart;
        Settings.MissileDamage.MaxLength         = MaxLength;
        Settings.MissileDamage.AmplitudeExponent = AmplitudeExponent;
        Settings.MissileDamage.LengthExponent    = LengthExponent;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume            = s.MissileDamage.Volume;
        Enabled           = s.MissileDamage.Enabled;
        Frequency         = s.MissileDamage.Frequency;
        AttackEnd         = s.MissileDamage.AttackEnd;
        DecayStart        = s.MissileDamage.DecayStart;
        MaxLength         = s.MissileDamage.MaxLength;
        AmplitudeExponent = s.MissileDamage.AmplitudeExponent;
        LengthExponent    = s.MissileDamage.LengthExponent;
    }
}