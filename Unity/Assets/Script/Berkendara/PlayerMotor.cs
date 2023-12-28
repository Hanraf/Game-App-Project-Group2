using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMotor : MonoBehaviour
{
    [Header("Player Settings")]
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 5f;
    public float minYPosition = 0.0f;
    public float maxYPosition = 5.0f;
    public float minXPosition = -5.0f;
    public float maxXPosition = 5.0f;
    private RaceManager raceManager;

    [Header("Level Settings")]
    public float levelDistance = 1000f;
    public string levelCompleteSceneName = "LevelCompleteScene";

    [Header("Timer Settings")]
    public float gameTime = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI distanceText;

    [Header("Game Over Settings")]
    public string gameOverSceneName = "GameOverScene";

    [Header("Road Settings")]
    public float scrollSpeed = 5f;
    private float lastFrameTime = 0f;
    public Renderer roadRenderer;

    private float currentDistance = 0f;
    private float currentTime = 0f;

    private bool isPowerupActive = false;
    private bool isObstacleActive = false;

    private void Start()
    {
        UpdateDistanceText();
        currentTime = gameTime;

        if (roadRenderer == null)
        {
            Debug.LogError("Renderer component not found on the road object.");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        MovePlayer();
        UpdateTimer();
        UpdateDistanceText();
        ScrollRoad();

        if (currentDistance >= levelDistance)
        {
            LevelComplete();
        }

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        float clampedYPosition = Mathf.Clamp(transform.position.y + verticalInput * verticalSpeed * Time.deltaTime, minYPosition, maxYPosition);
        float clampedXPosition = Mathf.Clamp(transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime, minXPosition, maxXPosition);

        if (clampedYPosition != transform.position.y || clampedXPosition != transform.position.x)
        {
            transform.position = new Vector3(clampedXPosition, clampedYPosition, transform.position.z);
        }
        // Disable rotation
        // Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void LevelComplete()
    {
        SceneManager.LoadScene(levelCompleteSceneName);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.RoundToInt(currentTime);
        }
    }

    private void UpdateDistanceText()
    {
        distanceText.text = "Distance: " + Mathf.RoundToInt(currentDistance) + " / " + levelDistance;
    }

    private void ScrollRoad()
    {
        float currentTime = Time.time;
        float deltaTime = currentTime - lastFrameTime;
        lastFrameTime = currentTime;

        float offset = currentTime * scrollSpeed;
        float deltaOffset = deltaTime * scrollSpeed;

        currentDistance += deltaOffset;

        roadRenderer.material.mainTextureOffset = new Vector2(0, offset);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision detected with: " + col.gameObject.tag);
        Debug.Log("Collision detected!");

        if (col.gameObject.CompareTag("Car"))
        {
            Debug.Log("Hit by Car!");
            GameOver();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Powerup"))
        {
            Debug.Log("Hit by Powerup!");
            Destroy(col.gameObject);

            // Reset durasi obstacle jika sedang aktif
            if (isObstacleActive)
            {
                StopCoroutine(DecelerateDistance());
                isObstacleActive = false;
            }

            if (!isPowerupActive)
            {
                isPowerupActive = true;
                StartCoroutine(AccelerateDistance());
            }
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit by Obstacle!");
            Destroy(col.gameObject);

            // Reset durasi powerup jika sedang aktif
            if (isPowerupActive)
            {
                StopCoroutine(AccelerateDistance());
                isPowerupActive = false;
            }

            if (!isObstacleActive)
            {
                isObstacleActive = true;
                StartCoroutine(DecelerateDistance());
            }
        }
    }

    private IEnumerator AccelerateDistance()
    {
        float accelerationFactor = 7f;
        float accelerationDuration = 3f;

        float originalScrollSpeed = scrollSpeed;

        // Reset durasi obstacle jika sedang aktif
        if (isObstacleActive)
        {
            StopCoroutine(DecelerateDistance());
            isObstacleActive = false;
        }

        // Temporarily increase the scroll speed
        scrollSpeed *= accelerationFactor;

        // Wait for the specified duration
        yield return new WaitForSeconds(accelerationDuration);

        // Restore the original scroll speed
        scrollSpeed = originalScrollSpeed;
        isPowerupActive = false;  // Reset status powerup
    }

    private IEnumerator DecelerateDistance()
    {
        float decelerationFactor = 5f;
        float decelerationDuration = 3f;

        float originalScrollSpeed = scrollSpeed;

        // Reset durasi powerup jika sedang aktif
        if (isPowerupActive)
        {
            StopCoroutine(AccelerateDistance());
            isPowerupActive = false;
        }

        // Temporarily decrease the scroll speed
        scrollSpeed /= decelerationFactor;

        // Wait for the specified duration
        yield return new WaitForSeconds(decelerationDuration);

        // Restore the original scroll speed
        scrollSpeed = originalScrollSpeed;
        isObstacleActive = false;  // Reset status obstacle
    }
}
