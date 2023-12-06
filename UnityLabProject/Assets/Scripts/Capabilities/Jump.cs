using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;

    private Rigidbody2D body;
    private Animator animator;
    private Ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale;

    private bool desiredJump;
    private bool onGround;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ground = GetComponent<Ground>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        desiredJump |= input.RetrieveJumpInput();
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        if(onGround)
        {
            jumpPhase = 0;
        }

       

        if(body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
            if (animator.GetFloat("yVelocity") < 0) animator.SetFloat("yVelocity", 1);
            if (animator.GetBool("Jump")) animator.SetBool("Jump", false);
        }
        else if (body.velocity.y < 0)
        {
            body.gravityScale = downwardMovementMultiplier;
            if (animator.GetFloat("yVelocity") > 0) animator.SetFloat("yVelocity", -1);
            if (animator.GetBool("Jump")) animator.SetBool("Jump", false);
        }
        else
        {
            body.gravityScale = defaultGravityScale;
            animator.SetFloat("yVelocity", 0);
            animator.SetBool("IsGrounded", true);

        }
        if (desiredJump)
        {
            desiredJump = false;
            JumpAction();
        }

        body.velocity = velocity;
    }

    private void JumpAction()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase++;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
            if (!animator.GetBool("Jump"))
            {
                animator.SetBool("Jump", true);
                animator.SetBool("IsGrounded", false);
            }
        }
    }
}
