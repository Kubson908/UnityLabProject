using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [SerializeField] private PlayerHealthController healthController;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    private Animator animator;
    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    private LookAtCursor look;

    private bool dead = false;


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
        look = GetComponent<LookAtCursor>();
    }

    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        animator.SetFloat("WalkSpeed", Mathf.Abs(direction.x));
        if (look.facingRight) animator.SetInteger("Direction", (int)direction.x);
        else animator.SetInteger("Direction", -(int)direction.x);
        desiredVelocity = new Vector2(direction.x * Mathf.Max(maxSpeed - ground.GetFriction()), 0f);
        if (healthController.dead && !dead)
        {
            body.velocity = Vector2.zero;
            animator.SetTrigger("Death");
            dead = true;
        }
    }

    private void FixedUpdate()
    {
        if (!healthController.dead)
        {   
            onGround = ground.GetOnGround();
            animator.SetBool("IsGrounded", onGround);
            velocity = body.velocity;

            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        
            body.velocity = velocity;
        }
    }
}
