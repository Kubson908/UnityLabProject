
using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ChangeWeapon(true);
            Destroy(gameObject);
        }
    }
}
