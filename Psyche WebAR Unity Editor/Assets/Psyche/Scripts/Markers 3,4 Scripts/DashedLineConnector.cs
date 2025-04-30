using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connects two objects in the scene using a LineRenderer, optionally with offset positions.
/// Designed to visually link elements with a dashed or solid line.
/// </summary>
public class DashedLineConnector : MonoBehaviour
{
    // LineRenderer component used to draw the line
    public LineRenderer lineRenderer;
    public Transform object1;
    public Transform object2;
    public Vector3 offsetObject1;
    public Vector3 offsetObject2;

    /// <summary>
    /// Unity Start callback. Initializes the LineRenderer with two points.
    /// </summary>
    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    /// <summary>
    /// Unity Update callback. Updates the positions of the LineRenderer each frame.
    /// </summary>
    void Update()
    {
        // Calculate world positions with offsets
        Vector3 position1 = object1.position + offsetObject1;
        Vector3 position2 = object2.position + offsetObject2;

        // Update line positions
        lineRenderer.SetPosition(0, position1);
        lineRenderer.SetPosition(1, position2);
    }
}
