using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GameCameraState { Starting, GameView, Centered};

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform Satellite;
    [SerializeField] private Transform LaunchPosition;
    [SerializeField] private BoxCollider2D BorderCollider;
    [SerializeField] private float StartingPositionYOffset = 55f;
    [SerializeField] private float CenterPositionYOffset = 20f;
    [SerializeField] private float CameraSpeed = 2.8f;
    [SerializeField] private float OrthographicSizeSpeed = 0.3f;
    [SerializeField] private float MaximumOrthographicSize = 150f;
    [SerializeField] private float MinumumOrthographicSize = 75f;
    [Space]
    [SerializeField] private float panningSpeedModifier = 0.1f;
    [Space]
    [SerializeField] private GameCameraState State;

    private Camera _camera;
    private Player playerScript;
    private float Z_pos;

    private bool isDragging;
    private Vector2 currentMousePos;

    /// <summary>
    /// Singleton instance providing global access
    /// </summary>
    public static GameCamera Instance { get; private set; }

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
        State = GameCameraState.Starting;
        _camera = GetComponent<Camera>();
        Z_pos = transform.position.z;
        playerScript = Satellite.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameCameraState.Starting:
                StartingCamera();
                break;
            case GameCameraState.GameView:
                GameViewCamera();
                break;
            case GameCameraState.Centered:
                CenteredCamera();
                break;
        }

        CameraDebug();
    }

    private void LateUpdate()
    {
        Bounds bounds = BorderCollider.bounds;

        // Get camera dimensions
        float camHeight = _camera.orthographicSize;
        float camWidth = _camera.aspect * camHeight;

        // Clamp position so the *edges* of the camera stay within bounds
        float minX = bounds.min.x + camWidth;
        float maxX = bounds.max.x - camWidth;
        float minY = bounds.min.y + camHeight;
        float maxY = bounds.max.y - camHeight;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void ChangeCameraState(GameCameraState state)
    {
        this.State = state;
    }

    void StartingCamera()
    {
        var step = CameraSpeed * Time.deltaTime; // calculate distance to move
        Vector3 pos = new Vector3(LaunchPosition.position.x, LaunchPosition.position.y + StartingPositionYOffset, Z_pos);
        transform.position = Vector3.Lerp(transform.position, pos, step);
    }

    void GameViewCamera()
    {
        // If the mouse is clicked over a UI element return
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDragging)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                    return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isDragging = true;
            currentMousePos = Input.mousePosition;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            isDragging = false;

        if (isDragging && currentMousePos != (Vector2)Input.mousePosition)
        {
            Vector2 dragVector = (Vector2)Input.mousePosition - currentMousePos;

            Vector2 camOffset = -dragVector * panningSpeedModifier / (Z_pos / gameObject.transform.position.z);
            Vector2 camPosWithOffset = new Vector2(
                gameObject.transform.position.x + camOffset.x,
                gameObject.transform.position.y + camOffset.y);

            gameObject.transform.position = new Vector3(camPosWithOffset.x, camPosWithOffset.y, gameObject.transform.position.z);

            currentMousePos = (Vector2)Input.mousePosition;
        }
    }

    void CenteredCamera()
    {
        var step = CameraSpeed * Time.deltaTime; // calculate distance to move
        Vector3 pos = new Vector3(Satellite.position.x, Satellite.position.y + CenterPositionYOffset, Z_pos);
        transform.position = Vector3.Slerp(transform.position, pos, step);

        if (playerScript.InGravityWell)
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, MinumumOrthographicSize, OrthographicSizeSpeed * Time.deltaTime);
        else
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, MaximumOrthographicSize, OrthographicSizeSpeed * Time.deltaTime);
    }

    void CameraDebug()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeCameraState(GameCameraState.Starting);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeCameraState(GameCameraState.GameView);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeCameraState(GameCameraState.Centered);
        }
    }

    public GameCameraState GetCameraState() { return State; }
}


