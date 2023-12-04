using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    public LayerMask mouseAimMask;
    public Transform weaponTransform;
    public Transform headTransform;
    public Transform upperSpineTransform;
    public Transform targetTransform;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    private Animator animator;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    private bool facingRight = true;
    private new Camera camera;
    private Quaternion startSpineRotation;


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        startSpineRotation = upperSpineTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        animator.SetFloat("Speed", Mathf.Abs(direction.x));
        if (facingRight) animator.SetInteger("Direction",  (int)direction.x);
        else animator.SetInteger("Direction", -(int)direction.x);
        desiredVelocity = new Vector2(direction.x * Mathf.Max(maxSpeed - ground.GetFriction()), 0f);
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        
        body.velocity = velocity;
        FaceMouse();
    }

    private void FaceMouse()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new(weaponTransform.position.x - mouseWorldPos.x, weaponTransform.position.y - mouseWorldPos.y);

        targetTransform.rotation = headTransform.rotation;
        targetTransform.up = direction;
        float diff = Mathf.Abs(targetTransform.rotation.z) - 0.7112001f;
        if(diff < 0.13f && diff > -0.13f)
        {
            headTransform.up = direction;
            if (facingRight)
                upperSpineTransform.rotation = startSpineRotation;
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z, startSpineRotation.w);
        }
        else
        {
            if (facingRight)
                upperSpineTransform.rotation = new Quaternion(0, 0, startSpineRotation.z + diff/2, upperSpineTransform.rotation.w);
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z - diff / 2, upperSpineTransform.rotation.w);
        }
        weaponTransform.up = direction;
        if (mouseWorldPos.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (mouseWorldPos.x > transform.position.x && !facingRight)
        {
            Flip();
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
