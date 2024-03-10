namespace MechShakerEngine.Settings;

public class LasersSettings : VolumeSettings
{
    public float Frequency          { get; set; }
    public float SecondaryFrequency { get; set; }
    public float SecondaryAmplitude { get; set; }
    public float AmplitudeExponent     { get; set; }
}