using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject carPrefab;
    public GameObject obstaclePrefab;
    public GameObject powerUpPrefab;
    public float spawnInterval = 2f;

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

                
                
                GameObject powerUp = Instantiate(powerUpPrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                GameObject newCar = Instantiate(carPrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                GameObject newobstacle = Instantiate(obstaclePrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
                Destroy(powerUp);  
                Destroy(newobstacle);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    
}
