using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EndingPoints", menuName = "ScriptableObjects/EndingPoints", order = 1)]
public class EndingPoints : ScriptableObject
{
    [SerializeField] private int totalPoints;
    public UnityAction OnTotalPointsChanged;

    public int TotalPoints
    {
        get { return totalPoints; }
        set
        {
            totalPoints = value;
            OnTotalPointsChanged?.Invoke(); // Invoke the event when total points are updated
        }
    }

    // Metode untuk mereset total poin ke 0
    public void ResetTotalPoints()
    {
        TotalPoints = 0;
    }
}
