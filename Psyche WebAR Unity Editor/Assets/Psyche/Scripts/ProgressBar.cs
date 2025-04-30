using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages progress bar UI element.
/// </summary>
public class ProgressBar : MonoBehaviour
{
    /// <summary>
    /// Changes the width of progress bar.
    /// </summary>
    /// <param name="change"></param>
    [SerializeField] GameObject progressBar;
    public void Change(float change){
        progressBar.transform.localScale=new Vector3(change,1.0f);
    }
    /// <summary>
    /// Ensure consistency between scene loading.
    /// </summary>
    void Awake(){
        DontDestroyOnLoad(progressBar);
    }
    /// <summary>
    /// Start progress bar with minimal value.
    /// </summary>
    void Start(){
        Change(.1f);
    }
}
