namespace MechShakerEngine.Settings;

public class PoweringSettings : VolumeSettings
{
    public float StartFrequency           { get; set; }
    public float EndFrequency             { get; set; }
    public float StartAmplitude           { get; set; }
    public float SecondaryFrequencyFactor { get; set; }
    public float SecondaryAmplitude       { get; set; }
    public float InitDelay                { get; set; }
    public float InitLength               { get; set; }
    public float PoweringUpDelay          { get; set; }
    public float PoweringUpLength         { get; set; }
    public float ShuttingDownDelay        { get; set; }
    public float ShuttingDownLength       { get; set; }
}