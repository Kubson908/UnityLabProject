using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack : MonoBehaviour
{
    [SerializeField] private WeaponController weapon = null;
    [SerializeField] private float shootDelay = 0.13f;
    [SerializeField] private bool fullAuto = true;
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
    public bool isFiring;
    private float timeCounter;
    private Animator weaponAnimator;
    private GameManager manager;
    private bool isReloading = false;

    private LookAtCursor look;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        manager = GameManager.Instance;
        weaponAnimator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
        /*if (manager.riffle)
        {
            manager.rifleCapacity = magCapacity;
            manager.rifleMagazine = magCapacity;
            manager.rifleTotalAmmo = totalAmmo;
            ammoDisplay.text = manager.rifleMagazine + "/" + manager.rifleTotalAmmo;
        }
        else
        {
            manager.pistolCapacity = magCapacity;
            manager.pistolMagazine = magCapacity;
            manager.pistolTotalAmmo = totalAmmo;
        }*/
        look = transform.root.GetComponent<LookAtCursor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            
        }
        if (isReloading) return;

        if (weapon.RetrieveReloadClick())
        {
            StartCoroutine(Reload());
            return;
        }

        if (weapon.RetrieveLeftClick() && manager.currentHealth > 0)
        {
            isFiring = true;
            /*weaponAnimator.SetBool("IsFiring", manager.magazine != 0 ? true : false);*/
        }
        else if (weapon.RetrieveLeftRelease())
        {
            isFiring = false;
            /*weaponAnimator.SetBool("IsFiring", false);*/
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
        {
            timeCounter -= Time.deltaTime;
        }
            

        
    }

    private void Shoot()
    {
        if (!manager.rifle && manager.pistolMagazine != 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(audioSource.clip);
            manager.pistolMagazine--;
            ammoDisplay.text = manager.pistolMagazine + "/" + manager.pistolTotalAmmo;
            weaponAnimator.SetTrigger("ShootPistol");
            if (!fullAuto)
            {
                isFiring = false;
            }
        }
        else if (manager.rifle && manager.rifleMagazine != 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(audioSource.clip);
            manager.rifleMagazine--;
            ammoDisplay.text = manager.rifleMagazine + "/" + manager.rifleTotalAmmo;
            weaponAnimator.SetTrigger("IsFiring");
            if (!fullAuto)
            {
                isFiring = false;
            }
        }
        else
        {
            audioSource.PlayOneShot(noAmmo);
/*            weaponAnimator.SetBool("IsFiring", false);*/
        }
        
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        isFiring = false;
        /*weaponAnimator.SetBool("IsFiring", false);*/
        if (CheckAmmo())
        {
            isReloading = false;
        }
        else
        {
            audioSource.PlayOneShot(reload);
            if (!manager.rifle)
            {
                playerAnimator.SetBool("ReloadingRifle", false);
                playerAnimator.SetBool("ReloadingPistol", true);
            }
            else
            {
                playerAnimator.SetBool("ReloadingPistol", false);
                playerAnimator.SetBool("ReloadingRifle", true);
            }
            mag.SetActive(false);
            GameObject dropMag = Instantiate(magPrefab, mag.transform.position, mag.transform.rotation, null);
            dropMag.transform.localScale *= look.facingRight ? 0.1f : -0.1f;
            if (!look.facingRight && manager.rifle)
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
            ManageReloadAmmo();
            isReloading = false;
            if (!manager.rifle) playerAnimator.SetBool("ReloadingPistol", false);
            else playerAnimator.SetBool("ReloadingRifle", false);
        }
    }

    private bool CheckAmmo()
    {
        if (manager.rifle)
        {
            return manager.rifleTotalAmmo == 0 || manager.rifleMagazine == manager.rifleCapacity;
        }
        else
        {
            return manager.pistolTotalAmmo == 0 || manager.pistolMagazine == manager.pistolCapacity;
        }
    }

    private void ManageReloadAmmo()
    {
        if (manager.rifle)
        {
            int diff = manager.rifleCapacity - manager.rifleMagazine;
            manager.rifleMagazine += diff <= manager.rifleTotalAmmo ? diff : manager.rifleTotalAmmo; ;
            manager.rifleTotalAmmo -= diff <= manager.rifleTotalAmmo ? diff : manager.rifleTotalAmmo;
            ammoDisplay.text = manager.rifleMagazine + "/" + manager.rifleTotalAmmo;
        }
        else
        {
            int diff = manager.pistolCapacity - manager.pistolMagazine;
            manager.pistolMagazine += diff <= manager.pistolTotalAmmo ? diff : manager.pistolTotalAmmo; ;
            manager.pistolTotalAmmo -= diff <= manager.pistolTotalAmmo ? diff : manager.pistolTotalAmmo;
            ammoDisplay.text = manager.pistolMagazine + "/" + manager.pistolTotalAmmo;
        }
    }
}
