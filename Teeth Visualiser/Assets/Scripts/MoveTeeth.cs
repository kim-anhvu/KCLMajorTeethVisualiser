using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOTE: This class needs a Collider to be attached
 *  to the GameObject it resides on in order to work properly!
 *  It allows for rotation by mouse drag only on the X World Axis.
 */
public class MoveTeeth : MonoBehaviour
{
    public float sensitivity = 0.5f;

    // Reference position for dragging
    public float mouseOffsetX; 
    public bool isRotating = false;

    //Min and Max rotation constraints
    public float min;
    public float max;

    //Current Rotation EulerAngles to relate to when applying new rotation
    public Vector3 rot;

    //Set initial rotation to current rotation
    void Start()
    {
        rot = transform.rotation.eulerAngles;
    }

    // Update rotation each frame
    void Update()
    {
        Rotate();
    }

    // Rotate the Teeth based on new inputs
    public void Rotate()
    {
        if (isRotating)
        {
            mouseOffsetX = Input.GetAxis("Mouse X");

            // Change x axis rotation by adding offset times sensitivity
            rot.x += mouseOffsetX * sensitivity;

            // Constraint checks
            rot.x = MoveTeethLogic.UpdateRotMin(rot.x, min);
            rot.x = MoveTeethLogic.UpdateRotMax(rot.x, max);

            // Apply Rotation - N.B.: Unity stores rotation information as Quaternions because of Gimball Lock 
            transform.rotation = Quaternion.Euler(rot);

        }
    }

    void OnMouseDown()
    {
        isRotating = true;
    }

    void OnMouseUp()
    {
        isRotating = false;
    }

}