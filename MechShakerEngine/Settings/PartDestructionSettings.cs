namespace MechShakerEngine.Settings;

public class PartDestructionSettings : VolumeSettings
{
    public float Frequency           { get; set; }
    public float Length              { get; set; }
    public float AttackEnd           { get; set; }
    public float DecayStart          { get; set; }
    public float SecondaryFrequency  { get; set; }
    public float SecondaryAmplitude  { get; set; }
    public float SecondaryLength     { get; set; }
    public float SecondaryAttackEnd  { get; set; }
    public float SecondaryDecayStart { get; set; }
    public float HeadFactor          { get; set; }
    public float CenterTorsoFactor   { get; set; }
    public float SideTorsoFactor     { get; set; }
    public float ArmFactor           { get; set; }
    public float LegFactor           { get; set; }
}