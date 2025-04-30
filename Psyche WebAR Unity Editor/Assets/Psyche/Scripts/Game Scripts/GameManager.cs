using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private Button MenuButton;
    [SerializeField] private Transform Menu;
    [SerializeField] private Button LevelSelection;
    [Space]
    [Header("View Level UI")]
    [SerializeField] private Button ViewLevel;
    [SerializeField] private Transform PanScreenIcon;
    [Space]
    [Header("Space Faring UI")]
    [SerializeField] private Button RestartLevel;
    [SerializeField] private Transform SpaceFaringUI;
    [SerializeField] private Transform MovementButtons;
    [SerializeField] private Button LeftArrow;
    [SerializeField] private Button RightArrow;
    [SerializeField] private Button UpArrow;
    [SerializeField] private Button DownArrow;
    [SerializeField] private TextMeshProUGUI FuelBarText;
    [SerializeField] private RectTransform FuelBarLevel;
    [Space]
    [Header("Level Complete UI")]
    [SerializeField] private Transform LevelCompleteUI;
    [SerializeField] private Button Complete_Next;
    [SerializeField] private Button Complete_Restart;
    [SerializeField] private Button Complete_LevelSelection;
    [SerializeField] private TextMeshProUGUI LevelCompleteTitle;
    [Space]
    [Header("Other UI")]
    [SerializeField] private Button BigResetButton;
    [SerializeField] private float BigResetButtonDelay = 1.8f;
    [SerializeField] private Transform PullBackText;
    [SerializeField] private Transform TutorialPrefabObject;

    private GameCamera MainCamera;
    private Player Satellite;
    private float InitialFuelBarHeight;
    private bool LevelCompleted = false;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    /// <summary>
    /// Singleton instance providing global access
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Establishes singleton pattern during initialization
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
    }

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameCamera.Instance;
        Satellite = Player.Instance;

        HorizontalInput = 0;
        VerticalInput = 0;
        InitialFuelBarHeight = FuelBarLevel.sizeDelta.y;

        MenuButton.onClick.AddListener(() => { SetUIVisibility(Menu, !Menu.gameObject.activeSelf); });
        LevelSelection.onClick.AddListener(() => { GoToLevelSelection(); });
        ViewLevel.onClick.AddListener(() => { ViewLevelToggle(); });
        RestartLevel.onClick.AddListener(() => { ResetLevel(); });
        BigResetButton.onClick.AddListener(() => { ResetLevel(); });
        Complete_LevelSelection.onClick.AddListener(() => { GoToLevelSelection(); });
        Complete_Next.onClick.AddListener(() => { GoToNextLevel(); });
        Complete_Restart.onClick.AddListener(() => { ResetLevel(); });

        AddHoldListener(LeftArrow.gameObject, () => HorizontalInput = -1, () => HorizontalInput = 0);
        AddHoldListener(RightArrow.gameObject, () => HorizontalInput = 1, () => HorizontalInput = 0);
        AddHoldListener(UpArrow.gameObject, () => VerticalInput = 1, () => VerticalInput = 0);
        AddHoldListener(DownArrow.gameObject, () => VerticalInput = -1, () => VerticalInput = 0);

        SetUIVisibility(PanScreenIcon, false);
        SetUIVisibility(Menu, false);
        SetUIVisibility(SpaceFaringUI, false);
        SetUIVisibility(BigResetButton.transform, false);
        SetUIVisibility(LevelCompleteUI, false);
    }

    private void Update()
    {
        if (Menu.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            if(!IsPointerOverUI())
            {
                SetUIVisibility(Menu, false);
            }
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetUIVisibility(Transform obj, bool active)
    {
        obj.gameObject.SetActive(active);
    }

    public void ToggleViewLevel(bool active)
    {
        SetUIVisibility(ViewLevel.transform, active);
    }

    public void PlayerDestroyed()
    {
        SetUIVisibility(SpaceFaringUI, false);
        Invoke("DelayBigResetButton", BigResetButtonDelay);
        Player.Instance.ChangePlayerState(PlayerState.Destroyed);
    }

    public void TogglePullBackText(bool active)
    {
        SetUIVisibility(PullBackText.transform, active);
    }

    void GoToLevelSelection()
    {
        // Go to the level selection scene
        if (LevelSelectionManager.Instance != null)
            LevelSelectionManager.Instance.GoToLevel(0);
        else
            Debug.LogWarning("Need to start at Level Selection scene to GoToLevelSelection !");
    }

    void GoToNextLevel()
    {
        if (LevelSelectionManager.Instance != null)
            LevelSelectionManager.Instance.GoToNextLevel();
        else
            Debug.LogWarning("Need to start at Level Selection scene to GoToNextLevel !");
    }

    void ViewLevelToggle()
    {
        if (MainCamera.GetCameraState() == GameCameraState.Starting)
        {
            SetUIVisibility(PanScreenIcon, true);
            MainCamera.ChangeCameraState(GameCameraState.GameView);
            Satellite.ChangePlayerState(PlayerState.Disabled);
        }
        else
        {
            SetUIVisibility(PanScreenIcon, false);
            MainCamera.ChangeCameraState(GameCameraState.Starting);
            Satellite.ChangePlayerState(PlayerState.Launching);
        }
    }

    public void LaunchSatellite()
    {
        MainCamera.ChangeCameraState(GameCameraState.Centered);
        Satellite.ChangePlayerState(PlayerState.SpaceFaring);
        ToggleViewLevel(false);
        SetUIVisibility(SpaceFaringUI, true);
    }

    void DelayBigResetButton()
    {
        if(!LevelCompleted)
            SetUIVisibility(BigResetButton.transform, true);
    }

    void AddHoldListener(GameObject buttonObj, UnityEngine.Events.UnityAction onDown, UnityEngine.Events.UnityAction onUp)
    {
        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        // PointerDown
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((eventData) => { onDown(); });
        trigger.triggers.Add(entryDown);

        // PointerUp
        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((eventData) => { onUp(); });
        trigger.triggers.Add(entryUp);
    }

    public void GameManagerLevelComplete()
    {
        LevelCompleted = true;
        int currentLevel = 0;
        if (LevelSelectionManager.Instance != null)
        {
            LevelSelectionManager.Instance.CurrentLevelCompleted();
            currentLevel = LevelSelectionManager.Instance.GetCurrentLevel();
        }
        else
            Debug.LogWarning("Need to start at Level Selection scene to update level completed!");

        LevelCompleteTitle.text = "Level " + currentLevel + "\nComplete!";
        SetUIVisibility(LevelCompleteUI, true);
        SetUIVisibility(SpaceFaringUI, false);
        SetUIVisibility(Menu, false);
        SetUIVisibility(MenuButton.transform, false);
        SetUIVisibility(BigResetButton.transform, false);
        SetUIVisibility(TutorialPrefabObject, false);

        Satellite.ChangePlayerState(PlayerState.Disabled);
    }

    public void UpdateFuelBar(float fuel, float maxFuelAmount)
    {
        fuel = Mathf.RoundToInt(fuel);
        FuelBarText.text = fuel.ToString();
        float heightRatio = fuel / maxFuelAmount;
        FuelBarLevel.sizeDelta = new Vector2(FuelBarLevel.sizeDelta.x, heightRatio * InitialFuelBarHeight);

        if (fuel <= 0)
        {
            SetUIVisibility(MovementButtons, false);
            Invoke("DelayBigResetButton", BigResetButtonDelay);
        }
    }
}
