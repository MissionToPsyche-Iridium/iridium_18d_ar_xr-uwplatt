using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the attached GameObject continuously around its Z-axis.
/// This script simulates a slow rotisserie spin for celestial objects.
/// </summary>
public class RotisseriePlanets : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public bool isRotisserie = true;

    /// <summary>
    /// Rotate about the z-axis with a specified speed.
    /// </summary>
    void Update()
    {
        if (isRotisserie) // rotates like the asteroid
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        } 
        else // roates like a top
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}
