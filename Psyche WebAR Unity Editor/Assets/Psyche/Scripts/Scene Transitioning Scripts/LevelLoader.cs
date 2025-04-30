using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions with animations and manages scene loading/unloading process
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static string NextScene;         // Name of the target scene to be loaded next in the transition sequence
    public static string PreviousScene;     // Name of the currently active scene that will be unloaded after transition
    [SerializeField] Animator transition;   // Reference to animator component that controls the visual transition effects
    [SerializeField] float transitionTime = 1f;  // Duration in seconds for the transition animation to complete
    private bool inSceneTransition;         // Flag to prevent multiple transitions from happening simultaneously

    /// <summary>
    /// Singleton instance accessible throughout the application for centralized scene management
    /// </summary>
    public static LevelLoader Instance { get; private set; }

    /// <summary>
    /// Sets up singleton pattern during initialization and ensures persistence across scene loads
    /// </summary>
    private void Awake()
    {
        // Standard singleton implementation to maintain only one instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Initiates scene transition to specified target scene with proper setup and validation
    /// </summary>
    /// <param name="NextSceneName">Name of scene to load as registered in the build settings</param>
    public void LoadNextScene(string NextSceneName)
    {
        // Prevent multiple transitions from occurring simultaneously to avoid conflicts
        if (inSceneTransition)
        {
            Debug.LogError("Already in scene transition!");
            return;
        }

        NextScene = NextSceneName;
        PreviousScene = SceneManager.GetActiveScene().name;

        StartCoroutine(LoadLevel());
    }

    /// <summary>
    /// Handles transition animation timing before beginning the actual scene loading process
    /// </summary>
    IEnumerator LoadLevel()
    {
        inSceneTransition = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        StartCoroutine(LoadYourAsyncScene());
    }

    /// <summary>
    /// Asynchronously loads new scene and unloads previous scene with proper sequencing for smooth transitions
    /// </summary>
    IEnumerator LoadYourAsyncScene()
    {
        // Load new scene additively while keeping the current scene active
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        transition.SetTrigger("End");

        // Now that new scene is fully loaded, unload the previous scene
        AsyncOperation asyncLoad2 = SceneManager.UnloadSceneAsync(PreviousScene);
        while (!asyncLoad2.isDone)
        {
            yield return null;
        }
        inSceneTransition = false;
    }
}