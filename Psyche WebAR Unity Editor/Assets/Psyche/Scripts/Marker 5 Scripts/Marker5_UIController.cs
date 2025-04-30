using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages all user interface components related to satellite orbit interaction.
/// Controls the display of orbit-specific popups, navigation buttons, and orbit info text.
/// Also toggles the satellite/asteroid model and the orbit path renderer.
/// </summary>
public class Marker5_UIController : MonoBehaviour
{
    // UI Components
    [SerializeField] private ToggleUI Popup_1;    // UI popup for Orbit A
    [SerializeField] private ToggleUI Popup_2;    // UI popup for Orbit B
    [SerializeField] private ToggleUI Popup_3;    // UI popup for Orbit C
    [SerializeField] private ToggleUI Popup_4;    // UI popup for Orbit D
    [SerializeField] private ToggleUI Popup_5;    // UI popup for Orbit E

    // Navigation and information elements
    [SerializeField] private Button InfoButton;         // Button to display orbit information
    [SerializeField] private ToggleUI Marker5Buttons;   // Container for UI buttons
    [SerializeField] private Button nextButton;         // Button to advance to next orbit
    [SerializeField] private Button previousButton;     // Button to return to previous orbit
    [SerializeField] private TextMeshProUGUI InfoText;  // Text display for orbit information
    [SerializeField] private TextMeshProUGUI nextText;  // Text display of next orbit name
    [SerializeField] private TextMeshProUGUI prevText;  // Text display of the previous orbit name

    // Satellite/Asteroid Models
    [SerializeField] private ToggleUI satelliteAsteroidModels;
    [SerializeField] private CircularPathFollow satelliteLineRenderer;

    /// <summary>
    /// Start UI element with first orbit position
    /// </summary>
    private void Start()
    {
        ChangeInfoText(1);
    }

    /// <summary>
    /// Toggles visibility of UI components based on current orbit state
    /// </summary>
    public void ShowUIPopup()
    {
        // Toggle main UI button container
        Marker5Buttons.ToggleUIComponent();
        InfoButton.interactable = !InfoButton.interactable;
        nextButton.interactable = !nextButton.interactable;
        previousButton.interactable = !previousButton.interactable;

        // Show the appropriate popup based on current orbit state
        switch (Marker5_SatelliteController.Instance.GetCurrentOrbitState())
        {
            case OrbitState.ZoomedOut:
                // No popup shown in zoomed out state
                break;
            case OrbitState.OrbitA:
                Popup_1.ToggleUIComponent();
                break;
            case OrbitState.OrbitB:
                Popup_2.ToggleUIComponent();
                break;
            case OrbitState.OrbitC:
                Popup_3.ToggleUIComponent();
                break;
            case OrbitState.OrbitD:
                Popup_4.ToggleUIComponent();
                break;
            case OrbitState.OrbitE:
                Popup_5.ToggleUIComponent();
                break;
            default:
                Debug.LogError("We got issues with an invalid orbit state! In ShowUIPopup");
                break;
        }

        // Toggle the Asteroid/Satellite Models
        satelliteAsteroidModels.ToggleUIComponent();
        satelliteLineRenderer.ToggleRadius();
    }

    /// <summary>
    /// Updates UI elements based on selected orbit position
    /// </summary>
    /// <param name="orbit_num">Numeric identifier for orbit position (1-6)</param>
    public void ChangeInfoText(int orbit_num)
    {
        // Enable/disable navigation buttons based on position
        nextButton.interactable = orbit_num < 6;      // Disable next button at last orbit
        previousButton.interactable = orbit_num > 1;  // Disable previous button at first orbit
        InfoButton.interactable = true;

        // Update information text based on current orbit position
        switch (orbit_num)
        {
            case 1:
                InfoButton.interactable = false;  // Disable info button in zoomed out view
                prevText.text = "--";
                InfoText.text = "Info";
                nextText.text = ">> Orbit A";
                break;
            case 2:
                prevText.text = "Solar System <<";
                InfoText.text = "Orbit A";
                nextText.text = ">> Orbit B1";
                break;
            case 3:
                prevText.text = "Orbit A <<";
                InfoText.text = "Orbit B1";
                nextText.text = ">> Orbit D";
                break;
            case 4:
                prevText.text = "Orbit B1 <<";
                InfoText.text = "Orbit D";
                nextText.text = ">> Orbit C";
                break;
            case 5:
                prevText.text = "Orbit D <<";
                InfoText.text = "Orbit C";
                nextText.text = ">> Orbit B2";
                break;
            case 6:
                prevText.text = "Orbit C <<";
                InfoText.text = "Orbit B2";
                nextText.text = "--";
                break;
            default:
                Debug.LogError("We got issues with an invalid orbit state! In ChangeInfoText");
                break;
        }
    }
}