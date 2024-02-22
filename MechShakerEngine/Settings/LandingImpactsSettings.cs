namespace MechShakerEngine.Settings;

public class LandingImpactsSettings : VolumeSettings
{
    public float Frequency  { get; set; }
    public float Length     { get; set; }
    public float AttackEnd  { get; set; }
    public float DecayStart { get; set; }
}