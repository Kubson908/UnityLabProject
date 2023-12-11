using UnityEngine;

// skrypt pocisku
public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpact;
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
        rb.velocity = transform.up * speed; // nadajemy pociskowi pr�dko�� od razu po inicjalizacji obiektu
        startPos = transform.position;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerCollider);
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, startPos) > 20f) Destroy(gameObject); // niszczymy pocisk po przebyciu okre�lonego dystansu
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "HeadSprite") // sprawdzamy czy pocisk uderzy� w g�ow� przeciwnika
        {
            Debug.Log("Headshot");
            HealthController controller = collision.collider.gameObject.transform.root.GetComponent<HealthController>();
            try 
            {
                controller.DealDamage(headDamage); // zadajemy przeciwnikowi obra�enia od trafienia w g�ow�
            } catch { }
        }
        else if (collision.collider.GetType() == typeof(PolygonCollider2D)) // sprawdzamy czy pocisk uderzy� w cia�o przeciwnika
        {
            HealthController controller = collision.collider.gameObject.transform.root.GetComponent<HealthController>();
            try
            {
                controller.DealDamage(damage);  // zadajemy zwyk�e obra�enia
            }
            catch { }
            
        }
        // w miejscu uderzenia na kr�tk� chwil� pojawia si� ma�y animowany ob�ok dymu (o ile trafi� w co� innego ni� przeciwnik)
        if (collision.gameObject.layer != 7 && collision.gameObject.layer != 11) Destroy(Instantiate(bulletImpact, transform.position, Quaternion.identity), 0.4f);
        Destroy(gameObject);    // niszczymy pocisk przy uderzeniu
    }
}
