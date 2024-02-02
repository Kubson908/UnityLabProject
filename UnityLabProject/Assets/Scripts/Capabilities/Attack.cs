using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// skrypt obs³uguj¹cy strzelanie z broni postaci
public class Attack : MonoBehaviour
{
    [SerializeField] private WeaponController weapon = null;
    [SerializeField] private float shootDelay = 0.13f; // opóŸnienie miêdzy kolejnymi strza³ami
    [SerializeField] private bool fullAuto = true;  // tryb strzelania full-auto
    [SerializeField] private AudioClip noAmmo; // dŸwiêk pustego magazynka
    [SerializeField] private Text ammoDisplay;
    [SerializeField] private float reloadTime;  // czas prze³adowania
    [SerializeField] private GameObject mag;
    [SerializeField] private GameObject magPrefab;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioClip reload;  // dŸwiêk prze³adowania
    [SerializeField] private Transform hand;

    public Transform firePoint; // punkt w którym inicjalizujemy pocisk
    public GameObject bulletPrefab;

    private AudioSource audioSource;
    public bool isFiring;
    private float timeCounter; // licznik czasu miêdzy wystrza³ami
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
        look = transform.root.GetComponent<LookAtCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;

        if (weapon.RetrieveReloadClick())
        {
            StartCoroutine(Reload());   // uruchomienie procesu prze³adowania
            return;
        }

        if (weapon.RetrieveLeftClick() && manager.currentHealth > 0)
        {
            isFiring = true;
        }
        else if (weapon.RetrieveLeftRelease())
        {
            isFiring = false;
        }

        if (isFiring)   // je¿eli postaæ jest w trakcie strzelania
        {
            timeCounter -= Time.deltaTime;

            if (timeCounter <= 0)   // sprawdzamy czy minê³o opóŸnienie wystrza³u
            {
                timeCounter = shootDelay;
                Shoot();    // strza³
            }
        }
        else
        {
            timeCounter -= Time.deltaTime;
        }
            

        
    }

    private void Shoot()
    {
        if (manager.paused) return; // je¿eli gra jest zapauzowana nie ma mo¿liwoœci strzelania
        if (!manager.rifle && manager.pistolMagazine != 0)  // gdy u¿ywamy pistoletu z niepustym magazynkiem
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(audioSource.clip);
            manager.pistolMagazine--;
            ammoDisplay.text = manager.pistolMagazine + "/" + manager.pistolTotalAmmo;
            weaponAnimator.SetTrigger("ShootPistol");
            if (!fullAuto)
            {
                isFiring = false;   // je¿eli tryb full-auto jest wy³¹czony przerywamy strzelanie po pierwszym wystrzale
            }
        }
        else if (manager.rifle && manager.rifleMagazine != 0)   // karabin z niepustym magazynkiem
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(audioSource.clip);
            manager.rifleMagazine--;
            ammoDisplay.text = manager.rifleMagazine + "/" + manager.rifleTotalAmmo;
            weaponAnimator.SetTrigger("IsFiring");
            if (!fullAuto)
            {
                isFiring = false;   // je¿eli tryb full-auto jest wy³¹czony przerywamy strzelanie po pierwszym wystrzale
            }
        }
        else
        {
            audioSource.PlayOneShot(noAmmo);    // gdy nie ma pocisków w magazynku puszczamy dŸwiêk pustego magazynka
        }
    }

    private IEnumerator Reload()
    {
        if (manager.paused) yield return null;  // je¿eli gra jest zapauzowana nie ma mo¿liwoœci prze³adowania
        isReloading = true;
        isFiring = false;
        if (NoAmmo())    // jeœli brak amunicji wy³¹czamy prze³adowanie
        {
            isReloading = false;
        }
        else
        {
            audioSource.PlayOneShot(reload);    // dŸwiêk prze³adowania
            // ustawienie odpowiednich animacji
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
            // dodatkowe obliczenia i ustawienia od³¹czania magazynka i wk³adania nowego podczas trwania animacji
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
            ManageReloadAmmo(); // zarz¹dzanie iloœci¹ amunicji w Game Manager oraz wyœwietlanie informacji na ekranie
            isReloading = false;
            if (!manager.rifle) playerAnimator.SetBool("ReloadingPistol", false);
            else playerAnimator.SetBool("ReloadingRifle", false);
        }
    }

    private bool NoAmmo()
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
            manager.rifleMagazine += diff <= manager.rifleTotalAmmo ? diff : manager.rifleTotalAmmo;
            manager.rifleTotalAmmo -= diff <= manager.rifleTotalAmmo ? diff : manager.rifleTotalAmmo;
            ammoDisplay.text = manager.rifleMagazine + "/" + manager.rifleTotalAmmo;
        }
        else
        {
            int diff = manager.pistolCapacity - manager.pistolMagazine;
            manager.pistolMagazine += diff <= manager.pistolTotalAmmo ? diff : manager.pistolTotalAmmo;
            manager.pistolTotalAmmo -= diff <= manager.pistolTotalAmmo ? diff : manager.pistolTotalAmmo;
            ammoDisplay.text = manager.pistolMagazine + "/" + manager.pistolTotalAmmo;
        }
    }
}
