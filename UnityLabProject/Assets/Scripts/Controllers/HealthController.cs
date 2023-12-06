using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    private Rigidbody2D rb;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            int damage = collision.collider.GetComponent<Bullet>().damage;
            DealDamage(damage);
        }
    }

    private void DealDamage(int damage)
    {
        health -= health > damage ? damage : health;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0) Die();
    }

    private void Die()
    {

    }
}
