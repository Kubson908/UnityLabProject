using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float speed = 2.5f;
    [SerializeField, Range(3f, 20f)] private float detectionDistance = 5f;
    [SerializeField] private HealthController healthController;

    public Transform player;
    public bool attackMode = false;

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
    private CapsuleCollider2D damageCollider;

    private float distance = 100000f;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        damageCollider = GetComponentInChildren<CapsuleCollider2D>();
        Physics2D.IgnoreCollision(GetComponentInChildren<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
        foreach (var collider in GetComponentsInChildren<PolygonCollider2D>())
            Physics2D.IgnoreCollision(collider, player.GetComponent<BoxCollider2D>());
    }

    void Update()
    {

        distance = Vector2.Distance(transform.position, player.position);
        direction = (player.transform.position - transform.position).normalized;
        animator.SetFloat("WalkSpeed", Mathf.Abs(body.velocity.x));
        desiredVelocity = new Vector2(direction.x * Mathf.Max(speed - ground.GetFriction()), 0f);
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
        if ((distance < detectionDistance || awareOfPlayer) && !healthController.dead)
        {
            if (direction.x < 0 && facingRight) Flip();
            if (direction.x > 0 && !facingRight) Flip();
            velocity = body.velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, direction.x < 0 ? desiredVelocity.x + 0.7f : desiredVelocity.x - 0.7f, 1f);
            body.velocity = velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !healthController.dead)
        {
            animator.SetBool("Attack", true);
            attackMode = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !damageCollider.IsTouching(collision))
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
