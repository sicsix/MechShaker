using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MechShakerEngine.Settings;
using MechShakerUI.Managers;
using MechShakerUI.ViewModels.SettingsViewModels;
using Serilog.Events;

namespace MechShakerUI.ViewModels;

internal partial class SettingsViewModel : SettingsViewModelBase
{
    private readonly SettingsManager _settingsManager;

    [ObservableProperty]
    private IEnumerable<string> _devices = Array.Empty<string>();

    [ObservableProperty]
    private string _outputDevice = "";

    [ObservableProperty]
    private int _latency;

    private readonly Timer _latencyUpdateTimer = new(500);

    public MasterSVM               Master               { get; } = new();
    public LasersSVM               Lasers               { get; } = new();
    public TAGSVM                  TAG                  { get; } = new();
    public FlamersSVM              Flamers              { get; } = new();
    public MachineGunsSVM          MachineGuns          { get; } = new();
    public AMSSVM                  AMS                  { get; } = new();
    public AutocannonsAndRiflesSVM AutocannonsAndRifles { get; } = new();
    public PPCsSVM                 PPCs                 { get; } = new();
    public MissilesSVM             Missiles             { get; } = new();
    public MeleeSVM                Melee                { get; } = new();
    public FootstepSVM             Footsteps            { get; } = new();
    public LandingImpactsSVM       LandingImpacts       { get; } = new();
    public LaserDamageSVM          LaserDamage          { get; } = new();
    public ProjectileDamageSVM     ProjectileDamage     { get; } = new();
    public MissileDamageSVM        MissileDamage        { get; } = new();
    public MeleeDamageSVM          MeleeDamage          { get; } = new();
    public ExplosionDamageSVM      ExplosionDamage      { get; } = new();
    public PartDestructionSVM      PartDestruction      { get; } = new();
    public TorsoTwistSVM           TorsoTwist           { get; } = new();
    public JumpJetsSVM             JumpJets             { get; } = new();
    public MASCSVM                 MASC                 { get; } = new();
    public DropshipSVM             Dropship             { get; } = new();
    public PoweringSVM             Powering             { get; } = new();

    [ObservableProperty]
    private bool _debugLogging;

    [ObservableProperty]
    private bool _advancedMode;

    public LowPassFilterSVM  LowPassLowPassFilter  { get; } = new();
    public HighPassFilterSVM HighPassLowPassFilter { get; } = new();

    private readonly List<SettingsViewModelBase> _svms;

    public SettingsViewModel(EngineManager engineManager, SettingsManager settingsManager)
    {
        EngineManager    = engineManager;
        _settingsManager = settingsManager;

        _svms = new List<SettingsViewModelBase>
        {
            Master,
            Lasers,
            TAG,
            Flamers,
            MachineGuns,
            AMS,
            AutocannonsAndRifles,
            PPCs,
            Missiles,
            Melee,
            Footsteps,
            LandingImpacts,
            LaserDamage,
            ProjectileDamage,
            MissileDamage,
            MeleeDamage,
            ExplosionDamage,
            PartDestruction,
            TorsoTwist,
            JumpJets,
            MASC,
            Dropship,
            Powering,
            LowPassLowPassFilter,
            HighPassLowPassFilter
        };

        foreach (var svm in _svms)
        {
            svm.Initialise(engineManager);
        }

        _latencyUpdateTimer.Elapsed   += LatencyUpdateTimerOnElapsed;
        _latencyUpdateTimer.AutoReset =  false;
        RevertCommand                 =  new RelayCommand(Revert);
        UndoCommand                   =  new RelayCommand(Undo);
        SaveCommand                   =  new RelayCommand(Save);
    }

    public void Initialise()
    {
        Devices = EngineManager.GetOutputDevices();
        LoadSettings(Settings);
    }

    protected override void LoadSettingsSupressed(Settings s)
    {
        OutputDevice = s.OutputDevice;
        Latency      = s.Latency;
        AdvancedMode = s.AdvancedMode;
        DebugLogging = s.DebugLogging;

        foreach (var svm in _svms)
        {
            svm.LoadSettings(s);
        }
    }

    partial void OnOutputDeviceChanged(string value)
    {
        Settings.OutputDevice = value;
        Logging.At("Settings").Debug("Output Device changed to '{Device}'", value);
        UpdateEngineSettings();
    }

    partial void OnLatencyChanged(int value)
    {
        Settings.Latency = value;
        if (SupressUpdates)
            return;
        _latencyUpdateTimer.Stop();
        _latencyUpdateTimer.Start();
    }

    private void LatencyUpdateTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Logging.At("Settings").Debug("Updating audio output latency...");
        UpdateEngineSettings();
    }

    partial void OnAdvancedModeChanged(bool value)
    {
        Settings.AdvancedMode = value;
        if (SupressUpdates)
            return;
        Logging.At("Settings").Debug("Advanced Mode {State}", value ? "Enabled" : "Disabled");
    }

    partial void OnDebugLoggingChanged(bool value)
    {
        Settings.DebugLogging = value;
        if (SupressUpdates)
            return;
        if (!value)
            Logging.At("Settings").Debug("Debug Logging Disabled");
        Logging.LevelSwitch.MinimumLevel = value ? LogEventLevel.Verbose : LogEventLevel.Information;
        if (value)
            Logging.At("Settings").Debug("Debug Logging Enabled");
    }

    public ICommand RevertCommand { get; }

    private void Revert()
    {
        _settingsManager.RevertSettings();
        LoadSettings(Settings);
        EngineManager.UpdateEngineSettings();
    }

    public ICommand UndoCommand { get; }

    private void Undo()
    {
        _settingsManager.UndoSettings();
        LoadSettings(Settings);
        EngineManager.UpdateEngineSettings();
    }

    public ICommand SaveCommand { get; }

    private void Save()
    {
        _settingsManager.SaveSettings();
    }
}