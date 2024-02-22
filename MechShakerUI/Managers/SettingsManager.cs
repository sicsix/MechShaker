using System;
using System.IO;
using Serilog.Events;
using YamlDotNet.Serialization;
using Settings = MechShakerEngine.Settings.Settings;


namespace MechShakerUI.Managers;

internal class SettingsManager
{
    internal Settings Settings = new();

    public void Initalise()
    {
        Settings                         = LoadSettings(@"\Settings.yaml");
        Logging.LevelSwitch.MinimumLevel = Settings.DebugLogging ? LogEventLevel.Verbose : LogEventLevel.Information;
    }

    private Settings LoadSettings(string filename)
    {
        Logging.At(this).Debug("Parsing settings...");
        try
        {
            using TextReader reader       = new StreamReader(Directory.GetCurrentDirectory() + filename);
            var              deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<Settings>(reader);
        }
        catch (Exception ex)
        {
            Logging.At(this).Error("FAILED TO PARSE SETTINGS: {ExMessage}", ex.Message);
            return new Settings();
        }
    }

    public void RevertSettings()
    {
        Logging.At(this).Debug("Reverting settings...");
        Settings = LoadSettings(@"\DefaultSettings.yaml");
    }

    public void UndoSettings()
    {
        Logging.At(this).Debug("Undoing settings changes...");
        Settings = LoadSettings(@"\Settings.yaml");
    }

    public void SaveSettings()
    {
        try
        {
            Logging.At(this).Debug("Saving settings...");
            using TextWriter writer     = new StreamWriter(Directory.GetCurrentDirectory() + @"\Settings.yaml");
            var              serializer = new SerializerBuilder().WithTypeInspector(o => new WriteableTypeInspector(o)).Build();
            serializer.Serialize(writer, Settings);
        }
        catch (Exception ex)
        {
            Logging.At(this).Error("FAILED TO SAVE SETTINGS: {ExMessage}", ex.Message);
        }
    }
}