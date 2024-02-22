namespace MechShakerEngine.Settings;

public class FootstepsSettings : VolumeSettings
{
    public float Frequency   { get; set; }
    public float Length      { get; set; }
    public float AttackEnd   { get; set; }
    public float DecayStart  { get; set; }
    public float SpeedFactor { get; set; }
    public float MassFactor  { get; set; }
}