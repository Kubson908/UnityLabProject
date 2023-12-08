using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] DetachWeapon weapon = null;

    public bool dead = false;
    
    private FloatingHealthBar healthBar;
    private int health;
    private Rigidbody2D rb;
    private CapsuleCollider2D attackTrigger;

    

    private void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        rb = GetComponent<Rigidbody2D>();
        attackTrigger = GetComponent<CapsuleCollider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void DealDamage(int damage)
    {
        if (!dead)
        {
            health -= health > damage ? damage : health;
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (weapon != null) weapon.Detach();
        attackTrigger.enabled = false;
        dead = true;
        Destroy(healthBar.gameObject, 1);
        Destroy(rb.gameObject, 5);
    }
}
