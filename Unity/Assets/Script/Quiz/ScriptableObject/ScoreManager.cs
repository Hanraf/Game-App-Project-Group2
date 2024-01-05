using UnityEngine;

[CreateAssetMenu(fileName = "ScoreManager", menuName = "ScriptableObjects/ScoreManager", order = 1)]
public class ScoreManager : ScriptableObject
{
    [SerializeField] private int totalScore;
    public int TotalScore => totalScore;

    // Define an event to notify when the total score changes
    public delegate void TotalScoreChangedHandler();
    public event TotalScoreChangedHandler OnTotalScoreChanged;

    public void ResetTotalScore()
    {
        totalScore = 0;
        // Invoke the event when the total score is reset
        OnTotalScoreChanged?.Invoke();
    }

    public void AddToTotalScore(int points)
    {
        totalScore += points;
        // Invoke the event when the total score is updated
        OnTotalScoreChanged?.Invoke();
    }
}
