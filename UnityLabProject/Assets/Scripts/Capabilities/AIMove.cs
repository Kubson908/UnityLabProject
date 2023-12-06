using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float speed = 1.5f;
    [SerializeField, Range(3f, 20f)] private float detectionDistance = 10f;

    public Transform player;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    private Animator animator;
    private bool onGround;
    private bool facingRight = true;
    private bool awareOfPlayer = false;

    private float distance;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponentInChildren<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<EdgeCollider2D>(), player.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), player.GetComponent<BoxCollider2D>());
    }

    void Update()
    {

        distance = Vector2.Distance(transform.position, player.position);
        direction = (player.transform.position - transform.position).normalized;
        desiredVelocity = new Vector2(direction.x * Mathf.Max(speed - ground.GetFriction()), 0f);
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        if (distance < detectionDistance || awareOfPlayer)
        {
            if (direction.x < 0 && facingRight) Flip();
            if (direction.x > 0 && !facingRight) Flip();
            velocity = body.velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, 1f);
            body.velocity = velocity;
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
