using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Sets TextMeshPro text content from marker data stored in JSON
/// </summary>
public class TextSetterFromJSON : MonoBehaviour
{
    [SerializeField] private string markerName;                  // Identifier for which marker data to retrieve
    [SerializeField] private MarkerManager.DataType dataType;    // Specifies which content field to display (main or additional)

    /// <summary>
    /// Initializes text component with content from the marker manager when enabled
    /// </summary>
    void Start()
    {
        TextMeshProUGUI textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
            textComponent.text = MarkerManager.Instance.GetMarkerInfo(markerName, dataType);
        else
            Debug.LogError("Script was not attached to an object with a TextMeshProUGUI component!");
    }
}