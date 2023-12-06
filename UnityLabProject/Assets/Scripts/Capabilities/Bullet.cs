using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 40;
    public int headDamage = 100;
    private Vector2 startPos;
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        startPos = transform.position;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerCollider);
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, startPos) > 20f) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "HeadSprite")
        {
            Debug.Log("Headshot");
            HealthController controller = collision.collider.gameObject.transform.root.GetComponent<HealthController>();
            controller.DealDamage(headDamage);
        }
        else if (collision.collider.GetType() == typeof(PolygonCollider2D))
        {
            HealthController controller = collision.collider.gameObject.transform.root.GetComponent<HealthController>();
            controller.DealDamage(damage);
        }
        Destroy(gameObject);
    }
}
