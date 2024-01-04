using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private TextMeshProUGUI totalPointsText;

    private void Start()
    {
        DisplayTotalPoints();
    }

    private void DisplayTotalPoints()
    {
        if (totalPointsText != null)
        {
            totalPointsText.text = "Total Points: " + endingPoints.TotalPoints;
        }
    }
}
