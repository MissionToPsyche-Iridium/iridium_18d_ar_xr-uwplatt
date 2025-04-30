using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles orbital movement of a GameObject around a specified target object.
/// </summary>
public class OrbitController : MonoBehaviour
{
    public Transform targetObject; // Object to rotate about
    public float rotationSpeed = 30f;
    public float rotationAmount = 0f; //Cumulative rotation amount, should be private
    public int rotationDirection = 1; // Rotational direction

    /// <summary>
    /// Applies rotation and tracks current rotation amount.
    /// </summary>
    void Update()
    {
        float rotationThisFrame = rotationSpeed * Time.deltaTime * rotationDirection;
        transform.RotateAround(targetObject.position, Vector3.up, rotationThisFrame);
        rotationAmount += Mathf.Abs(rotationThisFrame);
    }
}
