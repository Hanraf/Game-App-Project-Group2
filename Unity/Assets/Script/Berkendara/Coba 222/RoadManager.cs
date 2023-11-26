using UnityEngine;
using TMPro;

public class RoadManager : MonoBehaviour
{
    public float scrollSpeed = 5f;
    private float distanceTraveled = 0f;
    private float lastFrameOffset = 0f;

    private Renderer roadRenderer;
    public motorController playerMotor;
    public TextMeshProUGUI distanceText;

    void Start()
    {
        roadRenderer = GetComponent<Renderer>();
        if (roadRenderer == null)
        {
            Debug.LogError("Renderer component not found on the road object.");
        }
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        float deltaOffset = offset - lastFrameOffset;

        // Update the road distance based on deltaOffset
        distanceTraveled += deltaOffset;

        roadRenderer.material.mainTextureOffset = new Vector2(0, offset);

        // Update distanceText based on the player's distance traveled
        if (playerMotor != null)
        {
            UpdateDistanceText();
        }

        lastFrameOffset = offset;
    }

    void UpdateDistanceText()
    {
        float distance = distanceTraveled;
        distanceText.text = "Distance: " + distance.ToString("F0") + "m";
    }

    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }

    public void ApplySpeedBoost(float boostAmount)
    {
        scrollSpeed += boostAmount;
    }
}
