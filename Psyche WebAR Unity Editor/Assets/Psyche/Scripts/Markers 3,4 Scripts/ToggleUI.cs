using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the visibility and appearance of UI components with optional fade-in animation
/// </summary>
public class ToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject UIComponent;  // Reference to the UI element to toggle
    [SerializeField] private Vector3 popupScale = new Vector3(1f, 1f, 1f);  // Target scale for UI component
    //[SerializeField] private bool WillFadeIn = true;

    /// <summary>
    /// Toggles UI component visibility and applies fade-in animation if enabled
    /// </summary>
    public void ToggleUIComponent()
    {
        if (UIComponent.activeSelf)
        {
            StartCoroutine(FadeOutPopup());
        }
        else
        {
            UIComponent.SetActive(true);
            StartCoroutine(FadeInPopup());
        }
    }

    /// <summary>
    /// Toggle model visual.
    /// </summary>
    /// <param name="collider"></param>
    public void ToggleModelComponent(Collider collider)
    {
        if (UIComponent.activeSelf)
        {
            StartCoroutine(FadeOutPopup());
        }
        else
        {
            UIComponent.SetActive(true);
            StartCoroutine(FadeInPopup());
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

    /// <summary>
    /// Lerps fade in.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInPopup()
    {
        UIComponent.transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            UIComponent.transform.localScale = Vector3.Lerp(Vector3.zero, popupScale, timer / 0.3f);
            yield return null;
        }
        UIComponent.transform.localScale = popupScale;
    }
    /// <summary>
    /// Lerps fade out.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutPopup()
    {
        float timer = 0.3f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            UIComponent.transform.localScale = Vector3.Lerp(Vector3.zero, popupScale, timer / 0.3f);
            yield return null;
        }
        UIComponent.SetActive(false);
        UIComponent.transform.localScale = popupScale;
    }
}