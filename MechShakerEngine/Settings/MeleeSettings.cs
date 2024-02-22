namespace MechShakerEngine.Settings;

public class MeleeSettings : VolumeSettings
{
    public float HitFrequency    { get; set; }
    public float HitLength       { get; set; }
    public float HitAttackEnd    { get; set; }
    public float HitDecayStart   { get; set; }
    public float SwingFrequency  { get; set; }
    public float SwingLength     { get; set; }
    public float SwingAttackEnd  { get; set; }
    public float SwingDecayStart { get; set; }
    public float SwingAmplitude  { get; set; }
    public float MassFactor      { get; set; }
}