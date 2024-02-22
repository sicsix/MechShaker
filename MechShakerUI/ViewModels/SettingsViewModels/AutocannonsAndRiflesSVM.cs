using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class AutocannonsAndRiflesSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _minFrequency, _maxFrequency, _minAmplitude, _minLength, _maxLength, _attackEnd, _decayStart;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.AutocannonsAndRifles.Volume       = Volume;
        Settings.AutocannonsAndRifles.Enabled      = Enabled;
        Settings.AutocannonsAndRifles.MinFrequency = MinFrequency;
        Settings.AutocannonsAndRifles.MaxFrequency = MaxFrequency;
        Settings.AutocannonsAndRifles.MinAmplitude = MinAmplitude;
        Settings.AutocannonsAndRifles.MinLength    = MinLength;
        Settings.AutocannonsAndRifles.MaxLength    = MaxLength;
        Settings.AutocannonsAndRifles.AttackEnd    = AttackEnd;
        Settings.AutocannonsAndRifles.DecayStart   = DecayStart;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume       = s.AutocannonsAndRifles.Volume;
        Enabled      = s.AutocannonsAndRifles.Enabled;
        MinFrequency = s.AutocannonsAndRifles.MinFrequency;
        MaxFrequency = s.AutocannonsAndRifles.MaxFrequency;
        MinAmplitude = s.AutocannonsAndRifles.MinAmplitude;
        MinLength    = s.AutocannonsAndRifles.MinLength;
        MaxLength    = s.AutocannonsAndRifles.MaxLength;
        AttackEnd    = s.AutocannonsAndRifles.AttackEnd;
        DecayStart   = s.AutocannonsAndRifles.DecayStart;
    }
}