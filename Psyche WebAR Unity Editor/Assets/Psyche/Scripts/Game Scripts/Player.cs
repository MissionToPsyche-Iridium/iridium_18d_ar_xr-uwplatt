using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Disabled, Launching, SpaceFaring, Destroyed }

public class Player : MonoBehaviour
{
    [Header("Current Player State")]
    [SerializeField] private PlayerState playerState;
    [Space(20)]
    [Header("In Space Faring State")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fuelLossSpeed;
    [SerializeField] private float maxFuelAmount;
    [SerializeField] private float fuel;
    [Space(20)]
    [Header("In Launching State")]
    [SerializeField] private float launchPower = 10f;
    [SerializeField] private Collider2D LaunchCollider;
    [SerializeField] private float maxDragRadius = 3f;
    [SerializeField] private float rotateSmoothSpeed = 10f;
    [Space(20)]
    [Header("Arrow Visuals")]
    [SerializeField] private Transform arrow;                    // Assign in Inspector
    [SerializeField] private float maxArrowScale = 2f;           // Max scale factor for arrow length
    [SerializeField] private float arrowFollowSpeed = 10f;       // How fast the arrow trails (higher = snappier)
    [Space(20)]
    [Header("Thruster Particles")]
    [SerializeField] private ParticleSystem SteamTopLeft;
    [SerializeField] private ParticleSystem SteamTopRight;
    [SerializeField] private ParticleSystem SteamBottomLeft;
    [SerializeField] private ParticleSystem SteamBottomRight;
    [SerializeField] private ParticleSystem FlameStream;
    [SerializeField] private float FlameTimer = 1f;
    [Space(20)]
    [Header("Spacecraft Destroyed")]
    [SerializeField] private GameObject SatelliteBigExplosion;
    [SerializeField] private SpriteRenderer PlayerImage;

    private Rigidbody2D rb2d;
    private Vector2 startPos;
    private Quaternion startRot;
    private ArrowFader arrowFader;
    public bool IsDragging { get; set; }
    public bool InGravityWell { get; set; }

    /// <summary>
    /// Singleton instance providing global access
    /// </summary>
    public static Player Instance { get; private set; }

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

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        InGravityWell = false;
        playerState = PlayerState.Launching;
        startPos = rb2d.position;
        startRot = transform.rotation;
        rb2d.isKinematic = true;
        IsDragging = false;
        arrowFader = arrow.GetComponent<ArrowFader>();
        fuel = maxFuelAmount;

        SteamTopLeft.Stop();
        SteamTopRight.Stop();
        SteamBottomLeft.Stop();
        SteamBottomRight.Stop();
        FlameStream.Stop();
    }

    private void Update()
    {
        switch (playerState)
        {
            default: break;
            case PlayerState.Launching:     InLaunching();            break;
            case PlayerState.SpaceFaring:   InSpaceFaringVisual();    break;
        }
    }

    private void FixedUpdate()
    {
        if (playerState == PlayerState.SpaceFaring)
            InSpaceFaringPhysics();
    }

    public void ChangePlayerState(PlayerState state) { playerState = state; }

    void InLaunching()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.ToggleViewLevel(!IsDragging);
        GameManager.Instance.TogglePullBackText(!IsDragging);
        bool MouseOverLaunchCollider = LaunchCollider.OverlapPoint(mouseWorldPos);

