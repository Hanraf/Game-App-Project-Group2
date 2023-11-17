using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Transform target; // Transform pemain yang akan diikuti oleh kamera
    public float smoothSpeed = 0.125f; // Kecepatan pergerakan kamera (semakin kecil nilainya, semakin halus pergerakan kamera)

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z); // Dapatkan posisi yang diinginkan untuk kamera (berdasarkan posisi pemain)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Menggunakan Lerp untuk membuat pergerakan kamera menjadi halus
            transform.position = smoothedPosition; // Atur posisi kamera ke posisi yang dihasilkan
        }
    }
}
