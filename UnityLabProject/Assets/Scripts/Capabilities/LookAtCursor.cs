using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    public Transform weaponTransform;
    public Transform headTransform;
    public Transform upperSpineTransform;
    public Transform targetTransform;

    private new Camera camera;
    public bool facingRight = true;
    private Quaternion startSpineRotation;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        startSpineRotation = upperSpineTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        FaceMouse();
    }

    private void FaceMouse()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mouseWorldPos, weaponTransform.position) < 0.35f) return;

        Vector2 direction = new(weaponTransform.position.x - mouseWorldPos.x, weaponTransform.position.y - mouseWorldPos.y);

        targetTransform.rotation = headTransform.rotation;
        targetTransform.up = direction;
        float diff = Mathf.Abs(targetTransform.rotation.z) - 0.7112001f;
        if (diff < 0.13f && diff > -0.13f)
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
                upperSpineTransform.rotation = new Quaternion(0, 0, startSpineRotation.z + diff / 2, upperSpineTransform.rotation.w);
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
