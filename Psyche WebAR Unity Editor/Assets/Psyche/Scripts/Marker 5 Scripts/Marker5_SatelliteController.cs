using System;
using UnityEngine;


/// <summary>
/// Controls the satellite's position and camera state across multiple predefined orbit positions.
/// Handles transitions between orbits, camera toggling when zooming in/out,
/// and UI updates related to the current orbit.
/// </summary>
public class Marker5_SatelliteController : MonoBehaviour
{
    // Component references
    [SerializeField] private Transform Camera;               // Reference to the camera transform
    [SerializeField] private Transform OrbitObject;          // Reference to the object controlling the orbit path
    [SerializeField] private Marker5_UIController Marker5_UIController;  // Reference to UI controller for updating display

    // Orbit state tracking
    private OrbitState currentOrbitState;    // Current state of the satellite orbit
    private OrbitState nextOrbitState;       // Target state to transition to
    private CameraToggle cameraInstance;     // Camera movement controller reference
    private CircularPathFollow orbitInstance; // Orbit path controller reference

    // Ordered list of possible orbit states for sequential navigation
    private OrbitState[] orbitList = new OrbitState[]
    {
        OrbitState.ZoomedOut,
        OrbitState.OrbitA,
        OrbitState.OrbitB,
        OrbitState.OrbitC,
        OrbitState.OrbitD,
        OrbitState.OrbitE
    };

    // Singleton pattern implementation
    public static Marker5_SatelliteController Instance { get; private set; }

    /// <summary>
    /// Ensure singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Initialize objects, set orbit states.
    /// </summary>
    private void Start()
    {
        cameraInstance = Camera.GetComponent<CameraToggle>();
        orbitInstance = OrbitObject.GetComponent<CircularPathFollow>();

        currentOrbitState = OrbitState.ZoomedOut;
        nextOrbitState = OrbitState.ZoomedOut;
    }

    /// <summary>
    /// Determines what state orbit should be in on update.
    /// </summary>
    void Update()
    {
        // Handle state transitions when needed
        if (currentOrbitState != nextOrbitState)
        {
            // If transitioning to/from ZoomedOut state, toggle camera view
            if (Array.IndexOf(orbitList, currentOrbitState) == 0 || Array.IndexOf(orbitList, nextOrbitState) == 0)
            {
                cameraInstance.UpdateTarget();
            }
            // If moving backward in the orbit sequence
            else if (Array.IndexOf(orbitList, currentOrbitState) > Array.IndexOf(orbitList, nextOrbitState))
            {
                orbitInstance.RotateForward();
                orbitInstance.IncrementToNextPosition();
            }
            // If moving forward in the orbit sequence
            else
            {
                orbitInstance.RotateBackward();
                orbitInstance.DecrementToLastPosition();
            }
        }

        currentOrbitState = nextOrbitState;
    }

    /// <summary>
    /// Sets the next orbit state based on a relative position increment
    /// </summary>
    /// <param name="increment">Positive or negative value to determine next orbit state</param>
    public void SetNextOrbitState(int increment)
    {
        // Find current index in orbit sequence
        int nextArray = Array.IndexOf(orbitList, currentOrbitState);


        for (int i = 0; i < orbitList.Length; i++)
        {
            if (i == nextArray + increment)
            {
                // Set target state and update UI display
                nextOrbitState = orbitList[i];
                Marker5_UIController.ChangeInfoText(i + 1);
            }
        }
    }

    /// <summary>
    /// Returns the current orbit state
    /// </summary>
    public OrbitState GetCurrentOrbitState() { return currentOrbitState; }
}

/// <summary>
/// Defines possible orbit states for the satellite
/// </summary>
public enum OrbitState
{
    ZoomedOut,  // Camera zoomed out showing the full system
    OrbitA,     // First orbit configuration
    OrbitB,     // Second orbit configuration
    OrbitC,     // Third orbit configuration
    OrbitD,     // Fourth orbit configuration
    OrbitE      // Fifth orbit configuration
}