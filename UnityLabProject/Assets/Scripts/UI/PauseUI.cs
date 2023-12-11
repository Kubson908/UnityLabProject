using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource) StartCoroutine(FadeVolume());
    }

    IEnumerator FadeVolume()
    {
        float startVolume = audioSource.volume;
        float targetVolume = 1f;
        float fadeTime = 5f;
        float currentTime = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        GameManager.Instance.paused = false;
    }
}
