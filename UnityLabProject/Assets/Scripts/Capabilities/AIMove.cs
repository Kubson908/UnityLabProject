using System;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float speed = 2.5f;
    [SerializeField] private float maxDelta = 1f;
    [SerializeField, Range(3f, 20f)] private float detectionDistance = 5f;
    [SerializeField] private float stopDistance = 0.7f;

    public Transform playerAK47;
    public Transform playerPistol;
    public bool attackMode = false;

    private HealthController healthController;
    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    private Animator animator;
    private bool onGround;
    private bool facingRight = true;
    private bool awareOfPlayer = false;
    private bool dead = false;
    private CapsuleCollider2D attackTrigger;

    private float distanceAK47 = 100000f;
    private float distancePistol = 100000f;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        attackTrigger = GetComponent<CapsuleCollider2D>();
       /* Physics2D.IgnoreCollision(GetComponentInChildren<BoxCollider2D>(), playerAK47.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<BoxCollider2D>(), playerPistol.GetComponent<BoxCollider2D>());
        foreach (var collider in GetComponentsInChildren<PolygonCollider2D>())
        {
            Physics2D.IgnoreCollision(collider, playerAK47.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(collider, playerPistol.GetComponent<BoxCollider2D>());
        }*/
            
        if (transform.localScale.x < 0) facingRight = false;
    }

    void Update()
    {

        distanceAK47 = Vector2.Distance(transform.position, playerAK47.position);
        distancePistol = Vector2.Distance(transform.position, playerPistol.position);
        if (!GameManager.Instance.rifle) direction = (playerPistol.transform.position - transform.position).normalized;
        else direction = (playerAK47.transform.position - transform.position).normalized;
        animator.SetFloat("WalkSpeed", Mathf.Abs(body.velocity.x));
        desiredVelocity = new Vector2(direction.x * Mathf.Max(speed/* - ground.GetFriction()*/), 0f);
        if (healthController.dead && !dead)
        {
            attackMode = false;
            body.velocity = Vector2.zero;
            animator.SetBool("Attack", false);
            animator.SetTrigger("Death");
            dead = true;
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        if ((distanceAK47 < detectionDistance || distancePistol < detectionDistance || awareOfPlayer) && !healthController.dead)
        {
            if (direction.x < 0 && facingRight) Flip();
            if (direction.x > 0 && !facingRight) Flip();
            velocity = body.velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, direction.x < 0 ? desiredVelocity.x + stopDistance : desiredVelocity.x - stopDistance, maxDelta);
            body.velocity = velocity;
        }
        if (GameManager.Instance && GameManager.Instance.currentHealth <= 0 && attackMode)
        {
            attackMode = false;
            animator.SetBool("Attack", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !healthController.dead && GameManager.Instance.currentHealth > 0)
        {
            animator.SetBool("Attack", true);
            attackMode = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !attackTrigger.bounds.Intersects(collision.bounds))
        {
            animator.SetBool("Attack", false);
            attackMode = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            awareOfPlayer = true;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }

}
