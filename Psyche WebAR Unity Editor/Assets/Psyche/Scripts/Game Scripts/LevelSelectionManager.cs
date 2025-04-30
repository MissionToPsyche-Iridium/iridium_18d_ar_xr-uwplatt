using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private float LevelMaxCount;
    [SerializeField] private int CurrentLevelLoaded;
    Dictionary<int, bool> levelUnlocks;
    Dictionary<int, bool> levelCompleted;

    public bool TutorialShown { get; set; }

    /// <summary>
    /// Singleton instance for accessing
    /// </summary>
    public static LevelSelectionManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton pattern
    /// </summary>
    private void Awake()
    {
        // Standard singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Initialize the dictionary with numbers 1 through 8 and default boolean values
        levelUnlocks = new Dictionary<int, bool>();
        levelCompleted = new Dictionary<int, bool>();
        for (int i = 1; i <= LevelMaxCount; i++)
        {
            levelUnlocks[i] = false;
            levelCompleted[i] = false;
        }
        levelUnlocks[1] = true;
    }

    public void GoToLevel(int level)
    {
        if (level == 0)
        {
            CurrentLevelLoaded = 0;
            SceneManager.LoadScene("Level Selection");
            return;
        }

        if (!GetLevelUnlock(level))
            return;

        if (level < 0 || level > LevelMaxCount)
            return;

        

        CurrentLevelLoaded = level;
        string sceneName = "L" + level;
        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextLevel()
    {
        if (CurrentLevelLoaded < LevelMaxCount) 
        { 
            GoToLevel(CurrentLevelLoaded + 1);
        }
    }

    public bool GetLevelUnlock(int level) { return levelUnlocks[level]; }
    public bool GetLevelCompleted(int level) { return levelCompleted[level]; }
    public int GetCurrentLevel() { return CurrentLevelLoaded; }

    public void CurrentLevelCompleted()
    {
        levelCompleted[CurrentLevelLoaded] = true;
        if (CurrentLevelLoaded <= LevelMaxCount)
        {
            SetLevelUnlock(CurrentLevelLoaded + 1, true);
        }
    }

    void SetLevelUnlock(int level, bool value) 
    {   
        levelUnlocks[level] = value; 
    }
}
