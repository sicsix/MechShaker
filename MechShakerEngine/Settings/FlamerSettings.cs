namespace MechShakerEngine.Settings;

public class FlamerSettings : VolumeSettings
{
    public float Frequency          { get; set; }
    public float SecondaryFrequency { get; set; }
    public float SecondaryAmplitude { get; set; }
    public float TertiaryFrequency  { get; set; }
    public float TertiaryAmplitude  { get; set; }
}