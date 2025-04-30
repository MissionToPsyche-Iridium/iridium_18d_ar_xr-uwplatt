using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure this is included for TextMeshPro
using UnityEngine.SceneManagement;

/// <summary>
/// Manages quiz flow, displaying questions, score tracking, and results.
/// </summary>
public class QuizManager : MonoBehaviour
{
    [SerializeField] private PageTrackerManager _pageTrackerManager; // Reference to page tracker
    [Space]
    [SerializeField] private GameObject introPanel; // Intro panel reference
    [SerializeField] private GameObject[] questionPanels; // Question panels array reference
    [SerializeField] private GameObject endPanel; // End panel reference
    [Space]
    [SerializeField] private TextMeshProUGUI scoreText; // Changed from Text to TextMeshProUGUI

    private int currentQuestion = 0;
    private int score = 0;

    private bool shouldResetScoreOnStart = true;

    /// <summary>
    /// Disable page tracker initially 
    /// </summary>
    private void Start()
    {
        _pageTrackerManager.gameObject.SetActive(false);
    }
    /// <summary>
    /// Called to start quiz.
    /// Resets state, hides intro panel, shows first panel.
    /// </summary>
    public void StartQuiz()
    {
        Debug.Log("StartQuiz called!");
        introPanel.SetActive(false);
        _pageTrackerManager.gameObject.SetActive(true);

        if (shouldResetScoreOnStart)
        {
            score = 0;
            currentQuestion = 0;

            // Enable answer buttons and hide feedback and next buttons
            foreach (var panel in questionPanels)
            {
                foreach (var btn in panel.GetComponentsInChildren<Button>())
                {
                    btn.interactable = true;
                    btn.gameObject.SetActive(true);
                }

                Transform correctText = panel.transform.Find("CorrectText");
                Transform incorrectText = panel.transform.Find("IncorrectText");
                Transform nextButton = panel.transform.Find("NextButton");

                if (correctText) correctText.gameObject.SetActive(false);
                if (incorrectText) incorrectText.gameObject.SetActive(false);
                if (nextButton) nextButton.gameObject.SetActive(false);
            }
        }

        shouldResetScoreOnStart = true;

        ShowQuestion(currentQuestion);
    }

    /// <summary>
    /// Displays argument associated with given index.
    /// </summary>
    /// <param name="index"></param>
    public void ShowQuestion(int index)
    {
        _pageTrackerManager.UpdateUI(currentQuestion);
        for (int i = 0; i < questionPanels.Length; i++)
        {
            questionPanels[i].SetActive(i == index);
        }
    }

    /// <summary>
    /// Called on answer selection, disables other options, updates score and shows applicable feedback.
    /// </summary>
    /// <param name="clickedButton"></param>
    /// <param name="isCorrect"></param>
    public void AnswerSelected(Button clickedButton, bool isCorrect)
    {
        // Disable all answer buttons
        foreach (Button btn in clickedButton.transform.parent.GetComponentsInChildren<Button>())
        {
            btn.interactable = false;

            if (btn != clickedButton)
                btn.gameObject.SetActive(false);
        }

        // Move the selected button and change color
        clickedButton.transform.localPosition = new Vector3(0, 80, 0);
        clickedButton.image.color = isCorrect ? Color.green : Color.red;

        // Show correct or incorrect feedback
        Transform feedback = clickedButton.transform.parent.Find(isCorrect ? "CorrectText" : "IncorrectText");
        if (feedback != null) feedback.gameObject.SetActive(true);

        if (isCorrect) score++;

        // Show next button
        clickedButton.transform.parent.Find("NextButton").gameObject.SetActive(true);
    }

    /// <summary>
    /// Iterates to next question, ends if at final index.
    /// </summary>
    public void NextQuestion()
    {
        currentQuestion++;
        if (currentQuestion >= questionPanels.Length)
        {
            EndQuiz();
        }
        else
        {
            ShowQuestion(currentQuestion);
        }
    }

    /// <summary>
    /// Ends quiz.
    /// Displays the end panel as well as message depending on quiz score.
    /// </summary>
    public void EndQuiz()
    {
        _pageTrackerManager.gameObject.SetActive(false);
        foreach (GameObject panel in questionPanels)
            panel.SetActive(false);

        endPanel.SetActive(true);

        //string resultMessage = $"You completed the Psyche quiz. You got {score} question{(score == 1 ? "" : "s")} right!\n\n";
        string resultMessage = "!";

        switch (score)
        {
            case 5:
                //resultMessage += "Mission control is standing by. You're ready for launch!";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 5 Right", MarkerManager.DataType.AdditionalInfo);
                break;
            case 4:
                //resultMessage += "You're cleared for orbit � just don't forget your space snacks.";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 4 Right", MarkerManager.DataType.AdditionalInfo);
                break;
            case 3:
                //resultMessage += "You're Space Cadet certified. A little more training and you�ll be ready to dock.";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 3 Right", MarkerManager.DataType.AdditionalInfo);
                break;
            case 2:
                //resultMessage += "Grounded for now... but your cosmic curiosity is strong.";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 2 Right", MarkerManager.DataType.AdditionalInfo);
                break;
            case 1:
                //resultMessage += "Hey, even Neil Armstrong had to start somewhere. Study up, recruit!";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 1 Right", MarkerManager.DataType.AdditionalInfo);
                break;
            default:
                //resultMessage += "Hey, even Neil Armstrong had to start somewhere. Study up, recruit!";
                resultMessage = MarkerManager.Instance.GetMarkerInfo("Marker 6 End Panel 0 Right", MarkerManager.DataType.AdditionalInfo);
                break;
        }

        scoreText.text = resultMessage;
    }

    /// <summary>
    /// Restart quiz by resetting values.
    /// </summary>
    public void RestartQuiz()
    {
        // Hide all question panels and the end panel
        foreach (var panel in questionPanels)
        {
            panel.SetActive(false);
        }
        endPanel.SetActive(false);

        // Show the intro panel again
        introPanel.SetActive(true);

        // Don�t reset score or answers � just show intro again
        currentQuestion = 0;

        // Don't reset visuals; they stay as user left them
        shouldResetScoreOnStart = false;
    }
    /// <summary>
    /// Calls scene manager to reload scene.
    /// </summary>
    public void ReloadSceneActivate()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Starts quiz if related button is pressed.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartQuiz();
        }
    }
}