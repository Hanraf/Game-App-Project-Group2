using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    public string sceneName;
    // Metode ini akan dipanggil saat tombol ditekan
    public void LoadToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Memuat scene dengan nama yang diberikan
    }

    // Metode ini akan dipanggil ketika terjadi tabrakan
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName); // Memuat scene dengan nama yang diberikan
        }
    }

}
