using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class JumpJetsSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _frequency,
                  _secondaryFrequency,
                  _secondaryAmplitude,
                  _tertiaryFrequency,
                  _tertiaryAmplitude,
                  _transitionOnTime,
                  _transitionOffTime;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.JumpJets.Volume             = Volume;
        Settings.JumpJets.Enabled            = Enabled;
        Settings.JumpJets.Frequency          = Frequency;
        Settings.JumpJets.SecondaryFrequency = SecondaryFrequency;
        Settings.JumpJets.SecondaryAmplitude = SecondaryAmplitude;
        Settings.JumpJets.TertiaryFrequency  = TertiaryFrequency;
        Settings.JumpJets.TertiaryAmplitude  = TertiaryAmplitude;
        Settings.JumpJets.TransitionOnTime   = TransitionOnTime;
        Settings.JumpJets.TransitionOffTime  = TransitionOffTime;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume             = s.JumpJets.Volume;
        Enabled            = s.JumpJets.Enabled;
        Frequency          = s.JumpJets.Frequency;
        SecondaryFrequency = s.JumpJets.SecondaryFrequency;
        SecondaryAmplitude = s.JumpJets.SecondaryAmplitude;
        TertiaryFrequency  = s.JumpJets.TertiaryFrequency;
        TertiaryAmplitude  = s.JumpJets.TertiaryAmplitude;
        TransitionOnTime   = s.JumpJets.TransitionOnTime;
        TransitionOffTime  = s.JumpJets.TransitionOffTime;
    }
}