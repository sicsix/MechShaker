using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MeleeDamageSVM : SettingsViewModelBase
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
        Settings.MeleeDamage.Volume            = Volume;
        Settings.MeleeDamage.Enabled           = Enabled;
        Settings.MeleeDamage.Frequency         = Frequency;
        Settings.MeleeDamage.AttackEnd         = AttackEnd;
        Settings.MeleeDamage.DecayStart        = DecayStart;
        Settings.MeleeDamage.MaxLength         = MaxLength;
        Settings.MeleeDamage.AmplitudeExponent = AmplitudeExponent;
        Settings.MeleeDamage.LengthExponent    = LengthExponent;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume            = s.MeleeDamage.Volume;
        Enabled           = s.MeleeDamage.Enabled;
        Frequency         = s.MeleeDamage.Frequency;
        AttackEnd         = s.MeleeDamage.AttackEnd;
        DecayStart        = s.MeleeDamage.DecayStart;
        MaxLength         = s.MeleeDamage.MaxLength;
        AmplitudeExponent = s.MeleeDamage.AmplitudeExponent;
        LengthExponent    = s.MeleeDamage.LengthExponent;
    }
}