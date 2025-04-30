using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages node interaction behavior including shrinking effects and UI toggles
/// </summary>
public class NodeLogic : MonoBehaviour
{
    //public NodeID nodeID;
    [SerializeField] private float shrinkSize = .8f;  // Size multiplier when node is pressed
    [SerializeField] private ToggleUI Model;          // Reference to model UI component
    [SerializeField] private ToggleUI PopupUIToggle;  // Reference to popup UI component
    [SerializeField] private Material PressedNodeMaterial;  // Material applied when node is pressed
    [SerializeField] private TextMeshProUGUI counterText; // Text for number of nodes tapped

    private Vector3 originalScale;   // Original size of the node
    private bool isSmall = false;    // Tracks if node is currently shrunk
    private float heldDownDuration = 2f;  // Tracks how long node is being pressed
    private bool isTouching = false;  // Tracks if node is being touched

    /// <summary>
    /// Store initial scale when component initializes
    /// </summary>
    void Start()
    {
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Handle input processing each frame
    /// </summary>
    void Update()
    {
        // Process mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    ShrinkNode();
                    isTouching = true;
                    heldDownDuration = 0f;
                }
            }
        }

        if (Input.GetMouseButton(0) && isTouching)
        {
            heldDownDuration += Time.deltaTime;
        }

        // Process touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        ShrinkNode();
                    }
                }
            }
        }

        if (!isSmall)
        {
            OnRelease();
        }
    }

    /// <summary>
    /// Reduces node size when interacted with
    /// </summary>
    void ShrinkNode()
    {
        isSmall = true;
        transform.localScale = originalScale * shrinkSize;
    }

    /// <summary>
    /// Handles node behavior when released
    /// </summary>
    void OnRelease()
    {
        Collider collider = GetComponent<Collider>();
        if (heldDownDuration <= 1f)
        {
            Renderer targetRenderer = gameObject.GetComponent<Renderer>();
            if (!targetRenderer.sharedMaterial.Equals(PressedNodeMaterial))
            {
                CountClick();
            }
            targetRenderer.material = PressedNodeMaterial;
            Model.ToggleUIComponent();
            PopupUIToggle.ToggleUIComponent();
            if (collider != null)
            {
                collider.enabled = false;
            }
            heldDownDuration = 2f;
        }
        transform.localScale = originalScale;
    }

    /// <summary>
    /// Unity event when mouse button is released
    /// </summary>
    void OnMouseUp()
    {
        isSmall = false;
    }

    /// <summary>
    /// Handles node count behavior
    /// </summary>
    void CountClick()
    {
        // Marker 3 Logic
        if (counterText.text.Contains("6"))
        {
            if (counterText.text.Equals("0/6"))
            {
                counterText.text = "1/6";
            }
            else if (counterText.text.Equals("1/6"))
            {
                counterText.text = "2/6";
            }
            else if (counterText.text.Equals("2/6"))
            {
                counterText.text = "3/6";
            }
            else if (counterText.text.Equals("3/6"))
            {
                counterText.text = "4/6";
            }
            else if (counterText.text.Equals("4/6"))
            {
                counterText.text = "5/6";
            }
            else
            {
                counterText.text = "6/6";
            }
        }
        // Marker 4 Logic
        else
        {
            if (counterText.text.Equals("0/5"))
            {
                counterText.text = "1/5";
            }
            else if (counterText.text.Equals("1/5"))
            {
                counterText.text = "2/5";
            }
            else if (counterText.text.Equals("2/5"))
            {
                counterText.text = "3/5";
            }
            else if (counterText.text.Equals("3/5"))
            {
                counterText.text = "4/5";
            }
            else
            {
                counterText.text = "5/5";
            }
        }
    }
}