using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens external URLS when called.
/// </summary>
public class OpenLinks : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => { OpenURL("https://ar-602592.gitlab.io/#learnmore"); });
    }

    /// <summary>
    /// Opens external link for provided url argument.
    /// </summary>
    /// <param name="url"></param>
    public void OpenURL(string url) {
        Application.OpenURL(url);
    }
}
