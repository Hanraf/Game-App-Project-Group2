using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private TextMeshProUGUI totalPointsText;
    [SerializeField] private TextMeshProUGUI totalScoreQuizText; // New field for displaying total score from quiz
    [SerializeField] private ScoreManager quizScoreManager; // Reference to the ScoreManager for quiz

    [Header("Monologs")]
    [SerializeField] private TextAsset[] monologs;
    private int currentMonologIndex = 0;

    private void OnEnable()
{
    Debug.Log("EndingUI subscribed to OnTotalPointsChanged");
    endingPoints.OnTotalPointsChanged += UpdateTotalPoints;

    Debug.Log("EndingUI subscribed to OnTotalScoreChanged");
    quizScoreManager.OnTotalScoreChanged += UpdateTotalScoreQuiz;
}


    private void OnDisable()
    {
        endingPoints.OnTotalPointsChanged -= UpdateTotalPoints;
        // Unsubscribe from the event when the script is disabled
        quizScoreManager.OnTotalScoreChanged -= UpdateTotalScoreQuiz;
    }

    private void Start()
    {
        DisplayTotalPoints();
        // Display monolog based on total points
        DisplayMonologBasedOnPoints();
        UpdateTotalScoreQuiz();
        
    }

    private void UpdateTotalPoints()
    {
        DisplayTotalPoints();
        // Display monolog based on total points
        DisplayMonologBasedOnPoints();
        UpdateTotalScoreQuiz();
    }

    private void UpdateTotalScoreQuiz()
    {
        // Display the updated total score from the quiz
        if (totalScoreQuizText != null)
        {
            totalScoreQuizText.text = "Total Score (Quiz): " + quizScoreManager.TotalScore;
        }
    }

    private void DisplayTotalPoints()
    {
        if (totalPointsText != null)
        {
            totalPointsText.text = "Total Points: " + endingPoints.TotalPoints;
        }
    }

    private void DisplayMonologBasedOnPoints()
    {
        // Implement your logic for displaying monologs based on total points
        // You can use if statements or switch cases to check the range of total points

        // Example:
        if (endingPoints.TotalPoints >= 0 && endingPoints.TotalPoints <= 10)
        {
            StartCoroutine(StartMonolog(monologs[0]));
        }
        else if (endingPoints.TotalPoints >= 11 && endingPoints.TotalPoints <= 15)
        {
            StartCoroutine(StartMonolog(monologs[1]));
        }
        else if (endingPoints.TotalPoints >= 16 && endingPoints.TotalPoints <= 20)
        {
            StartCoroutine(StartMonolog(monologs[2]));
        }
        // Add more conditions as needed
    }

    private IEnumerator StartMonolog(TextAsset monolog)
    {
        // Delay to give time for other objects in the scene to prepare
        yield return new WaitForSeconds(0.1f);

        // Enter dialogue mode with the specified monolog
        // You may need to replace null with the appropriate animator
        DialogueManager.GetInstance().EnterDialogueMode(monolog, null);

        // Wait until the monolog is finished
        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        // Optional delay between monologs
        yield return new WaitForSeconds(1.0f);
    }
}
