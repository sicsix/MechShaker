namespace MechShakerEngine.Settings;

public class DropshipSettings : VolumeSettings
{
    public float FlightFrequency             { get; set; }
    public float FlightAmplitude             { get; set; }
    public float FlightSecondaryFrequency    { get; set; }
    public float FlightSecondaryAmplitude    { get; set; }
    public float FlightTertiaryFrequency     { get; set; }
    public float FlightTertiaryAmplitude     { get; set; }
    public float FlightTransitionOff         { get; set; }
    public float LandedFrequency             { get; set; }
    public float LandedAmplitude             { get; set; }
    public float LandedLength                { get; set; }
    public float LandedAttackEnd             { get; set; }
    public float LandedDecayStart            { get; set; }
    public float TurntableFrequency          { get; set; }
    public float TurntableAmplitude          { get; set; }
    public float TurntableSecondaryFrequency { get; set; }
    public float TurntableSecondaryAmplitude { get; set; }
    public float TurntableTransitionTime     { get; set; }
}