using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCar : MonoBehaviour
{
    public float normalSpeed = -1f;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * currentSpeed * Time.deltaTime);
    }

    // Called when the player takes a power-up
    public void ApplySpeedReduction(float reductionAmount)
    {
        currentSpeed -= reductionAmount;
        currentSpeed = Mathf.Max(currentSpeed, 0f);
    }
}
