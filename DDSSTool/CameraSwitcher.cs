using DDSSTool;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera thirdPersonCamera;  // Reference to your third-person camera

    void Start()
    {
        thirdPersonCamera = ToolMain.newCam;
        // If the third-person camera is assigned, set it as the main camera
        if (thirdPersonCamera != null)
        {
            // Remove the MainCamera tag from the current camera (if any)
            Camera oldCamera = Camera.main;
            if (oldCamera != null)
            {
                oldCamera.tag = "Untagged";  // Remove the "MainCamera" tag from the old camera
            }

            // Set the third-person camera as the new MainCamera
            thirdPersonCamera.tag = "MainCamera";  // Set the tag to "MainCamera"
        }
    }
}