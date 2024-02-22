namespace MechShakerEngine;

internal enum EventCode
{
    ClearFX         = -2,
    BridgeClosed    = -1,
    NULL            = 0,
    TorsoTwist      = 1,
    Footstep        = 2,
    Trace           = 3,
    Projectile      = 4,
    Missiles        = 5,
    AMS             = 6,
    Melee           = 7,
    Dropship        = 8,
    JumpJets        = 9,
    Airborne        = 10,
    Landed          = 11,
    MASC            = 12,
    Powering        = 13,
    PartDestruction = 14,
    Damaged         = 15,
}