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
    public string sceneToLoad;
    public string displayedText; // Add a field for the displayed text
}

public class EndingUI : MonoBehaviour
{
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private TextMeshProUGUI totalPointsText;
    [SerializeField] private TextMeshProUGUI totalScoreQuizText;
    [SerializeField] private ScoreManager quizScoreManager;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI displayedText;
    [SerializeField] private float buttonActivationDelay = 2f;

    [Header("Monologs")]
    [SerializeField] private MonologCondition[] monologConditions;
    private int currentMonologIndex = 0;

    private bool isButtonClickable = false;

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
        HideButton();
        DisplayMonologBasedOnConditions();
        DisplayTotalPoints();
        DisplayTotalScoreQuiz();
        
        // DisplayInitialText(); // Add this line to display initial text
        
        
    }

    private void UpdateTotalPoints()
    {
        DisplayTotalPoints();
        // DisplayMonologBasedOnConditions();
        // CheckButtonInteractivity();
    }

    private void UpdateTotalScoreQuiz()
    {
        DisplayTotalScoreQuiz();
        // DisplayMonologBasedOnConditions();
        // CheckButtonInteractivity();
    }

    private void DisplayTotalPoints()
    {
        if (totalPointsText != null)
        {
            totalPointsText.text = "Total Point penerapan nilai belanegara: " + endingPoints.TotalPoints;
        }
    }

    private void DisplayTotalScoreQuiz()
    {
        if (totalScoreQuizText != null)
        {
            totalScoreQuizText.text = "Total score ujian: " + quizScoreManager.TotalScore;
        }
    }

    // private void DisplayInitialText()
    // {
    //     // Display the specified text on TextMeshProUGUI at the beginning
    //     if (displayedText != null && monologConditions.Length > 0)
    //     {
    //         displayedText.text = monologConditions[currentMonologIndex].displayedText;
    //     }
    // }

    private void DisplayMonologBasedOnConditions()
    {
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

        isButtonClickable = true;

        yield return new WaitForSeconds(buttonActivationDelay);

        if (!string.IsNullOrEmpty(condition.sceneToLoad))
        {
            ShowButton();
            // Display the specified text on TextMeshProUGUI
            if (displayedText != null)
            {
                displayedText.text = condition.displayedText;
            }
        }
    }

    private void CheckButtonInteractivity()
    {
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
        MonologCondition condition = Array.Find(monologConditions, IsConditionMet);

        if (condition != null)
        {
            if (isButtonClickable)
            {
            SceneManager.LoadScene(condition.sceneToLoad);
            }
        }    
    }
}