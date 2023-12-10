using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private DetachWeapon weapon;
    public bool dead = false;

    public void Die()
    {
        dead = true;
        weapon.Detach();
    }
}
