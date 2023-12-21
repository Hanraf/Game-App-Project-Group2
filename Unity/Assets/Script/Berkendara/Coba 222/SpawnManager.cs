using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject obstaclePrefab;
    public GameObject powerUpPrefab;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (carPrefab != null && obstaclePrefab != null && powerUpPrefab != null)
            {
                float randomX = Random.Range(-5, 4);
                float spawnY = 6f;

                Instantiate(carPrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                Instantiate(obstaclePrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                GameObject powerUp = Instantiate(powerUpPrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                Destroy(powerUp, 5f);  // Adjust the time to destroy the power-up as needed
            }

            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }
}
