
using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.canChangeWeapon = true; 
            GameManager.Instance.ChangeWeapon(true);
            Destroy(gameObject);
            Destroy(this);
        }
    }
}
