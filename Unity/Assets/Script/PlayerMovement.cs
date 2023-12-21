using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Kecepatan pergerakan pemain
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDialogActive = false;

    // Sesuaikan batas-batas map sesuai kebutuhan
    public float mapLeftBoundary = 10.91f;
    public float mapRightBoundary = 32.21f;
    public float mapTopBoundary = 52.2f;
    public float mapBottomBoundary = 47.16f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ambil Collider dari GameObject ini (asumsi Collider adalah BoxCollider2D)
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        // Buat Material Collider baru
        PhysicsMaterial2D colliderMaterial = new PhysicsMaterial2D();
        // Set gesekan pada material Collider
        colliderMaterial.friction = 0.3f; // Sesuaikan nilai gesekan sesuai kebutuhan
        // Assign material ke Collider
        boxCollider.sharedMaterial = colliderMaterial;
        // rb.freezeRotation = true;
    }

    
        private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
    }
    

    void Update()
    {
        if (!isDialogActive || DialogueManager.GetInstance().dialogueIsPlaying)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
    
            // Deteksi tabrakan dengan Collider2D menggunakan Raycast
            RaycastHit2D hit = Physics2D.Raycast(rb.position, moveInput, 1f); // Sesuaikan jarak deteksi sesuai kebutuhan
    
            // Cek apakah ada tabrakan dan objek tersebut memiliki tag "colliderBarang"
            if (hit.collider != null && hit.collider.CompareTag("colliderBarang"))
            {
                // Jika terjadi tabrakan dengan objek yang memiliki tag "colliderBarang", hentikan karakter
                rb.velocity = Vector2.zero;
            }
            else
            {
                // Jika tidak ada tabrakan atau terdapat tabrakan dengan objek yang tidak memiliki tag "colliderBarang", lanjutkan pergerakan
                Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.deltaTime;
                newPosition.x = Mathf.Clamp(newPosition.x, mapLeftBoundary, mapRightBoundary); // Batasan horizontal
                newPosition.y = Mathf.Clamp(newPosition.y, mapBottomBoundary, mapTopBoundary); // Batasan vertikal
                rb.MovePosition(newPosition);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LevelTransitionZone"))
        {
            string sceneName = other.GetComponent<uiManager>().sceneName;
            SceneManager.LoadScene(sceneName); // Pindah ke scene baru
        }
    }

}
