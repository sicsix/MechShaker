using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MechShakerEngine.Settings;

namespace MechShakerUI.ViewModels.SettingsViewModels;

internal partial class MasterSVM : SettingsViewModelBase
{
    [ObservableProperty]
    private float _volume;

    [ObservableProperty]
    private bool _enabled;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (SupressUpdates)
            return;
        Settings.Master.Volume  = Volume;
        Settings.Master.Enabled = Enabled;
        UpdateEngineSettings();
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        Volume  = s.Master.Volume;
        Enabled = s.Master.Enabled;
    }
}