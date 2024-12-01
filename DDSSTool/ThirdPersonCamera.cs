using DDSSTool;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float distance = 5.0f;  // Distance from the player
    public float rotationSpeed = 3.0f;  // Speed of horizontal rotation
    public float verticalRotationSpeed = 2.0f;  // Speed of vertical rotation
    public float pitchMin = -20f;  // Minimum angle for vertical rotation
    public float pitchMax = 80f;  // Maximum angle for vertical rotation

    private float yaw = 0.0f;  // Horizontal rotation (yaw)
    private float pitch = 0.0f;  // Vertical rotation (pitch)

    void Update()
    {
        player = ToolMain.localPlayer.transform;
        // Handle mouse input for rotation
        yaw += rotationSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalRotationSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);  // Clamp vertical angle

        // Calculate the camera's position based on player position and rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = player.position - rotation * Vector3.forward * distance;

        // Smooth camera position to follow the player
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);

        // Make the camera always face the player
        transform.LookAt(player);
    }
}