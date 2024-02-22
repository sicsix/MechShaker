namespace MechShakerEngine.Settings;

public class Settings
{
    public string                       OutputDevice         { get; set; } = string.Empty;
    public int                          Latency              { get; set; }
    public VolumeSettings               Master               { get; set; } = new();
    public LasersSettings               Lasers               { get; set; } = new();
    public TAGSettings                  TAG                  { get; set; } = new();
    public FlamerSettings               Flamers              { get; set; } = new();
    public MachineGunSettings           MachineGuns          { get; set; } = new();
    public AMSSettings                  AMS                  { get; set; } = new();
    public AutocannonsAndRiflesSettings AutocannonsAndRifles { get; set; } = new();
    public PPCsSettings                 PPCs                 { get; set; } = new();
    public MissilesSettings             Missiles             { get; set; } = new();
    public MeleeSettings                Melee                { get; set; } = new();
    public FootstepsSettings            Footsteps            { get; set; } = new();
    public LandingImpactsSettings       LandingImpacts       { get; set; } = new();
    public LaserDamageSettings          LaserDamage          { get; set; } = new();
    public ProjectileDamageSettings     ProjectileDamage     { get; set; } = new();
    public MissileDamageSettings        MissileDamage        { get; set; } = new();
    public MeleeDamageSettings          MeleeDamage          { get; set; } = new();
    public ExplosionDamageSettings      ExplosionDamage      { get; set; } = new();
    public PartDestructionSettings      PartDestruction      { get; set; } = new();
    public TorsoTwistSettings           TorsoTwist           { get; set; } = new();
    public JumpJetsSettings             JumpJets             { get; set; } = new();
    public MASCSettings                 MASC                 { get; set; } = new();
    public DropshipSettings             Dropship             { get; set; } = new();
    public PoweringSettings             Powering             { get; set; } = new();
    public bool                         AdvancedMode         { get; set; }
    public bool                         DebugLogging         { get; set; }
    public FilterSettings               LowPassFilter        { get; set; } = new();
    public FilterSettings               HighPassFilter       { get; set; } = new();
}