using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[RequireComponent(typeof(LineRenderer))]

/// <summary>
/// Animates an object along a circular orbit around a central object,
/// with support for rotating the orbit plane and visualizing the path.
/// </summary>
public class CircularPathFollow : MonoBehaviour
{
    //------------Public Variables
    public GameObject rotationCenterObject; // Center object for the orbit (sun for planets, planet for satellites)
    public GameObject orbitingObject;       // The object that will orbit around the center
    public float radius = 5f;               // Radius of the circular path
    public float orbitSpeed = 20f;          // Degrees per second for movement along the orbit
    public float rotationTransitionSpeed = 30f; // Speed of transition between rotation angles
    public int numWaypoints = 100;          // Number of points to render the orbit path

    // Predefined rotation values for orbit plane orientation changes
    [Header("Rotation Settings")]
    [Tooltip("The 5 fixed rotation values in degrees for the XY plane")]
    // Array of XY rotation pairs that define different orbit orientations
    public float[,] fixedRotationValues = new float[,]
    {
        { 0f, 0f },
        { 72f, 60f },
        { 144f, 80f },
        { 216f, 100f },
        { 288f, 70f }
    };

    //----------Private Variables
    private Vector3 rotationCenter;         // Position of the center object
    private LineRenderer lineRenderer;      // Line renderer for visualizing the orbit path
    private float currentAngle = 0f;        // Current angle along the orbit
    private float currentYRotation = 0f;    // Current Y-axis rotation of orbit plane
    private float currentXRotation = 0f;    // Current X-axis rotation of orbit plane
    private int currentRotationIndex = 0;   // Index of current rotation preset
    private float targetYRotation = 0f;     // Target Y rotation to transition to
    private float targetXRotation = 0f;     // Target X rotation to transition to
    private bool isOrbitDirectionForward = true; // Direction of orbit (clockwise/counterclockwise)
    private float tempRadius;               // Temporarilly holds the starting set radius when it fades in and out
    private bool isActive;                  // Whether the path renderer is faded in or out

    //-------------Queue for rotation transitions
    private Queue<float[]> lineRendererQueue = new Queue<float[]>();
    private const int queueAxisCount = 2;   // Each queue entry has 2 values (X and Y rotation)

    /// <summary>
    /// Unity Start callback. Initializes the orbit and line renderer setup.
    /// </summary>
    void Start()
    {
        if (rotationCenterObject == null)
        {
            Debug.LogError("Rotation Center Object is not assigned, oops");
            return;
        }

        if (orbitingObject == null)
        {
            Debug.LogError("Orbiting Object is not assigned, oops");
            return;
        }

        // Initialize rotation to first preset value
        currentYRotation = fixedRotationValues[0, 0];
        targetYRotation = currentYRotation;

        currentXRotation = fixedRotationValues[0, 1];
        targetXRotation = currentXRotation;

        // Initialize line renderer to visualize orbit path
        rotationCenter = rotationCenterObject.transform.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numWaypoints + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.useWorldSpace = true;
        tempRadius = radius;
        isActive = true;

        // Initialize visual elements
        UpdateOrbitPath();
        UpdateSatellitePosition();
    }

    /// <summary>
    /// Unity Update callback. Updates the orbit and transition smoothly between rotation orientations.
    /// </summary>
    void Update()
    {
        // Process rotation transitions if there are pending changes
        if (lineRendererQueue.Count > 0)
        {
            float rotationStep = rotationTransitionSpeed * Time.deltaTime;

            var next = lineRendererQueue.Peek();
            targetYRotation = next[0];
            targetXRotation = next[1];

            // Smoothly rotate Y axis using the shortest path
            float yDelta = Mathf.DeltaAngle(currentYRotation, targetYRotation);
            if (Mathf.Abs(yDelta) > rotationStep)
            {
                currentYRotation += Mathf.Sign(yDelta) * rotationStep;
            }
            else
            {
                currentYRotation = targetYRotation;
            }

            // Smoothly rotate X axis using the shortest path
            float xDelta = Mathf.DeltaAngle(currentXRotation, targetXRotation);
            if (Mathf.Abs(xDelta) > rotationStep)
            {
                currentXRotation += Mathf.Sign(xDelta) * rotationStep;
            }
            else
            {
                currentXRotation = targetXRotation;
            }

            // Remove completed transition from queue
            if (Mathf.Approximately(currentYRotation, targetYRotation) &&
                Mathf.Approximately(currentXRotation, targetXRotation))
            {
                lineRendererQueue.Dequeue();
            }
        }

        // Update orbit position based on direction and speed
        float directionMultiplier = isOrbitDirectionForward ? 1f : -1f;
        currentAngle += orbitSpeed * directionMultiplier * Time.deltaTime;

        // Wrap angle within 0-360 range
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }
        else if (currentAngle < 0f)
        {
            currentAngle += 360f;
        }

