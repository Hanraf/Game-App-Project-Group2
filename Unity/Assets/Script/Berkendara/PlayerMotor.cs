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
    public Renderer roadRenderer;

    private float currentDistance = 0f;
    private float currentTime = 0f;

    private bool isPowerupActive = false;
    private bool isObstacleActive = false;

    private float lastFrameTime = 0f;  // Menambahkan deklarasi lastFrameTime

    private Coroutine powerupCoroutine;
    private Coroutine obstacleCoroutine;

    private void Start()
    {
        currentDistance = 0f;
        ResetPlayerState();
        UpdateDistanceText();
        currentTime = gameTime;
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
    }

    private void LevelComplete()
    {
        ResetPlayerState();
        SceneManager.LoadScene(levelCompleteSceneName);
    }

    private void ResetPlayerState()
    {
        currentDistance = 0f;
        scrollSpeed = 4f;
        lastFrameTime = 0f;
        isPowerupActive = false;
        isObstacleActive = false;
    }

    public void GameOver()
    {
        ResetPlayerState();
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
        float deltaOffset = Time.deltaTime * scrollSpeed;
        currentDistance += deltaOffset;

        float offset = currentDistance;
        roadRenderer.material.mainTextureOffset = new Vector2(0, offset);

        if (currentDistance >= levelDistance)
        {
            LevelComplete();
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
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

            StopObstacleCoroutine();

            if (!isPowerupActive)
            {
                isPowerupActive = true;
                powerupCoroutine = StartCoroutine(AccelerateDistance());
            }
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit by Obstacle!");
            Destroy(col.gameObject);

            StopPowerupCoroutine();

            if (!isObstacleActive)
            {
                isObstacleActive = true;
                obstacleCoroutine = StartCoroutine(DecelerateDistance());
            }
        }
    }

    private void StopPowerupCoroutine()
    {
        if (isPowerupActive)
        {
            StopCoroutine(powerupCoroutine);
            isPowerupActive = false;
        }
    }

    private void StopObstacleCoroutine()
    {
        if (isObstacleActive)
        {
            StopCoroutine(obstacleCoroutine);
            isObstacleActive = false;
        }
    }

    private IEnumerator AccelerateDistance()
    {
        float accelerationFactor = 7f;
        float accelerationDuration = 3f;

        float originalScrollSpeed = scrollSpeed;

        StopObstacleCoroutine();

        scrollSpeed *= accelerationFactor;

        yield return new WaitForSeconds(accelerationDuration);

        scrollSpeed = originalScrollSpeed;
        isPowerupActive = false;
    }

    private IEnumerator DecelerateDistance()
    {
        float decelerationFactor = 5f;
        float decelerationDuration = 3f;

        float originalScrollSpeed = scrollSpeed;

        StopPowerupCoroutine();

        scrollSpeed /= decelerationFactor;

        yield return new WaitForSeconds(decelerationDuration);

        scrollSpeed = originalScrollSpeed;
        isObstacleActive = false;
    }
}
