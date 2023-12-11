using Cinemachine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    [SerializeField] Slider healthBar;

    [Header("Ammo")]
    public int pistolCapacity = 8;
    public int pistolMagazine = 8;
    public int pistolTotalAmmo = 40;
    public int rifleCapacity = 30;
    public int rifleMagazine = 30;
    public int rifleTotalAmmo = 90;
    public bool rifle = false;
    [SerializeField] Text ammoInfo;
    [SerializeField] Image weaponIcon;
    [SerializeField] Sprite AK47Icon;
    [SerializeField] Sprite pistolIcon;

    [Header("Enemies")]
    [SerializeField] private Text enemiesCounter;
    [SerializeField] private Text eliteEnemiesCounter;
    public int enemiesLeft = 0;
    public int eliteEnemiesLeft = 0;

    [Header("PlayerConfig")]
    [SerializeField] GameObject playerAK47;
    [SerializeField] GameObject playerPistol;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Attack rifleAttackController;
    [SerializeField] Attack pistolAttackController;
    public bool canChangeWeapon = false;

    [Header("Game Status")]
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelCompletedUI;
    [SerializeField] GameObject pauseMenu;
    public bool paused = false;
    public static GameManager Instance;

    private PlayerHealthController playerPistolHealthController;
    private PlayerHealthController playerAK47HealthController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Count();
        eliteEnemiesLeft = GameObject.FindGameObjectsWithTag("EliteEnemy").Count();
        if (eliteEnemiesLeft == 0) eliteEnemiesCounter.gameObject.SetActive(false);
        enemiesCounter.text = enemiesLeft.ToString();
        eliteEnemiesCounter.text = eliteEnemiesLeft.ToString();
        playerPistolHealthController = playerPistol.GetComponent<PlayerHealthController>();
        playerAK47HealthController = playerAK47.GetComponent<PlayerHealthController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ammoInfo.text = rifle ? rifleMagazine + "/" + rifleTotalAmmo : pistolMagazine + "/" + pistolTotalAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHP();
        if (Input.GetKeyDown(KeyCode.Alpha1) && rifle)
            ChangeWeapon(false);
        if (Input.GetKeyDown(KeyCode.Alpha2) && !rifle)
            ChangeWeapon(true);

        if (MagazineEmpty() && enemiesLeft != 0 && eliteEnemiesLeft != 0) StartCoroutine(GameOver());
        else if (enemiesLeft == 0 && eliteEnemiesLeft == 0) StartCoroutine(LevelCompleted());

        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
    }

    private bool MagazineEmpty()
    {
        bool noAmmo = pistolMagazine == 0 && pistolTotalAmmo == 0;
        if (canChangeWeapon) noAmmo &= rifleMagazine == 0 && rifleTotalAmmo == 0;
        return noAmmo;
    }

    void DisplayHP()
    {
        UpdateHealthBar(currentHealth, maxHealth);
    }

    public void UpdateHealthBar(int currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    public void ChangeWeapon(bool rifle)
    {
        if (!canChangeWeapon || (pistolAttackController && pistolAttackController.isFiring) ||
            (rifleAttackController && rifleAttackController.isFiring)) return;
        var pistolAnimator = playerPistol.GetComponent<Animator>();
        var rifleAnimator = playerAK47.GetComponent<Animator>();
        if (rifle)
        {
            if ((pistolAnimator.isActiveAndEnabled && pistolAnimator.GetBool("ReloadingPistol")) ||
                rifleAnimator.GetBool("ReloadingRifle")) return;
            playerAK47.transform.position = playerPistol.transform.position;
            playerPistol.SetActive(false);
            playerAK47.SetActive(true);
            weaponIcon.sprite = AK47Icon;
            virtualCamera.Follow = playerAK47.transform;
            ammoInfo.text = rifleMagazine + "/" + rifleTotalAmmo;
        }
        else
        {
            if ((rifleAnimator.isActiveAndEnabled && rifleAnimator.GetBool("ReloadingRifle")) || 
                pistolAnimator.GetBool("ReloadingPistol")) return;
            playerPistol.transform.position = playerAK47.transform.position;
            playerPistol.SetActive(true);
            playerAK47.SetActive(false);
            weaponIcon.sprite = pistolIcon;
            virtualCamera.Follow = playerPistol.transform;
            ammoInfo.text = pistolMagazine + "/" + pistolTotalAmmo;
        }
        this.rifle = rifle;
    }

    public void UpdateEnemiesCounter(bool elite)
    {
        if (elite) eliteEnemiesCounter.text = eliteEnemiesLeft.ToString();
        else enemiesCounter.text = enemiesLeft.ToString();
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= currentHealth > damage ? damage : currentHealth;
        if (currentHealth <= 0)
        {
            if (rifle) playerAK47HealthController.Die();
            else playerPistolHealthController.Die();
            StartCoroutine(GameOver());
        }
    }

    private void Pause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        playerAK47.GetComponent<LookAtCursor>().gameObject.SetActive(false);
        playerPistol.GetComponent<LookAtCursor>().gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    private IEnumerator LevelCompleted()
    {
        yield return new WaitForSeconds(3);
        playerAK47.GetComponent<LookAtCursor>().gameObject.SetActive(false);
        playerAK47.GetComponentInChildren<Attack>().gameObject.SetActive(false);
        playerPistol.GetComponent<LookAtCursor>().gameObject.SetActive(false);
        playerPistol.GetComponentInChildren<Attack>().gameObject.SetActive(false);
        levelCompletedUI.SetActive(true);
        Time.timeScale = 0;
    }
}
