using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles touch/drag gestures for page swiping functionality in UI
/// </summary>
public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // Configuration parameters
    [SerializeField] private float percentThreshold = 0.2f;        // Minimum drag percentage required to trigger page change
    [SerializeField] private float easing = 0.5f;                  // Time in seconds for page transition animation
    [SerializeField] private float slidingSpeedMultiplier;         // Multiplier for drag responsiveness
    [SerializeField] private int startingSlide;                    // Initial page to show (1-based index)
    [SerializeField] private RectTransform rectContainer;          // Parent container that defines page width
    [SerializeField] private PageTrackerManager pageTrackerManager; // Reference to track current page in UI

    // State tracking
    private Vector3 startPanelLocation;
    private Vector3 panelLocation;       // Current anchor position
    private int currentChild;            // Index of currently visible page
    private bool inMotion;               // Flag to prevent input during transitions
    private float containerWidth;        // Width of a single page
    private RectTransform m_rectTransform; // This component's RectTransform

    void Start()
    {
        // Initialize component references and state
        m_rectTransform = GetComponent<RectTransform>();
        panelLocation = m_rectTransform.anchoredPosition;
        currentChild = startingSlide - 1;  // Convert 1-based index to 0-based
        containerWidth = rectContainer.sizeDelta.x;
        startPanelLocation = panelLocation;
    }

    /// <summary>
    /// Handles continuous dragging of pages
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // Allow dragging only when not in transition
        if (!inMotion)
        {
            // Calculate drag distance and update position accordingly
            float difference = eventData.pressPosition.x - eventData.position.x;
            m_rectTransform.anchoredPosition = panelLocation - new Vector3(difference * slidingSpeedMultiplier, 0, 0);
        }
    }

    /// <summary>
    /// Handles end of drag gesture to determine page transition
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Calculate what percentage of page width was dragged
        float percentage = (eventData.pressPosition.x - eventData.position.x) / containerWidth;

        // If dragged past threshold, change page
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;

            // Dragged right-to-left (next page)
            if (percentage > 0 && currentChild < transform.childCount - 1)
            {
                newLocation += new Vector3(-containerWidth, 0, 0);
                currentChild++;
                pageTrackerManager.UpdateUI(currentChild);
            }
            // Dragged left-to-right (previous page)
            else if (percentage < 0 && currentChild > 0)
            {
                newLocation += new Vector3(containerWidth, 0, 0);
                currentChild--;
                pageTrackerManager.UpdateUI(currentChild);
            }

            // Animate to new page position
            StartCoroutine(SmoothMove(m_rectTransform.anchoredPosition, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            // If below threshold, snap back to current page
            StartCoroutine(SmoothMove(m_rectTransform.anchoredPosition, panelLocation, easing));
        }
    }

    /// <summary>
    /// Smoothly animates between two positions using SmoothStep easing
    /// </summary>
    /// <param name="startpos">Starting position</param>
    /// <param name="endpos">Target position</param>
    /// <param name="seconds">Duration of animation</param>
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        inMotion = true;  // Prevent input during animation
        float t = 0f;

        // Gradually interpolate position over time
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            m_rectTransform.anchoredPosition = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;  // Wait for next frame
        }

        inMotion = false;  // Re-enable input after animation completes
    }

    public void GoToPage(int pageIndex)
    {
        // Do nothing if already at the desired page or animation in progress
        if (inMotion || pageIndex == currentChild)
            return;

        // Calculate the new target position
        Vector2 newLocation = new Vector2(startPanelLocation.x + -containerWidth * pageIndex, m_rectTransform.anchoredPosition.y);

        // Start animation
        StartCoroutine(SmoothMove(m_rectTransform.anchoredPosition, newLocation, easing));

        // Update current state
        panelLocation = newLocation;
        currentChild = pageIndex;
        pageTrackerManager.UpdateUI(currentChild);
    }
}