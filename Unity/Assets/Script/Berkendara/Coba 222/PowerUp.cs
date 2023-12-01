using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedBoost = 3f;
    public float cooldownTime = 1f;

    private bool isPowerUpActive = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPowerUpActive && other.CompareTag("Player"))
        {
            RoadManager roadManager = FindObjectOfType<RoadManager>();

            if (roadManager != null)
            {
                roadManager.ApplySpeedBoost(speedBoost);
                isPowerUpActive = true;

                StartCoroutine(CooldownTimer());
                gameObject.SetActive(false);  // Deactivate the power-up instead of destroying it
            }
        }
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        isPowerUpActive = false;
        gameObject.SetActive(true);  // Reactivate the power-up after cooldown
    }
}
