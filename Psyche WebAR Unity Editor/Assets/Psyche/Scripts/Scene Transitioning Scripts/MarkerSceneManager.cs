using MarksAssets.MindAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the AR marker detection system, scene transitions, and UI interactions for marker-based experiences
/// </summary>
public class MarkerSceneManager : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;    // The AR camera used for marker detection and rendering
    [Space]
    [SerializeField] private Button Marker1;
    [SerializeField] private Button Marker2;
    [SerializeField] private Button Marker3;
    [SerializeField] private Button Marker4;
    [SerializeField] private Button Marker5;
    [SerializeField] private Button Marker6;
    [Space]
    [SerializeField] private Transform Cube1;
    [SerializeField] private Transform Cube2;
    [SerializeField] private Transform Cube3;
    [SerializeField] private Transform Cube4;
    [SerializeField] private Transform Cube5;
    [SerializeField] private Transform Cube6;

    /// <summary>
    /// Singleton instance providing global access to marker management functionality
    /// </summary>
    public static MarkerSceneManager Instance { get; private set; }

    /// <summary>
    /// Establishes singleton pattern during initialization
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
    /// Sets up button click handlers for all marker transitions
    /// </summary>
    private void Start()
    {
        // Sets up interaction handlers for each marker button to record progress and transition to appropriate scenes
        // =============================================================================================
        Marker1.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(0);
            MarkerTransition("Marker 1");
        });

        Marker2.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(1);
            MarkerTransition("Marker 2");
        });

        Marker3.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(2);
            MarkerTransition("Marker 3");
        });

        Marker4.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(3);
            MarkerTransition("Marker 4");
        });

        Marker5.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(4);
            MarkerTransition("Marker 5");
        });

        Marker6.onClick.AddListener(() =>
        {
            ProgressBar2.Instance.IncrementMarkerCheck(5);
            MarkerTransition("Quiz");
        });
        // ==============================================================================================
    }

    /// <summary>
    /// Handles the scene transition process to a specific marker scene while pausing AR functionality
    /// </summary>
    /// <param name="sceneName">Target scene name to load</param>
    private void MarkerTransition(string sceneName)
    {
        LevelLoader.Instance.LoadNextScene(sceneName);
        PauseAR();
    }

    /// <summary>
    /// Pauses AR tracking and deactivates all associated objects when leaving the AR scene
    /// All objects connected to the AR will persist between scenes in an inactive state until AR is reactivated
    /// </summary>
    public void PauseAR()
    {
        MindAR.pause();

        SetCameraActive(false);

        Cube1.gameObject.SetActive(false);
        Cube2.gameObject.SetActive(false);
        Cube3.gameObject.SetActive(false);
        Cube4.gameObject.SetActive(false);
        Cube5.gameObject.SetActive(false);
        Cube6.gameObject.SetActive(false);
    }

    /// <summary>
    /// Resumes AR tracking and prepares associated objects when returning to the AR scene
    /// Includes workarounds for MindAR-specific tracking initialization issues
    /// </summary>
    public void UnPauseAR()
    {
        MindAR.unpause();

        SetCameraActive(true);

        // Explicitly deactivates all marker objects as a safeguard against the MindAR.unpause() bug
        // When calling MindAR.unpause(), TargetFound may be falsely triggered, incorrectly activating objects
        // Within ImageTargetMono, inactive objects with enabled scripts will be teleported and reactivated
        // when their actual position changes (when the marker is actually found)
        Cube1.gameObject.SetActive(false);
        Cube2.gameObject.SetActive(false);
        Cube3.gameObject.SetActive(false);
        Cube4.gameObject.SetActive(false);
        Cube5.gameObject.SetActive(false);
        Cube6.gameObject.SetActive(false);
    }

    /// <summary>
    /// Controls AR camera activation state which needs toggling when switching between scenes
    /// </summary>
    /// <param name="active">Whether camera should be active</param>
    public void SetCameraActive(bool active)
    {
        m_Camera.gameObject.SetActive(active);
    }

    // Below is purely debug hotkeys for developers and should be disabled at a later date
    // =================================================================================

    /// <summary>
    /// Processes input each frame for debug controls
    /// </summary>
    private void Update()
    {
        DebugControls();
    }

    /// <summary>
    /// Provides keyboard shortcuts (keys 1-5) for developers to quickly navigate between marker scenes
    /// This function should be removed before final release
    /// </summary>
    private void DebugControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(0);
            MarkerTransition("Marker 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(1);
            MarkerTransition("Marker 2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(2);
            MarkerTransition("Marker 3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(3);
            MarkerTransition("Marker 4");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(4);
            MarkerTransition("Marker 5");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ProgressBar2.Instance.IncrementMarkerCheck(5);
            MarkerTransition("Quiz");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GoToGame();   
        }

    }
    
    public void GoToGame()
    {
        ProgressBar2.Instance.gameObject.SetActive(false);
        MarkerTransition("Level Selection");
    }

    // =================================================================================
}