using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class PPCsSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume,
                  _popFrequency,
                  _popAttackEnd,
                  _popDecayStart,
                  _popLength,
                  _tailFrequency,
                  _tailAmplitude,
                  _tailAttackEnd,
                  _tailDecayStart,
                  _tailLength;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.PPCs.Volume         = Volume;
        Settings.PPCs.Enabled        = Enabled;
        Settings.PPCs.PopFrequency   = PopFrequency;
        Settings.PPCs.PopAttackEnd   = PopAttackEnd;
        Settings.PPCs.PopDecayStart  = PopDecayStart;
        Settings.PPCs.PopLength      = PopLength;
        Settings.PPCs.TailFrequency  = TailFrequency;
        Settings.PPCs.TailAmplitude  = TailAmplitude;
        Settings.PPCs.TailAttackEnd  = TailAttackEnd;
        Settings.PPCs.TailDecayStart = TailDecayStart;
        Settings.PPCs.TailLength     = TailLength;
        UpdateEngineSettings();
    }


    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume         = s.PPCs.Volume;
        Enabled        = s.PPCs.Enabled;
        PopFrequency   = s.PPCs.PopFrequency;
        PopAttackEnd   = s.PPCs.PopAttackEnd;
        PopDecayStart  = s.PPCs.PopDecayStart;
        PopLength      = s.PPCs.PopLength;
        TailFrequency  = s.PPCs.TailFrequency;
        TailAmplitude  = s.PPCs.TailAmplitude;
        TailAttackEnd  = s.PPCs.TailAttackEnd;
        TailDecayStart = s.PPCs.TailDecayStart;
        TailLength     = s.PPCs.TailLength;
    }
}