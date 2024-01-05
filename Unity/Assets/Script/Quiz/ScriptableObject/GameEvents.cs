using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/New GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI = null;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer = null;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int addScore, int answerScore);
    public DisplayResolutionScreenCallback DisplayResolutionScreen = null;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated = null;

    public delegate void TotalScoreUpdatedCallback();
    public TotalScoreUpdatedCallback TotalScoreUpdated = null;

    [HideInInspector]
    public int CurrentFinalScore = 0;
    [HideInInspector]
    public int StartupHighscore = 0;

    public ScoreManager scoreManager; // Reference to the ScoreManager scriptable object
}
