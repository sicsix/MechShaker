using System;
using System.Collections.Generic;
using MechShakerEngine;
using MechShakerEngine.Settings;

namespace MechShakerUI.Managers;

internal class EngineManager : IDisposable
{
    private readonly SettingsManager _settingsManager;
    private readonly Engine          _engine = new();

    public EngineManager(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public static IEnumerable<string> GetOutputDevices()
    {
        return Engine.GetOutputDevices();
    }

    public Settings Settings => _settingsManager.Settings;

    public void Dispose()
    {
        _engine.Dispose();
    }

    public void Initialise()
    {
        _engine.Initialise();
        _engine.BeginListening();
        UpdateEngineSettings();
    }

    public void UpdateEngineSettings()
    {
        _engine.UpdateSettings(_settingsManager.Settings);
    }
}