        // Update visual representation
        UpdateOrbitPath();
        UpdateSatellitePosition();
    }

    /// <summary>
    /// Switch to next orbit plane orientation preset.
    /// </summary>
    public void IncrementToNextPosition()
    {
        // Cycle to next rotation preset
        currentRotationIndex = (currentRotationIndex + 1) % fixedRotationValues.GetLength(0);

        // Set the new target rotations
        targetYRotation = fixedRotationValues[currentRotationIndex, 0];
        targetXRotation = fixedRotationValues[currentRotationIndex, 1];

        // Queue the transition
        lineRendererQueue.Enqueue(new float[] {
            targetYRotation,
            targetXRotation
        });
    }

    /// <summary>
    /// Switch to previous orbit plane orientation preset.
    /// </summary>
    public void DecrementToLastPosition()
    {
        // Cycle to previous rotation preset
        currentRotationIndex = (currentRotationIndex - 1 + fixedRotationValues.GetLength(0)) % fixedRotationValues.GetLength(0);

        // Set the new target rotations
        targetYRotation = fixedRotationValues[currentRotationIndex, 0];
        targetXRotation = fixedRotationValues[currentRotationIndex, 1];

        // Queue the transition
        lineRendererQueue.Enqueue(new float[] {
            targetYRotation,
            targetXRotation
        });
    }

    /// <summary>
    /// Set orbit direction to forward (counterclockwise).
    /// </summary>
    public void RotateForward()
    {
        isOrbitDirectionForward = true;
    }

    /// <summary>
    /// Set orbit direction to backward (clockwise).
    /// </summary>
    public void RotateBackward()
    {
        isOrbitDirectionForward = false;
    }

    /// <summary>
    /// Generate points for the orbit path visualization.
    /// </summary>
    void UpdateOrbitPath()
    {
        float angleStep = 360f / numWaypoints;

        for (int i = 0; i <= numWaypoints; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Create a point in the XY plane
            Vector3 basePoint = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            // Apply rotations to create the tilted orbit plane
            Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
            Vector3 rotatedPoint = rotation * basePoint;

            // Position relative to the center object
            Vector3 finalPoint = rotationCenter + rotatedPoint;

            lineRenderer.SetPosition(i, finalPoint);
        }
    }

    /// <summary>
    /// Update the orbiting object's position and orientation.
    /// </summary>
    void UpdateSatellitePosition()
    {
        // Calculate base position on the XY plane
        float pathAngleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 basePosition = new Vector3(
            Mathf.Cos(pathAngleRad) * radius,
            Mathf.Sin(pathAngleRad) * radius,
            0f
        );

        // Apply orbit plane rotation
        Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
        Vector3 rotatedPosition = rotation * basePosition;

        // Set the final position
        orbitingObject.transform.position = rotationCenter + rotatedPosition;

        // Calculate tangent direction for forward orientation
        Vector3 tangent = rotation * new Vector3(
            -Mathf.Sin(pathAngleRad),
            Mathf.Cos(pathAngleRad),
            0f
        );

        // Adjust tangent direction based on orbit direction
        if (!isOrbitDirectionForward)
        {
            tangent = -tangent;
        }

        // Orient object to face movement direction with appropriate tilt
        Vector3 up = (orbitingObject.transform.position - rotationCenter).normalized;
        orbitingObject.transform.rotation = Quaternion.LookRotation(tangent, -up);
    }

    /// <summary>
    /// Toggles radial effect
    /// </summary>
    public void ToggleRadius()
    {
        if (isActive)
            StartCoroutine(FadeOutRadius());
        else
            StartCoroutine(FadeInRadius());
        isActive = !isActive;
    }

    /// <summary>
    /// Adds Lerp controls to add gradual fade in.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInRadius()
    {
        float timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            radius = Mathf.Lerp(0f, tempRadius, timer / 0.3f);
            yield return null;
        }
        radius = tempRadius;
    }

    /// <summary>
    /// Add Lerp controls to add gradual fade out.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutRadius()
    {
        float timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            radius = Mathf.Lerp(tempRadius, 0f, timer / 0.3f);
            yield return null;
        }
        radius = 0f;
    }
}