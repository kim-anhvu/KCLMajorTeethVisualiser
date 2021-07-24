using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  NOTE: This class needs a collider on the GameObject it resides on
 *  in order to work properly!
 *  This class exists in order to allow each tooth from the upper jaw
 *  to be clicked and as such to drag around the entire jaw. It just passes
 *  on the click information to the parent object/script.
 */
public class TeethMove : MonoBehaviour
{
    //The Teeth Script to which this tooth belongs
    public MoveTeeth t;


    // Each frame, update your script
    void Update()
    {
        t.Rotate();
    }

    // If dragging, communicate that to the script
    void OnMouseDown()
    {
        t.isRotating = true;
    }

    // If no longer dragging, communcate that to the script
    void OnMouseUp()
    {
        t.isRotating = false;
    }
}
