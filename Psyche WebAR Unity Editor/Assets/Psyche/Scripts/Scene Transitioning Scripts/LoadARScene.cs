using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles navigation back to the AR scene from marker-specific scenes
/// </summary>
public class LoadARScene : MonoBehaviour
{
    [SerializeField] private Button BackButton;    // Reference to UI button that returns user to main AR scene

    /// <summary>
    /// Sets up the back button's functionality when the scene initializes
    /// </summary>
    void Start()
    {
        // Configures back button to reactivate AR tracking and return to the main AR interface
        BackButton.onClick.AddListener(() =>
        {
            if (MarkerSceneManager.Instance != null) MarkerSceneManager.Instance.UnPauseAR();
            if (LevelLoader.Instance != null)
            {
                ProgressBar2.Instance.gameObject.SetActive(true);
                LevelLoader.Instance.LoadNextScene("MindAR");
            }
            else Debug.Log("Cannot travel back to the MindAR scene if you didn't start there!");
        });
    }
}