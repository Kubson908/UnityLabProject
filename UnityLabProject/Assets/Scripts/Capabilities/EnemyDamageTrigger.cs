using UnityEngine;
// skrypt triggera zadaj¹cego obra¿enia graczowi
public class EnemyDamageTrigger : MonoBehaviour
{
    public Transform root;
    [SerializeField] private int damagePoints = 1;
    [SerializeField] private float minDistance = 1.7f;  // odleg³oœæ od pivota dla której wy³¹czamy mo¿liwoœæ zadawania obra¿eñ
    [SerializeField] private float maxDistance = 2.7f;  // i dla której j¹ w³¹czamy

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
        if (collision.gameObject.CompareTag("Player") && parentMove.attackMode && damage)   // sprawdzamy czy trigger uruchomi³ gracz, czy w³¹czony jest tryb ataku i czy mo¿na zadaæ obra¿enia
        {
            Debug.Log("Damage: " + damagePoints);
            try
            {
                GameManager.Instance.HurtPlayer(damagePoints);  // zadajemy obra¿enia graczowi

            }
            catch { }
            // wy³¹czamy mo¿liwoœæ zadania obra¿eñ na wypadek ponownego wejœcia gracza w trigger przed osi¹gniêciem docelowej wysokoœci triggera
            damage = false; 
            audioSource.Play(); // odtwarzamy odg³os ataku
        }
    }

    // Update is called once per frame
    void Update()
    {
        // zarz¹dzanie mo¿liwoœci¹ zadawania obra¿eñ
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
