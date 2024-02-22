using MechShakerEngine.Settings;
using MechShakerUI.Managers;

namespace MechShakerUI.ViewModels;

internal abstract class SettingsViewModelBase : ViewModelBase
{
    protected EngineManager EngineManager = null!;
    protected Settings      Settings => EngineManager.Settings;
    protected bool          SupressUpdates;

    public void Initialise(EngineManager engineManager)
    {
        EngineManager = engineManager;
    }

    protected void UpdateEngineSettings()
    {
        if (!SupressUpdates)
            EngineManager.UpdateEngineSettings();
    }

    public void LoadSettings(Settings s)
    {
        SupressUpdates = true;
        LoadSettingsSupressed(s);
        SupressUpdates = false;
    }

    protected abstract void LoadSettingsSupressed(Settings s);
}