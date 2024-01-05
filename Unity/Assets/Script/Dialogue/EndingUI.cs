using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MonologCondition
{
    public string conditionName;
    public int minTotalPoints;
    public int maxTotalPoints;
    public int minTotalScore;
    public int maxTotalScore;
    public TextAsset monolog;
    public string sceneToLoad; // Add a field for the scene to load
}

public class EndingUI : MonoBehaviour
{
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private TextMeshProUGUI totalPointsText;
    [SerializeField] private TextMeshProUGUI totalScoreQuizText;
    [SerializeField] private ScoreManager quizScoreManager;
    [SerializeField] private Button continueButton; // Reference to the continue button
    [SerializeField] private float buttonActivationDelay = 2f; // Delay in seconds before the button becomes clickable

    [Header("Monologs")]
    [SerializeField] private MonologCondition[] monologConditions;
    private int currentMonologIndex = 0;

    private bool isButtonClickable = false; // Variable to track whether the button is clickable

    private void OnEnable()
    {
        endingPoints.OnTotalPointsChanged += UpdateTotalPoints;
        quizScoreManager.OnTotalScoreChanged += UpdateTotalScoreQuiz;
    }

    private void OnDisable()
    {
        endingPoints.OnTotalPointsChanged -= UpdateTotalPoints;
        quizScoreManager.OnTotalScoreChanged -= UpdateTotalScoreQuiz;
    }

    private void Start()
    {
        DisplayTotalPoints();
        DisplayTotalScoreQuiz();
        DisplayMonologBasedOnConditions();
        HideButton(); // Hide the button initially
    }

    private void UpdateTotalPoints()
    {
        DisplayTotalPoints();
        DisplayMonologBasedOnConditions();
        CheckButtonInteractivity();
    }

    private void UpdateTotalScoreQuiz()
    {
        DisplayTotalScoreQuiz();
        DisplayMonologBasedOnConditions();
        CheckButtonInteractivity();
    }

    private void DisplayTotalPoints()
    {
        if (totalPointsText != null)
        {
            totalPointsText.text = "Total Points: " + endingPoints.TotalPoints;
        }
    }

    private void DisplayTotalScoreQuiz()
    {
        if (totalScoreQuizText != null)
        {
            totalScoreQuizText.text = "Total Score (Quiz): " + quizScoreManager.TotalScore;
        }
    }

    private void DisplayMonologBasedOnConditions()
    {
        // Find the first condition that matches the current total points and total score
        MonologCondition condition = Array.Find(monologConditions, IsConditionMet);

        if (condition != null)
        {
            StartCoroutine(StartMonolog(condition));
        }
    }

    private bool IsConditionMet(MonologCondition condition)
    {
        int totalPoints = endingPoints.TotalPoints;
        int totalScore = quizScoreManager.TotalScore;

        return totalPoints >= condition.minTotalPoints && totalPoints <= condition.maxTotalPoints &&
               totalScore >= condition.minTotalScore && totalScore <= condition.maxTotalScore;
    }

    private IEnumerator StartMonolog(MonologCondition condition)
    {
        yield return new WaitForSeconds(0.1f);

        DialogueManager.GetInstance().EnterDialogueMode(condition.monolog, null);

        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        // Set the button to be clickable after the dialogue finishes
        isButtonClickable = true;

        // Wait for the specified delay before making the button clickable
        yield return new WaitForSeconds(buttonActivationDelay);

        // Show the button only if the scene has not been loaded yet
        if (!string.IsNullOrEmpty(condition.sceneToLoad))
        {
            ShowButton();
        }
    }

    private void CheckButtonInteractivity()
    {
        // Enable the button only if all dialogues have been completed and the button is clickable
        if (continueButton != null)
        {
            continueButton.interactable = !DialogueManager.GetInstance().dialogueIsPlaying && isButtonClickable;
        }
    }

    private void HideButton()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    private void ShowButton()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    public void ContinueButtonClicked()
    {
        // Load the specified scene after finishing the dialogue
        if (isButtonClickable)
        {
            SceneManager.LoadScene(monologConditions[currentMonologIndex].sceneToLoad);
        }
    }
}
