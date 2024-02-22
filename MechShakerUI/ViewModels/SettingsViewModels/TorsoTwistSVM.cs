using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class TorsoTwistSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume, _frequency, _transitionTime, _massFactor;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.TorsoTwist.Volume         = Volume;
        Settings.TorsoTwist.Enabled        = Enabled;
        Settings.TorsoTwist.Frequency      = Frequency;
        Settings.TorsoTwist.TransitionTime = TransitionTime;
        Settings.TorsoTwist.MassFactor     = MassFactor;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume         = s.TorsoTwist.Volume;
        Enabled        = s.TorsoTwist.Enabled;
        Frequency      = s.TorsoTwist.Frequency;
        TransitionTime = s.TorsoTwist.TransitionTime;
        MassFactor     = s.TorsoTwist.MassFactor;
    }
}