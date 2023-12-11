using UnityEngine;
// skrypt triggera zadaj�cego obra�enia graczowi
public class EnemyDamageTrigger : MonoBehaviour
{
    public Transform root;
    [SerializeField] private int damagePoints = 1;
    [SerializeField] private float minDistance = 1.7f;  // odleg�o�� od pivota dla kt�rej wy��czamy mo�liwo�� zadawania obra�e�
    [SerializeField] private float maxDistance = 2.7f;  // i dla kt�rej j� w��czamy

    private AIMove parentMove;
    private bool damage = false;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        root = transform.root.transform;
        parentMove = transform.root.GetComponent<AIMove>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && parentMove.attackMode && damage)   // sprawdzamy czy trigger uruchomi� gracz, czy w��czony jest tryb ataku i czy mo�na zada� obra�enia
        {
            Debug.Log("Damage: " + damagePoints);
            try
            {
                GameManager.Instance.HurtPlayer(damagePoints);  // zadajemy obra�enia graczowi

            }
            catch { }
            // wy��czamy mo�liwo�� zadania obra�e� na wypadek ponownego wej�cia gracza w trigger przed osi�gni�ciem docelowej wysoko�ci triggera
            damage = false; 
            audioSource.Play(); // odtwarzamy odg�os ataku
        }
    }

    // Update is called once per frame
    void Update()
    {
        // zarz�dzanie mo�liwo�ci� zadawania obra�e�
        float distance = Vector2.Distance(root.position, transform.position);
        if (distance < minDistance)
        {
            damage = false;
        }
        else if (distance > maxDistance)
        {
            damage = true;
        }
    }
}
