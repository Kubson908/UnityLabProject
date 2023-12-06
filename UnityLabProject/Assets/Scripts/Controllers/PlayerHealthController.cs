using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public bool dead = false;

    public void UpdateHealthBar(int currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
    }

    public void Die()
    {
        dead = true;
    }
}
