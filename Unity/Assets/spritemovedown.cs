using System.Collections;
using UnityEngine;

public class spritemovedown : MonoBehaviour
{
    public float speed = 10; // Kecepatan pergerakan ke bawah
    public float resetYPosition = -10f; // Posisi Y untuk ulang gambar
    public float animationDuration = 5; // Durasi animasi dalam detik
    private float elapsedTime = 0.0f; // Waktu yang telah berlalu
    private bool isAnimating = true; // Status animasi
    private Vector3 StartPosition;

    // Misalnya, pada saat awal permainan
    void Start()
    {
        StartPosition = transform.position;
    }

    private void Update()
    {
        if (isAnimating)
        {
            // Menggerakkan GameObject ke bawah
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            // Menambahkan waktu yang telah berlalu
            elapsedTime += Time.deltaTime;

            // Jika waktu yang telah berlalu mencapai batas waktu, hentikan animasi
            if (elapsedTime >= animationDuration)
            {
                isAnimating = false;
            }

            // Jika posisi Y GameObject lebih kecil dari resetYPosition, atur ulang posisi ke atas
            if (transform.position.y < resetYPosition)
            {
                transform.position = StartPosition;
            }
        }
    }

}