        if (IsDragging && !MouseOverLaunchCollider)
            arrowFader.FadeIn();
        else
            arrowFader.FadeOut();

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseOverLaunchCollider)
            {
                IsDragging = true;
            }
        }

        if (IsDragging && Input.GetMouseButton(0))
        {
            Vector2 dragVector = mouseWorldPos - startPos;

            if (dragVector.magnitude > maxDragRadius)
            {
                dragVector = dragVector.normalized * maxDragRadius;
            }

            rb2d.position = startPos + dragVector;

            // Smooth player rotation
            Vector2 lookDir = startPos - rb2d.position;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                float playerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, playerAngle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSmoothSpeed);
            }

            // Update arrow visuals
            if (arrow != null)
            {
                Vector2 launchDir = startPos - rb2d.position;
                float distance = launchDir.magnitude;

                // Position arrow
                arrow.position = Vector2.Lerp(arrow.position, startPos, Time.deltaTime * arrowFollowSpeed);

                // Rotate arrow
                float angle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, angle);
                arrow.rotation = Quaternion.Lerp(arrow.rotation, targetRot, Time.deltaTime * arrowFollowSpeed);

                // Scale arrow based on distance
                float scale = Mathf.Min(distance / maxDragRadius, 1f) * maxArrowScale;
                Vector3 newScale = new Vector3(scale, arrow.localScale.y, arrow.localScale.z);
                arrow.localScale = Vector3.Lerp(arrow.localScale, newScale, Time.deltaTime * arrowFollowSpeed);
            }
        }

        if (IsDragging && Input.GetMouseButtonUp(0))
        {
            Vector2 releasePos = rb2d.position;
            Vector2 launchDir = (startPos - releasePos);

            if (MouseOverLaunchCollider)
            {
                rb2d.position = startPos;
                transform.rotation = startRot;
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true;
            }
            else
            {
                LaunchPlayer(launchDir);
            }

            IsDragging = false;
        }

    }

    void InSpaceFaringVisual()
    {
        if (fuel <= 0)
        {
            SteamTopLeft.Stop();
            SteamTopRight.Stop();
            SteamBottomLeft.Stop();
            SteamBottomRight.Stop();
            return;
        }

        float horizontalInput = GameManager.Instance.HorizontalInput;
        float verticalInput = GameManager.Instance.VerticalInput;

        // Steam Particles
        bool playTopLeft = false;
        bool playTopRight = false;
        bool playBottomLeft = false;
        bool playBottomRight = false;

        // Decide which systems should be playing
        if (horizontalInput == -1)
        {
            playTopLeft = true;
            playBottomRight = true;
        }
        else if (horizontalInput == 1)
        {
            playTopRight = true;
            playBottomLeft = true;
        }

        if (verticalInput == 1)
        {
            playBottomLeft = true;
            playBottomRight = true;
        }
        else if (verticalInput == -1)
        {
            playTopLeft = true;
            playTopRight = true;
        }

        // Activate required systems
        if (playTopLeft) SteamBurst(SteamTopLeft); else SteamTopLeft.Stop();
        if (playTopRight) SteamBurst(SteamTopRight); else SteamTopRight.Stop();
        if (playBottomLeft) SteamBurst(SteamBottomLeft); else SteamBottomLeft.Stop();
        if (playBottomRight) SteamBurst(SteamBottomRight); else SteamBottomRight.Stop();
    }

    void InSpaceFaringPhysics()
    {
        if (fuel <= 0)
            return;

        float horizontalInput = GameManager.Instance.HorizontalInput;
        float verticalInput = GameManager.Instance.VerticalInput;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            fuel -= fuelLossSpeed * Time.fixedDeltaTime;
            GameManager.Instance.UpdateFuelBar(fuel, maxFuelAmount);
        }

        if (verticalInput != 0)
        {
            Vector2 movement = transform.right * moveSpeed * verticalInput * Time.fixedDeltaTime;
            rb2d.AddForce((Vector3)movement);
        }

        float rotateAmount = -horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        rb2d.MoveRotation(rb2d.rotation + rotateAmount);
    }

    void LaunchPlayer(Vector2 launchDir)
    {
        // Snap arrow rotation
        float angle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        Quaternion arrowTargetRot = Quaternion.Euler(0, 0, angle);
        arrow.rotation = arrowTargetRot;

        // Snap player rotation
        Vector2 lookDir = startPos - rb2d.position;
        float playerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion playerTargetRot = Quaternion.Euler(0, 0, playerAngle);
        transform.rotation = playerTargetRot;

        // Launch player
        rb2d.isKinematic = false;
        rb2d.velocity = launchDir * launchPower;
        StartCoroutine(LaunchRocketFlames());
        GameManager.Instance.LaunchSatellite();
    }

    void SteamBurst(ParticleSystem particle)
    {
        if (!particle.isPlaying)
        {
            particle.Stop();
            particle.Clear();
            particle.Play();
        }
    }

    IEnumerator LaunchRocketFlames()
    {
        FlameStream.Play();
        yield return new WaitForSeconds(FlameTimer);
        FlameStream.Stop();
    }

    void CrashedSpacecraft()
    {
        // Instantiate the prefab at the specified world position with no rotation.
        GameObject newObject = Instantiate(SatelliteBigExplosion, transform.position, Quaternion.identity);
        // Destroy object after particle effect ends
        Destroy(newObject, 3f);

        SteamTopLeft.Stop();
        SteamTopRight.Stop();
        SteamBottomLeft.Stop();
        SteamBottomRight.Stop();
        FlameStream.Stop();
        PlayerImage.color = Color.black;
        GameManager.Instance.PlayerDestroyed();
    }

    void CrashedIntoSun()
    {
        rb2d.velocity = rb2d.velocity / 3;
        CrashedSpacecraft();
    }

    void PlayerLevelComplete()
    {
        SetStatic();
        GameManager.Instance.GameManagerLevelComplete();
    }

    void SetStatic()
    {
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (playerState == PlayerState.SpaceFaring)
        {
            switch (col.gameObject.tag)
            {
                default: break;
                case "Asteroid": CrashedSpacecraft(); break;
                case "Border Wall": CrashedSpacecraft(); break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (playerState == PlayerState.SpaceFaring)
        {
            switch (col.gameObject.tag)
            {
                default: break;
                case "Flag": PlayerLevelComplete(); break;
                case "Sun": CrashedIntoSun(); break;
            }
        }
        else if (col.gameObject.tag == "InternalSun")
            SetStatic();
    }

    public PlayerState GetPlayerState() { return playerState; }
}
