using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Objek ini akan bertahan selama game dan hanya boleh ada satu
    private static GameManager instance;

    // Objek ini menyimpan total poin
    [SerializeField] private EndingPoints endingPoints;
    [SerializeField] private ScoreManager scoreManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Inisialisasi total poin jika perlu
        endingPoints.ResetTotalPoints();
        scoreManager.ResetTotalScore();
    }

    // Metode ini dipanggil saat sesi gameplay dimulai
    public void StartNewSession()
    {
        endingPoints.ResetTotalPoints();
        scoreManager.ResetTotalScore();
    }

    // Metode ini untuk mendapatkan total poin
    public int GetTotalPoints()
    {
        return endingPoints.TotalPoints;
        return scoreManager.TotalScore;
    }
}
