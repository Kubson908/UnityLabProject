using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject playButtons;
    [SerializeField] private GameObject settingsButtons;
    [SerializeField] private GameObject mainButtons;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeVolume());
    }

    public void Play()
    {
        playButtons.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void Settings()
    {
        settingsButtons.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void Back()
    {
        playButtons.SetActive(false);
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

    public void EasyLevel()
    {
        SceneManager.LoadScene(1);
    }
    
    public void HardLevel()
    {
        SceneManager.LoadScene(2);
    }
}
