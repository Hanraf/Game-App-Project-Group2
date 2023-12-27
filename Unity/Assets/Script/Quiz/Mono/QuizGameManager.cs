using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizGameManager : MonoBehaviour
{
    #region Variables

    private Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    [SerializeField] Animator timerAnimtor = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;
    private Color timerDefaultColor = Color.white;

    [Header("Question Settings")]
    [SerializeField] string questionsFolderName = "Questions1"; // Default folder name

    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private int timerStateParaHash = 0;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;

    private bool IsFinished
    {
        get { return (FinishedQuestions.Count < Questions.Length) ? false : true; }
    }

    #endregion

    #region Default Unity methods

    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    void Awake()
    {
        events.CurrentFinalScore = 0;
    }

    void Start()
    {
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        timerDefaultColor = timerText.color;
        LoadQuestions();
        timerStateParaHash = Animator.StringToHash("TimerState");
        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);
        Display();
    }

    #endregion

    void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single ||
            Questions[currentQuestion].GetAnswerType == Question.AnswerType.MultipleTrue)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method.");
        }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        // Menggunakan AnswerScore dari setiap jawaban sebagai tambahan atau pengurangan skor
        int addScore = (isCorrect ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);
        int answerScore = CalculateAnswerScores();

        UpdateScore(addScore + answerScore);

        if (IsFinished)
        {
            SetHighscore();
        }

        var type = (IsFinished)
            ? UIManager.ResolutionScreenType.Finish
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct
            : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen?.Invoke(type, addScore, answerScore);
        }

        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);
                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }
                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }

    IEnumerator StartTimer()
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> correctAnswers = Questions[currentQuestion].GetCorrectAnswers();
            List<int> pickedAnswers = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.MultipleTrue)
            {
                // Check if all correct answers are picked and no additional answers are picked
                var incorrectAnswersPicked = PickedAnswers
                    .Where(picked => !correctAnswers.Contains(picked.AnswerIndex))
                    .ToList();

                return !incorrectAnswersPicked.Any();
            }
            else if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Multi)
            {
                // Check if all correct answers are picked and no incorrect answers are picked
                return correctAnswers.All(answer => pickedAnswers.Contains(answer))
                    && pickedAnswers.All(answer => correctAnswers.Contains(answer));
            }
            else if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
            {
                // Check if only one answer is picked, and it is the correct one
                return PickedAnswers.Count == 1 && correctAnswers.Contains(PickedAnswers[0].AnswerIndex);
            }
        }

        return false;
    }

    void LoadQuestions()
    {
        string folderPath = questionsFolderName;
        Object[] objs = Resources.LoadAll(folderPath, typeof(Question));

        if (objs.Length == 0)
        {
            Debug.LogError($"No questions found in the specified folder: {folderPath}");
            return;
        }

        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    // Menambahkan metode untuk menghitung skor jawaban
    int CalculateAnswerScores()
    {
        int answerScores = 0;
        foreach (var pickedAnswer in PickedAnswers)
        {
            answerScores += Questions[currentQuestion].Answers[pickedAnswer.AnswerIndex].AnswerScore;
        }
        return answerScores;
    }

    #region Getters

    Question GetRandomQuestion()
    {
        if (Questions.Length == 0)
        {
            Debug.LogError("No questions available.");
            return null;
        }

        var randomIndex = GetRandomQuestionIndex();

        if (randomIndex < 0 || randomIndex >= Questions.Length)
        {
            Debug.LogError($"Invalid random index: {randomIndex}. Check the GetRandomQuestionIndex() method.");
            return null;
        }

        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    #endregion
}
