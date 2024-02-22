namespace MechShakerEngine.Settings;

public class MissilesSettings : VolumeSettings
{
    public float LaunchFrequency  { get; set; }
    public float LaunchAttackEnd  { get; set; }
    public float LaunchDecayStart { get; set; }
    public float LaunchLength     { get; set; }
    public float TailFrequency    { get; set; }
    public float TailAmplitude    { get; set; }
    public float TailAttackEnd    { get; set; }
    public float TailDecayStart   { get; set; }
    public float TailLength       { get; set; }
    public float StreamFactor     { get; set; }
}