using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeLimit = 300f;
    public float targetDistance = 500f;
    public GameObject playerCar;
    public TextMeshProUGUI timeText;

    [Header("Level Settings")]
    [SerializeField] string levelComplete = "1-Kampus"; // Default folder name
    [SerializeField] string levelGameOver = "1-kamar"; // Default folder name

    private RoadManager roadManager;  // Tambahkan referensi ke RoadManager

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateTimeText();

        // Dapatkan referensi ke RoadManager
        roadManager = FindObjectOfType<RoadManager>();

        if (roadManager == null)
        {
            Debug.LogError("RoadManager not found!");
        }
    }

    void Update()
    {
        timeLimit -= Time.deltaTime;
        UpdateTimeText();

        if (timeLimit <= 0)
        {
            GameOver();
        }

        // Menggunakan GetDistanceTraveled() dari RoadManager
        float distanceTraveled = roadManager != null ? roadManager.GetDistanceTraveled() : 0f;

        if (playerCar != null && distanceTraveled > targetDistance)
        {
            LevelComplete();
        }
    }

    void UpdateTimeText()
    {
        timeText.text = "Time: " + FormatTime(timeLimit);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        string gameover = levelGameOver;
        SceneManager.LoadScene(gameover);
    }

    public void LevelComplete()
    {
        Debug.Log("Level Complete!");
        string complete = levelComplete;
        SceneManager.LoadScene(complete);
    }

    string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60f);
        return string.Format("{0:0}:{1:00}", minutes, remainingSeconds);
    }
}
