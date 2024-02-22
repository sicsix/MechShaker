namespace MechShakerEngine.Settings;

public class PPCsSettings : VolumeSettings
{
    public float PopFrequency   { get; set; }
    public float PopAttackEnd   { get; set; }
    public float PopDecayStart  { get; set; }
    public float PopLength      { get; set; }
    public float TailFrequency  { get; set; }
    public float TailAmplitude  { get; set; }
    public float TailAttackEnd  { get; set; }
    public float TailDecayStart { get; set; }
    public float TailLength     { get; set; }
}