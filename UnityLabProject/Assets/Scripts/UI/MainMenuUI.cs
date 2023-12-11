using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsButtons;
    [SerializeField] private GameObject mainButtons;

    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        StartCoroutine(FadeVolume());
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settingsButtons.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void Back()
    {
        settingsButtons.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        else audioSource.Play();
    }

    IEnumerator FadeVolume()
    {
        float startVolume = audioSource.volume;
        float targetVolume = 1f;
        float fadeTime = 7f;
        float currentTime = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
