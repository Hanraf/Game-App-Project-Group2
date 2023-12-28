using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Destroyer : MonoBehaviour
{
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 5f;
    public float minYPosition = 0.0f;
    public float maxYPosition = 5.0f;
    public float minXPosition = -5.0f;
    public float maxXPosition = 5.0f;
    private RaceManager raceManager;


    // Update is called once per frame
    public void Update()
    {
        MovePlayer();
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


    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Car"))
        {
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Powerup"))
        {
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            Destroy(col.gameObject);
        }
    }

}
