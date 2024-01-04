using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private TextMeshProUGUI totalPointsText;

    private void OnEnable()
    {
        // Subscribe to the event when the total points are updated
        endingPoints.OnTotalPointsChanged += UpdateTotalPoints;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        endingPoints.OnTotalPointsChanged -= UpdateTotalPoints;
    }

    private void Start()
    {
        DisplayTotalPoints();
    }

    private void UpdateTotalPoints()
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
