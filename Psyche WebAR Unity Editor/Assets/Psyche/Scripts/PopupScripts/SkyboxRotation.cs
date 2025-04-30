using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates sky-box slowly.
/// </summary>
public class SkyboxRotation : MonoBehaviour
{
    /// <summary>
    /// Not used
    /// </summary>
    void Start()
    {
        
    }

    public float rotationSpeed = 5f;
    /// <summary>
    /// Apply rotation to sky-box every update.
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    
}
