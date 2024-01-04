using UnityEngine;
using TMPro;

public class EndingPoints : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;

    private const string PlayerPrefsKey = "TotalPoints";
    private int totalPoints = 0;

    private void Start()
    {
        LoadTotalPoints();
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        totalPoints += points;
        Debug.Log("Total Points Updated: " + totalPoints); // Add this line
        SaveTotalPoints();
        UpdateUI();
    }


    private void UpdateUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Total Points: " + totalPoints;
        }
    }

    public void SaveTotalPoints()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, totalPoints);
        PlayerPrefs.Save();
    }

    private void LoadTotalPoints()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            totalPoints = PlayerPrefs.GetInt(PlayerPrefsKey);
        }
    }
}
