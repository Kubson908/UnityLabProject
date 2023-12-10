using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject playButtons;
    [SerializeField] private GameObject mainButtons;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Play()
    {
        playButtons.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void Back()
    {
        playButtons.SetActive(false);
        mainButtons.SetActive(true);
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
