using Unity.VisualScripting;
using UnityEngine;

// skrypt od��czaj�cy bro� od postaci wykorzystywany przy jej �mierci
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
        var trigger = weapon.GetComponent<EnemyDamageTrigger>();
        if (trigger != null ) Destroy(trigger);
        Destroy(weapon.gameObject, 5);
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
