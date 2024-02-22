using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class PartDestructionSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _frequency,
                  _length,
                  _attackEnd,
                  _decayStart,
                  _secondaryFrequency,
                  _secondaryAmplitude,
                  _secondaryLength,
                  _secondaryAttackEnd,
                  _secondaryDecayStart,
                  _headFactor,
                  _centerTorsoFactor,
                  _sideTorsoFactor,
                  _armFactor,
                  _legFactor;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.PartDestruction.Volume              = Volume;
        Settings.PartDestruction.Enabled             = Enabled;
        Settings.PartDestruction.Frequency           = Frequency;
        Settings.PartDestruction.Length              = Length;
        Settings.PartDestruction.AttackEnd           = AttackEnd;
        Settings.PartDestruction.DecayStart          = DecayStart;
        Settings.PartDestruction.SecondaryFrequency  = SecondaryFrequency;
        Settings.PartDestruction.SecondaryAmplitude  = SecondaryAmplitude;
        Settings.PartDestruction.SecondaryLength     = SecondaryLength;
        Settings.PartDestruction.SecondaryAttackEnd  = SecondaryAttackEnd;
        Settings.PartDestruction.SecondaryDecayStart = SecondaryDecayStart;
        Settings.PartDestruction.HeadFactor          = HeadFactor;
        Settings.PartDestruction.CenterTorsoFactor   = CenterTorsoFactor;
        Settings.PartDestruction.SideTorsoFactor     = SideTorsoFactor;
        Settings.PartDestruction.ArmFactor           = ArmFactor;
        Settings.PartDestruction.LegFactor           = LegFactor;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume              = s.PartDestruction.Volume;
        Enabled             = s.PartDestruction.Enabled;
        Frequency           = s.PartDestruction.Frequency;
        Length              = s.PartDestruction.Length;
        AttackEnd           = s.PartDestruction.AttackEnd;
        DecayStart          = s.PartDestruction.DecayStart;
        SecondaryFrequency  = s.PartDestruction.SecondaryFrequency;
        SecondaryAmplitude  = s.PartDestruction.SecondaryAmplitude;
        SecondaryLength     = s.PartDestruction.SecondaryLength;
        SecondaryAttackEnd  = s.PartDestruction.SecondaryAttackEnd;
        SecondaryDecayStart = s.PartDestruction.SecondaryDecayStart;
        HeadFactor          = s.PartDestruction.HeadFactor;
        CenterTorsoFactor   = s.PartDestruction.CenterTorsoFactor;
        SideTorsoFactor     = s.PartDestruction.SideTorsoFactor;
        ArmFactor           = s.PartDestruction.ArmFactor;
        LegFactor           = s.PartDestruction.LegFactor;
    }
}