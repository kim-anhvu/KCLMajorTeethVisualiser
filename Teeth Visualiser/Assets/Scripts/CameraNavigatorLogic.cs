using UnityEngine;


/* This class models the logic behind
 * the naviagtional cube. It handels raycasts
 *  in order to determine the side 
 *  of the cube which has been clicked.
 */
public class CameraNavigatorLogic
{
    // Enum for each side of the Cube
    public enum CubeFace
    {
        None,
        Up,
        Down,
        East,
        West,
        North,
        South
    }

    //Based on the RayCastHit, figure out the clicked face of the cube.
    public static CubeFace GetHitFace(RaycastHit hit)
    {
        Vector3 incomingVec = hit.normal - Vector3.up;

        if (incomingVec == new Vector3(0, -1, -1))
            return CubeFace.South;

        if (incomingVec == new Vector3(0, -1, 1))
            return CubeFace.North;

        if (incomingVec == new Vector3(0, 0, 0))
            return CubeFace.Up;

        if (incomingVec == new Vector3(1, 1, 1))
            return CubeFace.Down;

        if (incomingVec == new Vector3(-1, -1, 0))
            return CubeFace.West;

        if (incomingVec == new Vector3(1, -1, 0))
            return CubeFace.East;

        return CubeFace.None;
    }

    // Decide if a click is actually a click or a drag based on time 
    public static bool IsClick(float lastTime, float currentTime)
    {
        if(currentTime - lastTime < 0.25)
        {
            return true;
        } else
        {
            return false;
        }
    }

}
