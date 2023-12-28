using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerMotor playerMotor;  // Rujukan ke PlayerMotor

    [Header("Level Settings")]
    public float levelDistance = 1000f;
    public string levelCompleteSceneName = "LevelCompleteScene";

    [Header("Spawn Settings")]
    public GameObject carPrefab;
    public GameObject powerupPrefab;
    public GameObject obstaclePrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public float yourInitialSpeed = 5f;
    public float spawnRangeXMin = -5f;
    public float spawnRangeXMax = 5f;
    public float spawnRangeY = 6f;

    [Header("Timer Settings")]
    public float gameTime = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI distanceText;

    [Header("Game Over Settings")]
    public string gameOverSceneName = "GameOverScene";

    [Header("Road Settings")]
    public float scrollSpeed = 5f;
    private float lastFrameOffset = 0f;
    public Renderer roadRenderer;

    private float currentDistance = 0f;
    private float currentTime = 0f;

    private void Start()
    {
        StartCoroutine(SpawnCars());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnObstacles());
        UpdateDistanceText();
        currentTime = gameTime;

        if (roadRenderer == null)
        {
            Debug.LogError("Renderer component not found on the road object.");
        }
    }

    private void Update()
    {
        playerMotor.Update();  // Panggil Update dari PlayerMotor untuk menggerakkan pemain
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
        float offset = Time.time * scrollSpeed;
        float deltaOffset = offset - lastFrameOffset;

        currentDistance += deltaOffset;

        roadRenderer.material.mainTextureOffset = new Vector2(0, offset);

        lastFrameOffset = offset;
    }

    private void LevelComplete()
    {
        SceneManager.LoadScene(levelCompleteSceneName);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            float randomX = Random.Range(spawnRangeXMin, spawnRangeXMax);
            GameObject newCar = Instantiate(carPrefab, new Vector3(randomX, spawnRangeY, 0f), Quaternion.identity);

            Rigidbody2D carRigidbody2D = newCar.GetComponent<Rigidbody2D>();
            if (carRigidbody2D == null)
            {
                carRigidbody2D = newCar.AddComponent<Rigidbody2D>();
                carRigidbody2D.gravityScale = 0;
            }
            else
            {
                // Debug.LogWarning("Car already has a Rigidbody2D component.");
            }

            carRigidbody2D.velocity = Vector2.down * yourInitialSpeed;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            float randomX = Random.Range(spawnRangeXMin, spawnRangeXMax);
            GameObject newPowerup = Instantiate(powerupPrefab, new Vector3(randomX, spawnRangeY, 0f), Quaternion.identity);

            Rigidbody2D powerupRigidbody2D = newPowerup.GetComponent<Rigidbody2D>();
            if (powerupRigidbody2D == null)
            {
                powerupRigidbody2D = newPowerup.AddComponent<Rigidbody2D>();
                powerupRigidbody2D.gravityScale = 0;
            }
            else
            {
                // Debug.LogWarning("Powerup already has a Rigidbody2D component.");
            }

            powerupRigidbody2D.velocity = Vector2.down * yourInitialSpeed;

            yield return new WaitForSeconds(spawnInterval * 2);
        }
    }

    public IEnumerator SpawnObstacles()
    {
        while (true)
        {
            float randomX = Random.Range(spawnRangeXMin, spawnRangeXMax);
            GameObject newObstacle = Instantiate(obstaclePrefab, new Vector3(randomX, spawnRangeY, 0f), Quaternion.identity);

            Rigidbody2D obstacleRigidbody2D = newObstacle.GetComponent<Rigidbody2D>();
            if (obstacleRigidbody2D == null)
            {
                obstacleRigidbody2D = newObstacle.AddComponent<Rigidbody2D>();
                obstacleRigidbody2D.gravityScale = 0;
            }
            else
            {
                // Debug.LogWarning("Obstacle already has a Rigidbody2D component.");
            }

            obstacleRigidbody2D.velocity = Vector2.down * yourInitialSpeed;

            yield return new WaitForSeconds(spawnInterval * 1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision detected with: " + col.gameObject.tag);
        Debug.Log("Collision detected!");

        if (col.gameObject.CompareTag("Car"))
        {
            Debug.Log("Hit by Car!");
            GameOver();
        }
        else if (col.gameObject.CompareTag("Powerup"))
        {
            Debug.Log("Hit by Powerup!");
            Destroy(col.gameObject);
            playerMotor.horizontalSpeed *= 1.5f;  // Increase horizontal speed
            playerMotor.verticalSpeed *= 1.5f;    // Increase vertical speed

            // Accelerate the increase of currentDistance
            StartCoroutine(AccelerateDistance());
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit by Obstacle!");
            Destroy(col.gameObject);
            playerMotor.horizontalSpeed *= 0.5f;  // Decrease horizontal speed
            playerMotor.verticalSpeed *= 0.5f;    // Decrease vertical speed

            // Decelerate the increase of currentDistance
            StartCoroutine(DecelerateDistance());
            StartCoroutine(SpawnObstacles());
        }
    }

    public IEnumerator AccelerateDistance()
    {
        // Increase the speed of distance increase for a short duration
        float accelerationFactor = 5f;
        float accelerationDuration = 5f;

        float originalScrollSpeed = scrollSpeed;

        // Temporarily increase the scroll speed
        scrollSpeed *= accelerationFactor;

        // Wait for the specified duration
        yield return new WaitForSeconds(accelerationDuration);

        // Restore the original scroll speed
        scrollSpeed = originalScrollSpeed;
    }

    public IEnumerator DecelerateDistance()
    {
        // Decrease the speed of distance increase for a short duration
        float decelerationFactor = 100f;
        float decelerationDuration = 5f;

        float originalScrollSpeed = scrollSpeed;

        // Temporarily decrease the scroll speed
        scrollSpeed /= decelerationFactor;

        // Wait for the specified duration
        yield return new WaitForSeconds(decelerationDuration);

        // Restore the original scroll speed
        scrollSpeed = originalScrollSpeed;
    }
}
