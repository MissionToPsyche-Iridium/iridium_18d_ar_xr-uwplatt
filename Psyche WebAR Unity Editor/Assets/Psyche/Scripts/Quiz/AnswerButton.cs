using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles behavior of answer button in the quiz UI.
/// Communicates the user selection to manager.
/// </summary>
public class AnswerButton : MonoBehaviour
{
    public bool isCorrect;
    public QuizManager quizManager;

    private Button button;

    /// <summary>
    /// Initialize start button.
    /// </summary>
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Notifies Manager of selected answer.
    /// </summary>
    void OnClick()
    {
        quizManager.AnswerSelected(button, isCorrect);
    }
}