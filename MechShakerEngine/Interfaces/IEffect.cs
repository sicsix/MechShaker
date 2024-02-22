namespace MechShakerEngine.Interfaces;

public interface IEffect
{
    float GetActiveInstances();
    void  WriteToGroup(float[] buffer, int offset, int count);
}