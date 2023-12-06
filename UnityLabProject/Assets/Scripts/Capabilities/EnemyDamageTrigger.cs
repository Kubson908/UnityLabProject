using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public Transform root;
    [SerializeField] private int damagePoints = 1;

    private AIMove parentMove;
    private bool damage = false;
    // Start is called before the first frame update
    void Start()
    {
        root = transform.root.transform;
        parentMove = transform.root.GetComponent<AIMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && parentMove.attackMode && damage)
        {
            Debug.Log("Hit");
            GameManager.Instance.HurtPlayer(damagePoints);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(root.position, transform.position);
        if (distance < 1.7f) damage = false;
        else if (distance > 2.7f) damage = true;
    }
}
