using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawner : MonoBehaviour
{
    public GameObject cars2;
    public float maxPos = 5.201854f;
    public float delayTimer = 10f;
    float timer;

    void Start()
    {
        timer = delayTimer;
    }

    void Update()
    {
        timer -= Time.deltaTime; // Perbaikan: 'timer.delayTimer' diganti dengan 'Time.deltaTime'
        if (timer <= 0)
        {
            Vector3 carPos = new Vector3(Random.Range(-maxPos, maxPos), transform.position.y, transform.position.z); // Perbaikan: '-5.201854f' diganti dengan '-maxPos'
            Instantiate(cars2, carPos, transform.rotation);
            timer = delayTimer;
        }
    }
}
