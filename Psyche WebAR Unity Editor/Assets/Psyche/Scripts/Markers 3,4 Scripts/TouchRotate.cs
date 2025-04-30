using UnityEngine;

// Script to rotate 3d model with touch input and mouse input for devs. (your welcome)
/* Optional Parameters:
        Use Hitbox: For adding a hitbox where only touches within box will affect rotation
                Hitbox Width
                Hitbox Height
        Use Custom Center: For changing where center of hitbox is (default is center of object)
                X Coordinate
                Y Coordinate
        Show Hitbox: Draws box on screen over hitbox, for debugging purposes

*/ 


/// <summary>
/// Script to rotate 3D model with touch input. Has undergone several revisions: hence the 
/// large swaths of commented out code.
/// </summary>
public class TouchRotate : MonoBehaviour
{
    // Control variables
    private Vector2 lastTouchPosition;     // Stores previous touch/mouse position for delta calculations
    private bool isDragging = false;
    private float rotation_speed = 30f;    // Controls rotation sensitivity
    private Quaternion targetRotation;     // Target rotation for smooth interpolation

    // Optional hitbox parameter
    public bool useHitbox = false;
    [SerializeField] private BoxCollider2D OptionalHitbox;
    //public float hitboxWidth = 200f;
    //public float hitboxHeight = 200f;

    // Optional parameter for center of box
    //public bool useCustomCenter = false;
    //public Vector2 customHitboxCenter;

    // Optional toggle to show hitbox on screen (for debug purposes)
    //public bool showHitbox = false;

    // Track whether current drag/touch began inside the box
    private bool touchStartedInBox = false;

    /// <summary>
    /// Initialize target rotation on start
    /// </summary>
    private void Start()
    {
        // Initialize target rotation to rotation at start
        targetRotation = transform.rotation;
    }

    /// <summary>
    /// Process input and handle rotation each frame
    /// </summary>
    void Update()
    {
        // I currently am not testing this on a phone.
        // I don't fully know how to deal with multiple touches at once. In future make it so that original touch takes priority and subsequent touches are ignored.
        // Check if there's a touch on the screen
        if (Input.touchCount > 0)
        {
            // Touch touch = Input.GetTouch(0); is one of the goofiest lines of code I've seen.
            // Got needed information from https://discussions.unity.com/t/how-can-i-drag-the-object-with-touch-mobile/93379
            // Set touch to first read touch
            Touch touch = Input.GetTouch(0);

            // If touch has initially touched down:
            if (touch.phase == TouchPhase.Began)
            {
                // If touch started in hitbox, update last touch position with current position
                touchStartedInBox = IsTouchWithinBounds(touch.position);
                if (touchStartedInBox)
                {
                    lastTouchPosition = touch.position;
                }
            }
            // If touch is moving and it started in box:
            else if (touch.phase == TouchPhase.Moved && touchStartedInBox)
            {
                // compute rotation velocity based off how quickly user touch positon has changed.
                Vector2 delta = touch.position - lastTouchPosition;
                RotateModel(delta);
                lastTouchPosition = touch.position;
            }
            // When touch ends, reset started in box flag
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchStartedInBox = false;
            }
        }

        // Handle mouse input for testing
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = IsTouchWithinBounds(Input.mousePosition);
            if (isDragging)
            {
                lastTouchPosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            RotateModel(delta);
            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Apply smooth rotation transition
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime);
    }

    /// <summary>
    /// Calculate and apply rotation based on input delta
    /// </summary>
    /// <param name="delta">Change in touch/mouse position</param>
    private void RotateModel(Vector2 delta)
    {
        // I dont...
        // This is not ideal, however it doesnt snap:
        // Still has weird controls at certain angles

        /*Vector3 localRight = transform.right;
        Quaternion yRotation = Quaternion.AngleAxis(-delta.x * rotationSpeed, Vector3.up);
        Quaternion xRotation = Quaternion.AngleAxis(delta.y * rotationSpeed, localRight);
        transform.rotation = yRotation * transform.rotation * xRotation;
        targetRotation = transform.rotation;*/

        // Create rotation axis from input delta
        Vector3 axis = new Vector3(delta.y, -delta.x, 0); // X inverted for intuitive feel
        float angle = axis.magnitude * rotation_speed * Time.deltaTime;

        if (angle > 0f)
        {
            // Create and apply rotation quaternion
            Quaternion rotation = Quaternion.AngleAxis(angle, axis.normalized);
            // Apply rotation to the target
            targetRotation = rotation * targetRotation;
        }
    }
    /// <summary>
    /// Checks to see if given position is within hitbox
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    private bool IsTouchWithinBounds(Vector2 screenPosition)
    {
        // If hitbox isnt used, return true always
        if (!useHitbox) return true;

        // Determine where center of box is
        // If custom center is used, set center to that
        // Otherwise, set center to calculated screen position of object 
        /*Vector2 center = useCustomCenter
            ? customHitboxCenter
            : (Vector2)Camera.main.WorldToScreenPoint(transform.position);

        // Build interaction rectangle based on given height and width
        Rect interactionRect = new Rect(
            center.x - hitboxWidth / 2,
            center.y - hitboxHeight / 2,
            hitboxWidth,
            hitboxHeight
        );*/
        // Get all 8 corners of the bounds in world space
        Bounds bounds = OptionalHitbox.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        // Project bounds min/max to screen space
        Vector3 screenMin = Camera.main.WorldToScreenPoint(min);
        Vector3 screenMax = Camera.main.WorldToScreenPoint(max);

        // Build screen-space rect (note: y may be inverted)
        Rect screenRect = Rect.MinMaxRect(
            Mathf.Min(screenMin.x, screenMax.x),
            Mathf.Min(screenMin.y, screenMax.y),
            Mathf.Max(screenMin.x, screenMax.x),
            Mathf.Max(screenMin.y, screenMax.y)
        );

        return screenRect.Contains(screenPosition);
    }

    // Called every frame to draw interface elements
    // Draw debug box over hitbox, if enabled
    /*void OnGUI()
    {
        // Only draw debug box if interaction box and debug box are enabled
        if (!useHitbox || !showHitbox) return;

        // Determine center (either custom or calculated object center)
        Vector2 center = useCustomCenter
            ? customHitboxCenter
            : (Vector2)Camera.main.WorldToScreenPoint(transform.position);


        // Find top-left of rectangle 
        float left = center.x - hitboxWidth / 2f;
        float top = Screen.height - center.y - hitboxHeight / 2f; // Flip Y for GUI

        Rect hitboxRect = new Rect(left, top, hitboxWidth, hitboxHeight);

        // Draw semi-transparent grey rectangle to visualize hitbox
        Color originalColor = GUI.color;
        GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.25f); // Semi-transparent grey
        GUI.Box(hitboxRect, GUIContent.none);
        GUI.color = originalColor;
    }*/
}
