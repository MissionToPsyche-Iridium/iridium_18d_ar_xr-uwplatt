using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the progress bar between scenes.
/// </summary>
public class ProgressBar2 : MonoBehaviour
{
    // UI slider representing the progress bar
    [SerializeField] private Slider slider;

    // Particle system for bar movement effect (TODO)
    // private ParticleSystem particleSystem;

    // Target value the bar should transition to
    private float targetValue;

    // Speed of the progress transition
    public float speed = 0.5f;

    // Tracks which markers have been activated (5 total)
    private bool[] markerCheck = new bool[] { false, false, false, false, false, false };

    // Singleton instance for global access
    public static ProgressBar2 Instance { get; private set; }

    /// <summary>
    /// Unity Awake callback. Sets up singleton instance and ensures persistence across scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Unity Start callback. Called before the first frame update.
    /// </summary>
    public void Start()
    {
        // IncrementProgress(0.75f);
    }

    /// <summary>
    /// Unity Update callback. Called once per frame to update progress bar smoothly.
    /// </summary>
    public void Update()
    {
        if (slider.value < targetValue)
        {
            slider.value += speed * Time.deltaTime;

            // if (!particleSystem.isPlaying)
            //     particleSystem.Play();
        }
        // else
        //     particleSystem.Stop();
    }

    /// <summary>
    /// Increments the progress bar by a given amount.
    /// </summary>
    /// <param name="newProgress">Value to add (from 0.0f to 1.0f).</param>
    public void IncrementProgress(float newProgress)
    {
        if (slider.value + newProgress <= 1f)
            targetValue = slider.value + newProgress;
        else
            targetValue = 1f;
    }

    /// <summary>
    /// Marks a milestone and updates progress if not already counted.
    /// </summary>
    /// <param name="markerNum">Index of the marker (0 to 4).</param>
    public void IncrementMarkerCheck(int markerNum){
        //Debug.Log("help me "+markerNum);
        if(!markerCheck[markerNum]){
            IncrementProgress(1.0f/6.0f);
            markerCheck[markerNum]=true;
        }
    }
}
