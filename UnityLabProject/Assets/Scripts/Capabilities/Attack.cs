using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack : MonoBehaviour
{
    [SerializeField] private WeaponController weapon = null;
    [SerializeField] private float shootDelay = 0.13f;
    [SerializeField] private AudioClip noAmmo;
    [SerializeField] private Text ammoDisplay;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject mag;
    [SerializeField] private GameObject magPrefab;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioClip reload;
    [SerializeField] private Transform hand;

    public Transform firePoint;
    public GameObject bulletPrefab;

    private AudioSource audioSource;
    private bool isFiring;
    private float timeCounter;
    private Animator weaponAnimator;
    private GameManager manager;
    private bool isReloading = false;

    private LookAtCursor look;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
        manager = GameManager.Instance;
        look = transform.root.GetComponent<LookAtCursor>();
        ammoDisplay.text = manager.magazine + "/" + manager.totalAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;

        if (weapon.RetrieveReloadClick())
        {
            StartCoroutine(Reload());
            return;
        }

        if (weapon.RetrieveLeftClick() && manager.currentHealth > 0)
        {
            isFiring = true;
            weaponAnimator.SetBool("IsFiring", manager.magazine != 0 ? true : false);
        }
        else if (weapon.RetrieveLeftRelease())
        {
            isFiring = false;
            weaponAnimator.SetBool("IsFiring", false);
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
        if (manager.magazine != 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(audioSource.clip);
            manager.magazine--;
            ammoDisplay.text = manager.magazine + "/" + manager.totalAmmo;
        }
        else
        {
            audioSource.PlayOneShot(noAmmo);
            weaponAnimator.SetBool("IsFiring", false);
        }
        
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        isFiring = false;
        weaponAnimator.SetBool("IsFiring", false);
        if (manager.totalAmmo == 0 || manager.magazine == manager.capacity)
        {
            isReloading = false;
        }
        else
        {
            audioSource.PlayOneShot(reload);
            playerAnimator.SetBool("IsReloading", true);
            mag.SetActive(false);
            GameObject dropMag = Instantiate(magPrefab, mag.transform.position, mag.transform.rotation, null);
            dropMag.transform.localScale *= look.facingRight ? 0.1f : -0.1f;
            if (!look.facingRight)
                dropMag.transform.Rotate(new Vector3(0, 0, -66f));
            Destroy(dropMag, 10);
            yield return new WaitForSeconds(0.5f);
            GameObject newMag = Instantiate(magPrefab, hand.transform.position, hand.rotation, hand);
            newMag.transform.localScale *= 0.5f;
            Destroy(newMag.GetComponent<Rigidbody2D>());
            Destroy(newMag.GetComponent<PolygonCollider2D>());
            yield return new WaitForSeconds(0.9165f);
            Destroy(newMag);
            mag.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5831f);
            int diff = manager.capacity - manager.magazine;
            manager.magazine += diff <= manager.totalAmmo ? diff : manager.totalAmmo; ;
            manager.totalAmmo -= diff <= manager.totalAmmo ? diff : manager.totalAmmo;
            ammoDisplay.text = manager.magazine + "/" + manager.totalAmmo;
            isReloading = false;
            playerAnimator.SetBool("IsReloading", false);
        }
    }
}
