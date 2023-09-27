using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Untuk SceneManager

public class StartGame : MonoBehaviour
{
    // Metode ini akan dipanggil saat tombol ditekan
    public void StartLevel1()
    {
        SceneManager.LoadScene("level1"); // Memuat "level1" scene
    }
}
