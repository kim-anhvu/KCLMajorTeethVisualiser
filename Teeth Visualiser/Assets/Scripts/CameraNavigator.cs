using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOTE: This class requires a collider component on the GameObject it resides on,
 * in order to work properly!
 * Class for providing additional navigation funtionality to the 3rd person orbiting camera
 * It is meant to be attached to a Navigational Cube which will become a constant HUD element.
 * The cube can be dragged around with left mouse click which will in turn move the camera.
 * Clicking on its sides will allow for quick snapping to predefined camera angles.
 */
public class CameraNavigator : MonoBehaviour
{
    public Camera currCam; //Camera connected to this script
    public ThirdPersonCamera cam; // Camera script attached to this script
    public Transform camTrans; // Transform of the camera 
    private float lastTime = 0f; //lastTime of a MouseDown() event


    // Don't allow the camera to move around normally with right-click drag
    // Whilst the mouse hovers over the navigational cube
    private void OnMouseOver()
    {
        cam.dragging = false;
    }

    // Allow the camera to move again after mouse hover exit.
    private void OnMouseExit()
    {
        cam.dragging = true;
    }

    //Update lastTime for the MouseDown() event in order to calculate clicks
    void OnMouseDown()
    {
        lastTime = Time.time;


    }

    //Decide if a MouseUp() event is a click based on the last time of the MouseDown() event 
    // and handle accordingly
    private void OnMouseUp()
    {
        bool click = CameraNavigatorLogic.IsClick(lastTime, Time.time); 
        if (click)
        {
            Raycast();
        }
    }
    
    // Handle a drag event by moving the camera
    void OnMouseDrag()
    {
        cam.updateXY();
        cam.MoveCam();
    }

    /*
     * Cast a Ray from the camera to the position of the camera to the mouse
     * Check which side of the navCube it intersects and move the camera to the appropiate
     * Camera postion.
     */
    private void Raycast()
    {
        Ray ray = currCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            switch (CameraNavigatorLogic.GetHitFace(hit))
            {
                case CameraNavigatorLogic.CubeFace.Up:
                    cam.currentX = 0f;
                    cam.currentY = 89f;
                    break;

                case CameraNavigatorLogic.CubeFace.North:
                    cam.currentX = 180f;
                    cam.currentY = 0f;
                    break;

                case CameraNavigatorLogic.CubeFace.South:
                    cam.currentX = 360f;
                    cam.currentY = 0f;
                    break;

                case CameraNavigatorLogic.CubeFace.East:
                    cam.currentX = 270f;
                    cam.currentY = 0f;
                    break;

                case CameraNavigatorLogic.CubeFace.West:
                    cam.currentX = 90f;
                    cam.currentY = 0f;
                    break;

                case CameraNavigatorLogic.CubeFace.None:
                    cam.currentX = 0f;
                    cam.currentY = -89f;
                    break;

                default:
                    break;
            }
        }
    }
}
