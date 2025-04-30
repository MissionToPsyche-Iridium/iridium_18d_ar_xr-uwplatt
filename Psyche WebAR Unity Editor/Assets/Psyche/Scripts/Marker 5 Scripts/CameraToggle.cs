using UnityEngine;

/// <summary>
/// Handles toggling the camera between two predefined points with smooth movement.
/// </summary>
public class CameraToggle : MonoBehaviour
{
    // Camera position targets
    public Transform point1;        // First camera position
    public Transform point2;        // Second camera position
    public float moveSpeed = 5f;    // Camera movement speed multiplier

    // State tracking
    private bool isAtObject1 = true;    // Tracks which target the camera is currently focused on
    private Transform targetObject;     // Current target the camera is moving towards

    /// <summary>
    /// sets target object on start
    /// </summary>
    void Start()
    {
        targetObject = point1;
    }

    /// <summary>
    /// Toggles between camera positions when called
    /// </summary>
    public void UpdateTarget()
    {
        // Switch to opposite target position
        if (isAtObject1)
        {
            targetObject = point2;
        }
        else
        {
            targetObject = point1;
        }

        // Invert current position state
        isAtObject1 = !isAtObject1;
    }

    /// <summary>
    /// On update move camera towards target location
    /// </summary>
    void Update()
    {
        MoveCameraToTarget();
    }

    /// <summary>
    /// Smoothly moves camera toward current target position
    /// </summary>
    void MoveCameraToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetObject.position, moveSpeed * Time.deltaTime);
    }
}