using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    [SerializeField] PlayerHealthController healthController;

    [Header("Ammo")]
    public int capacity;
    public int magazine;
    public int totalAmmo;

    [Header("Game Status")]
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHP();
    }

    void DisplayHP()
    {
        /*if (currentHealth >= maxHealth) currentHealth = maxHealth;
        else if (currentHealth <= maxHealth) currentHealth = maxHealth;*/

        healthController.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= currentHealth > damage ? damage : currentHealth;
        if (currentHealth <= 0)
            healthController.Die();
    }
}
