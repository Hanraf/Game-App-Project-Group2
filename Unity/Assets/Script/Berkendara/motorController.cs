using UnityEngine;
using TMPro;

public class motorController : MonoBehaviour
{
    public float motorSpeed;
    public float verticalSpeed; // Kecepatan vertikal
    public float maxPos = 5.401854f;
    public float minPos = -5.401854f;
    public float maxVerticalPos = 4.17f;
    public float minVerticalPos = -4f;

    public TextMeshProUGUI crashText; // Reference to CrashText

    private Vector3 position;
    private float distanceTraveled;

    [Header("Level Settings")]
    [SerializeField] string levelGameOver = "1-kamar"; // Default folder name

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        // Pergerakan horizontal
        position.x += Input.GetAxis("Horizontal") * motorSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minPos, maxPos);

        // Pergerakan vertikal
        float verticalInput = Input.GetAxis("Vertical");
        position.y += verticalInput * verticalSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, minVerticalPos, maxVerticalPos);

        // Update distanceTraveled based on forward movement only
        if (verticalInput > 0f)
        {
            distanceTraveled += verticalInput * verticalSpeed * Time.deltaTime;
        }

        transform.position = position;
    }

    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }

    public void ApplySpeedBoost(float boostAmount)
    {
        motorSpeed += boostAmount;

        // Batas kecepatan
        motorSpeed = Mathf.Clamp(motorSpeed, 0f, 20f); // Sesuaikan batas kecepatan yang diinginkan
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy Car")
        {
            // Show crashText
            crashText.gameObject.SetActive(true);

            // Implement your game over logic here (optional)
            Debug.Log("Game Over!");
            string gameover = levelGameOver;

            // Load the game over scene (replace "GameOverScene" with your actual scene name)
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameover);
        }
    }
}