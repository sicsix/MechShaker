using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class ProjectileDamageSVM : SettingsViewModelBase
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
        Settings.ProjectileDamage.Volume            = Volume;
        Settings.ProjectileDamage.Enabled           = Enabled;
        Settings.ProjectileDamage.Frequency         = Frequency;
        Settings.ProjectileDamage.AttackEnd         = AttackEnd;
        Settings.ProjectileDamage.DecayStart        = DecayStart;
        Settings.ProjectileDamage.MaxLength         = MaxLength;
        Settings.ProjectileDamage.AmplitudeExponent = AmplitudeExponent;
        Settings.ProjectileDamage.LengthExponent    = LengthExponent;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume            = s.ProjectileDamage.Volume;
        Enabled           = s.ProjectileDamage.Enabled;
        Frequency         = s.ProjectileDamage.Frequency;
        AttackEnd         = s.ProjectileDamage.AttackEnd;
        DecayStart        = s.ProjectileDamage.DecayStart;
        MaxLength         = s.ProjectileDamage.MaxLength;
        AmplitudeExponent = s.ProjectileDamage.AmplitudeExponent;
        LengthExponent    = s.ProjectileDamage.LengthExponent;
    }
}