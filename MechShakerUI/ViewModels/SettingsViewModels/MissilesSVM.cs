using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MissilesSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _launchFrequency,
                  _launchAttackEnd,
                  _launchDecayStart,
                  _launchLength,
                  _tailFrequency,
                  _tailAmplitude,
                  _tailAttackEnd,
                  _tailDecayStart,
                  _tailLength,
                  _streamFactor;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Missiles.Volume           = Volume;
        Settings.Missiles.Enabled          = Enabled;
        Settings.Missiles.LaunchFrequency  = LaunchFrequency;
        Settings.Missiles.LaunchAttackEnd  = LaunchAttackEnd;
        Settings.Missiles.LaunchDecayStart = LaunchDecayStart;
        Settings.Missiles.LaunchLength     = LaunchLength;
        Settings.Missiles.TailFrequency    = TailFrequency;
        Settings.Missiles.TailAmplitude    = TailAmplitude;
        Settings.Missiles.TailAttackEnd    = TailAttackEnd;
        Settings.Missiles.TailDecayStart   = TailDecayStart;
        Settings.Missiles.TailLength       = TailLength;
        Settings.Missiles.StreamFactor     = StreamFactor;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume           = s.Missiles.Volume;
        Enabled          = s.Missiles.Enabled;
        LaunchFrequency  = s.Missiles.LaunchFrequency;
        LaunchAttackEnd  = s.Missiles.LaunchAttackEnd;
        LaunchDecayStart = s.Missiles.LaunchDecayStart;
        LaunchLength     = s.Missiles.LaunchLength;
        TailFrequency    = s.Missiles.TailFrequency;
        TailAmplitude    = s.Missiles.TailAmplitude;
        TailAttackEnd    = s.Missiles.TailAttackEnd;
        TailDecayStart   = s.Missiles.TailDecayStart;
        TailLength       = s.Missiles.TailLength;
        StreamFactor     = s.Missiles.StreamFactor;
    }
}