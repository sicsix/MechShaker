namespace MechShakerEngine.Interfaces;

internal interface IGenerator
{
    bool Complete { get; }

    void Write(float[] buffer, int offset, int count, ref float activeInstances);
}