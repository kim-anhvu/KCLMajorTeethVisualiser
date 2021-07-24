using UnityEngine;
using UnityEngine.UI;


/*
 * Third Person Camera Class which provides orbiting functionality.
 * Right click and drag to move around the target.
 * Scroll Wheel to zoom in and out.
 * Constraints on zooming in and on out-of-bounds orbiting.
 * Ability to split the screen into two
 */
public class ThirdPersonCamera : MonoBehaviour
{

    public Transform lookAt; // Transform at which this object constantly looks
    public Transform navCubeTrans; // Tranform of the navigational cube
    public Camera main_cam; // The camera to which this cube is attached
    public Toggle lock_cam; // toggle for decideing if the cameras should move together, if in split screen mode
    public ThirdPersonCamera other_cam; // the other camera, if in split screen mode
    
    // Values in free aspect to make the NavBox positioned in the bottom right 
    public float fd;        //
    public float rt;        // ratio
    public float dn;        // 
    public float scroll_sensitivity;
    // Zoom distance lower and upper bounds
    public float minDist = 0f;
    public float maxDist = 1000f;

    // Current distance
    public float distance = 300.0f;

    // X and Y mouse position on screen determine the rotation of the cam in 3D space
    public float currentX = 0.0f;
    public float currentY = 0.0f;

    // Assumed Screen Aspect Ratio 16/9 sensitivity values for each axis 
    // in order to make scrolling constant across dimensions
    private float sensitivityX = 9.0f;
    private float sensitivityY = 4.0f;

    // If the right click button is held
    public bool rightclicked;

    public bool dragging = true;

    // Used to determine if the camera is allowed to move in compare mode
    public bool canMove_split;

    public UnityEngine.UI.Slider zoomSlider;

    //If we're currently in the compare mode
    public bool splitScreen;

    // is set whether this camera is operating on the left side of the screen.
    public bool leftSide;       

    private Vector3 clickLocation;
    public GameObject otherNavBox;  //refrernce to second navbox.

    //Initialise the camera
    void Start()
    {
        splitScreen = false;
        this.setSplitScreen(false);
        this.rt = ThirdPersonLogic.GetScreenSizeRatio(Screen.width, splitScreen);
    }

    // Set camera split view on or off
    public void setSplitScreen(bool split)
    {
        this.splitScreen = split; 
        updateCameraSize();
    } 

    //Split screen size accordingly
    private void updateCameraSize()
    {
        // if split screen
        if(splitScreen){
            // set camera settings of right side
            if(!leftSide){  // right side - camera details 
                main_cam.enabled = true; 
                main_cam.rect = new Rect((float)0.5,0,(float) 0.5,1);

            }else { // left side 
                main_cam.enabled = true; 
                main_cam.rect = new Rect(0,0,(float)0.5,1);
            }
        }else{
            if(leftSide){
                //make camera full 
                main_cam.enabled = true; 
                main_cam.rect = new Rect(0,0,1,1);
            }else {
                // disable camera 
                //make camera have an empty click space
                main_cam.enabled = false; 
                main_cam.rect = new Rect(0,0,0,0);
            }
        }
        
    }
    
    //Each frame, update the location of the camera
    private void Update()
    {
        if(!splitScreen)
        {
            canMove_split = true;
        }
        float ratio = ThirdPersonLogic.GetScreenSizeRatio(Screen.width, splitScreen);
        if (rt != ratio)
        {
            rt = ratio;
        }
        //Update Orbiting distance based on scroll wheel position.
        distance -= Input.mouseScrollDelta.y * scroll_sensitivity;
        // Apply constraints to orbiting distance
        distance = ThirdPersonLogic.UpdateMinDist(distance, minDist);
        distance = ThirdPersonLogic.UpdateMaxDist(distance, maxDist);
        zoomSlider.value = distance; // Update zoom slider to match new dist
        setCanMove(); // Lock camera if in split-view mode and other camera is moving
        if (splitScreen && lock_cam.isOn && !leftSide)
        {
            // If split view and moving together, make sure cameras are in sync
            currentX = other_cam.currentX;
            currentY = other_cam.currentY;
        } else
        {   // Handle Inputs
            if (Input.GetMouseButtonDown(1))
            {
                rightclicked = true;
                clickLocation = Input.mousePosition;

            }
            if (Input.GetMouseButtonUp(1))
            {
                rightclicked = false;
            }
            // Moce Camera
            if (rightclicked && dragging)
            {
                setCanMove();
                if(canMove_split)
                    updateXY();
            }
            if (splitScreen)
            {
                lock_cam.gameObject.SetActive(true);
            }
            else
            {
                lock_cam.gameObject.SetActive(false);
            }
        }
    }

    // Update the XY coordinates of the mouse on the screen
    // which in turn corelate to a certain rotation
    public void updateXY()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY += Input.GetAxis("Mouse Y") * sensitivityY;
    }

    // Based on split screen status, decide if this camera
    // is allowed to move or not
    private void setCanMove()
    {
        //Lock cameras, if toggle is on.
        if(lock_cam.isOn){
            canMove_split = true;
            otherNavBox.SetActive(false);
            return;
        } else
        {
            otherNavBox.SetActive(true);
        }
        //Camera only works on the side of the screen allocated.
        if(!splitScreen && leftSide){
            canMove_split = true;
        }
        else if(splitScreen && leftSide){
            if(clickLocation.x < Screen.width/2){
                canMove_split = true;
            }else{
                canMove_split = false; 
            }
        }else if(splitScreen && !leftSide){
            if(clickLocation.x > Screen.width/2){
                canMove_split = true;
            }else{
                canMove_split = false; 
            }
        }
        else{
            canMove_split = true; 
        }

    }

    // Similar to Update() which is called once a frame, but 
    // LateUpdate() ensures even times between calls 
    // (Update depends on frame draw time which varies according to load)
    private void LateUpdate()
    {
            MoveCam();
    }

    // Update distance based on slider position.
    public void SetDist(GameObject slider)
    {
        distance = slider.GetComponent<UnityEngine.UI.Slider>().value;
    }


    // Move the camrea to the newly specified position in space
    // Based on mouse X Y coordinates as well as the specified 
    // zoom distance
    public void MoveCam()
    {
        //Make sure Y position is constrained accordingly
        currentY = ThirdPersonLogic.UpdateCurrentY(currentY);

        //Distance vector
        Vector3 dir = new Vector3(0, 0, -distance);

        //Construct rotation based on currentX, Y
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 lookRot = lookAt.position;
        Vector3 newLoc = ThirdPersonLogic.CalculateNewLocation(lookRot, rotation, dir);


        //Move to the new position
        transform.position = newLoc;
        
        // Rotate in order to look at the target
        transform.LookAt(lookRot);

        //Move the navigational Cube to the new postion
        navCubeTrans.position = newLoc;

        //Keep track of the original rotation of the cube
        Quaternion oldRot = navCubeTrans.rotation;

        //Make cube look at target, in order to make translation easier
        navCubeTrans.LookAt(lookRot);

        //Move cube relative to camera position, in order to make it stay in a certain part of the screen as a HUD element
        navCubeTrans.position = ThirdPersonLogic.CalculateNewNavBoxLocaton(navCubeTrans, fd, rt, dn);

        //Revert navigational Cube to original rotation
        navCubeTrans.rotation = oldRot;
    }
}
