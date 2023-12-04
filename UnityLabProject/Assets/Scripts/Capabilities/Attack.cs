using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private WeaponController weapon = null;
    [SerializeField] private float shootDelay = 0.13f;

    public Transform firePoint;
    public GameObject bulletPrefab;

    private AudioSource audioSource;
    private bool isFiring;
    private float timeCounter;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon.RetrieveLeftClick())
        {
            isFiring = true;
            animator.SetBool("IsFiring", true);
        }
        else if (weapon.RetrieveLeftRelease())
        {
            isFiring = false;
            animator.SetBool("IsFiring", false);
        }

        if (isFiring)
        {
            timeCounter -= Time.deltaTime;

            if (timeCounter <= 0)
            {
                timeCounter = shootDelay;
                Shoot();
            }
        }
        else 
            timeCounter -= Time.deltaTime;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
