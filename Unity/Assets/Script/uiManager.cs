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
        AudioManager.Instance.PlaySound("open-door");
        SceneManager.LoadScene(sceneName); // Memuat scene dengan nama yang diberikan
    }

    // Metode ini akan dipanggil ketika terjadi tabrakan
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("open-door");
            StartCoroutine(LoadSceneWithDelay(sceneName));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSecondsRealtime(1f); // Delay sejenak sebelum memuat scene baru
        SceneManager.LoadScene(sceneName);
    }

}
