using UnityEngine;

[CreateAssetMenu(fileName = "EndingPoints", menuName = "ScriptableObjects/EndingPoints", order = 1)]
public class EndingPoints : ScriptableObject
{
    [SerializeField] private int totalPoints;

    public int TotalPoints
    {
        get { return totalPoints; }
        set { totalPoints = value; }
    }
}
