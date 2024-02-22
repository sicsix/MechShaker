namespace MechShakerEngine.Settings;

public class MeleeDamageSettings : VolumeSettings
{
    public float Frequency         { get; set; }
    public float AttackEnd         { get; set; }
    public float DecayStart        { get; set; }
    public float MaxLength         { get; set; }
    public float AmplitudeExponent { get; set; }
    public float LengthExponent    { get; set; }
}