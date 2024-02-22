namespace MechShakerEngine.Settings;

public class JumpJetsSettings : VolumeSettings
{
    public float Frequency          { get; set; }
    public float SecondaryFrequency { get; set; }
    public float SecondaryAmplitude { get; set; }
    public float TertiaryFrequency  { get; set; }
    public float TertiaryAmplitude  { get; set; }
    public float TransitionOnTime   { get; set; }
    public float TransitionOffTime  { get; set; }
}