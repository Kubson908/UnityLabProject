using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform target;
    private new Camera camera;

    public void UpdateHealthBar(int currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + new Vector3(0, 2.85f, 0);
    }
}
