using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

/*
!!!-----PLEASE PUT THE JSON FILE IN THE STREAMING ASSETS FOLDER-----!!!
*/

/// <summary>
/// Data class representing individual marker entries from the JSON file with name and text content fields
/// <summary>
[Serializable]
public class MarkerTextData
{
    public string name;              // Identifier for the marker (e.g., "Marker 1")
    public string mainData;          // Primary content text to display for this marker
    public string additionalInfo;    // Secondary/supplemental information for this marker
}

/// <summary>
/// Container class that holds the complete collection of marker data loaded from JSON
/// <summary>

[Serializable]
public class MarkerDatabase
{
    public List<MarkerTextData> markers;  // Collection of all marker data entries
}

/// <summary>
/// Manages loading and retrieval of marker text data from JSON configuration file across different build platforms
/// </summary>
public class MarkerManager : MonoBehaviour
{
    // Path to JSON file - default name is markers.json (adjust line 46 if filename changes)
    private string jsonFilePath;
    private MarkerDatabase markerDatabase;  // Parsed JSON data container

    [Header("Testing Variables")]
    [SerializeField] private string markerName = "Marker 1";  // Test marker identifier for editor testing
    [SerializeField] private DataType dataType = DataType.MainData;  // Which data field to retrieve in tests

    /// <summary>
    /// Singleton instance for global access to marker data
    /// </summary>
    public static MarkerManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance and loads JSON data with platform-specific approaches
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Debug.Log("Second instance of MarkerManager detected!");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Uses different JSON loading methods between Unity Editor and WebGL build
        // System.IO file system doesn't work on WebGL, requiring UnityWebRequest instead
#if UNITY_EDITOR
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, "markers.json");
        LoadJsonData();
#elif UNITY_WEBGL
        StartCoroutine(GetJsonPath("markers.json"));      
#endif
    }

    /// <summary>
    /// Loads JSON data via web request for WebGL builds where direct file access isn't possible
    /// </summary>
    IEnumerator GetJsonPath(string file)
    {
        string path = "StreamingAssets/" + file;
        UnityWebRequest uwr = UnityWebRequest.Get(path);
        yield return uwr.SendWebRequest();
        Debug.Log("responseCode:" + uwr.responseCode);
        if (uwr.isDone)
        {
            string jsonData = uwr.downloadHandler.text;
            //Debug.Log(jsonData);
            markerDatabase = JsonUtility.FromJson<MarkerDatabase>(jsonData);
        }
    }

    /// <summary>
    /// Defines which data field to retrieve from marker entries
    /// </summary>
    public enum DataType
    {
        MainData,
        AdditionalInfo
    }

    /// <summary>
    /// Loads and parses JSON data from file system - Editor-only function as System.IO doesn't work in WebGL
    /// </summary>
#if UNITY_EDITOR
    void LoadJsonData()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            markerDatabase = JsonUtility.FromJson<MarkerDatabase>(jsonData);
        }
        else
        {
            Debug.LogError("JSON filepath incorrect - Check directory location Please, please, please");
        }
    }
#endif

    /// <summary>
    /// Provides easy testing access from Unity Editor context menu for debugging
    /// </summary>
    [ContextMenu("Get Marker Info")]
    public void GetMarkerInfoFromContextMenu()
    {
        string result = GetMarkerInfo(markerName, dataType);
        Debug.Log(result);
    }

    /// <summary>
    /// Retrieves specified marker information by name and content type from the loaded database
    /// </summary>
    /// <param name="markerName">The identifier of the marker to find</param>
    /// <param name="dataType">Which data field to return (MainData or AdditionalInfo)</param>
    /// <returns>The requested text content or error message if not found</returns>
    public string GetMarkerInfo(string markerName, DataType dataType)
    {
        // Validates database is properly loaded before attempting to access data
        if (markerDatabase != null && markerDatabase.markers != null)
        {
            MarkerTextData marker = markerDatabase.markers.Find(m => m.name.Equals(markerName, StringComparison.OrdinalIgnoreCase));
            if (marker != null)
            {
                // Returns the appropriate field based on requested data type
                if (dataType == DataType.MainData)
                {
                    return $"{marker.mainData}";
                }
                else if (dataType == DataType.AdditionalInfo)
                {
                    return $"{marker.additionalInfo}";
                }
                else
                {
                    return "Invalid enum type specified.";
                }
            }
        }
        return "Marker not found";
    }
}