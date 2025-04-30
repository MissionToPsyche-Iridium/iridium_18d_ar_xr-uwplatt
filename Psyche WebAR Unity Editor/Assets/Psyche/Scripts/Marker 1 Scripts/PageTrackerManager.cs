using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the visual indicators that show which page is currently active
/// </summary>
public class PageTrackerManager : MonoBehaviour
{
    // UI Elements
    [SerializeField] private Image[] CircleTrackers = new Image[4];  // Array of dot indicators, one for each page

    // Color settings
    [SerializeField] private Color activeColor;     // Color for the currently active page indicator
    [SerializeField] private Color inactiveColor;   // Color for inactive page indicators

    /// <summary>
    /// Initialize the UI with the first page indicator active
    /// </summary>
    public void Start()
    {
        // Set the first circle tracker to active color on start
        CircleTrackers[0].color = activeColor;
    }

    /// <summary>
    /// Updates the page indicators to show the current active page
    /// </summary>
    /// <param name="currentChild">Index of the currently active page</param>
    public void UpdateUI(int currentChild)
    {
        // Reset all indicators to inactive
        foreach (Image image in CircleTrackers)
        {
            image.color = inactiveColor;
        }

        // Set the current page indicator to active
        if (currentChild + 1 <= CircleTrackers.Length) 
        {
            CircleTrackers[currentChild].color = activeColor;
        }
        else
        {
            Debug.LogError("Index Out of Bound for PageTrackerManager: " + currentChild + " " + CircleTrackers.Length);
        }
        
    }
}