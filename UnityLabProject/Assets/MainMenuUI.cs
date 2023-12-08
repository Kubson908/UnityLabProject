using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnMouseUp()
    {
        Clicked();
    }

    public void Clicked()
    {
        SceneManager.LoadScene(1);
    }
}
