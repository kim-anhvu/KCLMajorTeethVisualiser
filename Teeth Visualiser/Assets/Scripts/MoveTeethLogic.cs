/**
 * Simple class that ensures the rotation paramters
 * for the teeth are not out of bounds
 */
public class MoveTeethLogic
{
    // Make sure rotation is not under the lower bound
    public static float UpdateRotMin(float rot, float min)
    {
        if (rot < min) rot = min;
        return rot;
    }

    // Make sure rotation is not over upper bound
    public static float UpdateRotMax(float rot, float max)
    {
        if (rot > max) rot = max;
        return rot;
    }
}
