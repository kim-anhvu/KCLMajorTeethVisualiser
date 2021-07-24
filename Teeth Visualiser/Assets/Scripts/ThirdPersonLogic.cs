using UnityEngine;
using UnityEngine.UI;

/**
 * Helper class containing the logic for the ThirdPersonCam
 * camera script.
 */
public class ThirdPersonLogic
{
    //Screen Width is used to determine the position of the navigational cube
    public static float GetScreenSizeRatio(float width, bool splitScreen)
    {
        float ratio = (width / 100) + 0.75f;
        if (splitScreen)
        {
            ratio =  ratio / 2;
        }
        return ratio - 4;
    }

    // Don't allow the camera to loop around in the Y direction (up-down)
    // i.e. ensure currentY is doesn't step outside bounds
    public static float UpdateCurrentY(float y)
    {
        if (y < -89f) y = -89f;
        if (y > 89f) y = 89f;
        return y;
    }

    // Ensure distance isn't bellow lower bound
    public static float UpdateMinDist(float cur, float min)
    {
        if (cur < min) cur = min;
        return cur;
    }

    // Ensure distance isn't over the upper bound
    public static float UpdateMaxDist(float cur, float max)
    {
        if (cur > max) cur = max;
        return cur;
    }

    // Calculate the new location for the camera GameObject based on the target to look at,
    // the rotation and the direction
    public static Vector3 CalculateNewLocation(Vector3 lookAtPos, Quaternion rot, Vector3 dir)
    {
        return lookAtPos + rot * dir;
    }

    // Reposition the navigational cube to stay in the same position relative to the camera
    public static Vector3 CalculateNewNavBoxLocaton(Transform init, float fd, float rt, float dn)
    {
        return init.position + init.forward * fd + init.right * rt + init.up * -dn;
    }

}
