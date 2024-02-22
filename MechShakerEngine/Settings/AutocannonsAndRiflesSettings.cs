namespace MechShakerEngine.Settings;

public class AutocannonsAndRiflesSettings : VolumeSettings
{
    public float MinFrequency { get; set; }
    public float MaxFrequency { get; set; }
    public float MinAmplitude { get; set; }
    public float MinLength    { get; set; }
    public float MaxLength    { get; set; }
    public float AttackEnd    { get; set; }
    public float DecayStart   { get; set; }
}