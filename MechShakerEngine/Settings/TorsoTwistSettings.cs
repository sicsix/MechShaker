namespace MechShakerEngine.Settings;

public class TorsoTwistSettings : VolumeSettings
{
    public float Frequency      { get; set; }
    public float TransitionTime { get; set; }
    public float MassFactor     { get; set; }
}