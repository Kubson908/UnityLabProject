using Unity.VisualScripting;
using UnityEngine;

public class DetachWeapon : MonoBehaviour
{
    [SerializeField] private Transform weapon;

    private Rigidbody2D rb;

    public void Detach()
    {
        weapon.parent = null;
        rb = weapon.AddComponent<Rigidbody2D>();
        rb.mass = 50;
        weapon.AddComponent<PolygonCollider2D>();
        Destroy(weapon.GetComponent<EnemyDamageTrigger>());
        Debug.Log(weapon);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet" && weapon.parent == null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
        }
    }
}
