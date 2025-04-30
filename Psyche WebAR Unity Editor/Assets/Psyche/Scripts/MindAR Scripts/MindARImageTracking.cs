using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton manager for MindAR image tracking functionality.
/// </summary>
public class MindARImageTracking : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for accessing
    /// </summary>
    public static MindARImageTracking Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton pattern
    /// </summary>
    private void Awake()
    {
        // Standard singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Controls the active state of the image tracking system
    /// </summary>
    /// <param name="active">Whether tracking should be enabled</param>
    public void ActiveState(bool active)
    {
        gameObject.SetActive(active);
    }
}