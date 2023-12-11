using Unity.VisualScripting;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursor;
    public Transform weaponTransform;
    public Transform headTransform;
    public Transform upperSpineTransform;
    public Transform targetTransform;

    private new Camera camera;
    public bool facingRight = true;
    private Quaternion startSpineRotation;
/*    private Quaternion startingWeaponRotation;*/

    void Start()
    {
        camera = Camera.main;
        startSpineRotation = upperSpineTransform.rotation; // pocz�tkowa rotacja kr�gos�upa
        /*startingWeaponRotation = weaponTransform.rotation;*/
        Cursor.SetCursor(cursor, new Vector2(32, 32), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentHealth > 0 && !GameManager.Instance.paused) // celowanie w stron� kursora jest mo�liwe je�li posta� �yje i gra nie jest zatrzymana
            FaceMouse();
        else if (!GameManager.Instance.paused)  // je�li posta� umiera i gra nie jest zatrzymana ustawiamy rotacj� jako identity aby unikn�� nieoczekiwanych ustawie� podczas animacji
        {
            weaponTransform.rotation = Quaternion.identity;
            upperSpineTransform.rotation = Quaternion.identity;
        }
    }

    private void FaceMouse()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);     // pobieramy pozycj� kursora na ekranie
        // sprawdzamy czy kursor jest odpowiednio daleko od �rodka obrotu broni aby unikn�� efektu ci�g�ego przeskakiwania postaci
        if (Vector2.Distance(mouseWorldPos, weaponTransform.position) < 0.35f || 
            (mouseWorldPos.y < weaponTransform.position.y && Mathf.Abs(weaponTransform.position.x - mouseWorldPos.x) < 0.20f )) return;

        Vector2 direction = new(weaponTransform.position.x - mouseWorldPos.x, weaponTransform.position.y - mouseWorldPos.y); // obliczenie kierunku celowania
        
        // obr�cenie postaci w stron� kursora
        if (mouseWorldPos.x <= weaponTransform.position.x && facingRight)
        {
            Flip();
        }
        else if (mouseWorldPos.x >= weaponTransform.position.x && !facingRight)
        {
            Flip();
        }

        // ustawiamy obiekt pomocniczy w stron� kierunku kursora
        targetTransform.rotation = headTransform.rotation;
        targetTransform.up = direction;
        float diff = Mathf.Abs(targetTransform.rotation.z) - 0.7112001f;
        if (diff < 0.13f && diff > -0.13f) // sprawdzamy czy kierunek mie�ci si� w granicy w miar� naturalnego u�o�enia g�owy postaci
        {
            headTransform.up = direction; // obracamy g�ow� w kierunku kursora
            // nast�pnie ustawiamy kr�gos�up w rotacji pocz�tkowej
            if (facingRight)
                upperSpineTransform.rotation = startSpineRotation;
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z, startSpineRotation.w);
        }
        else // je�eli kursor jest za nisko posta� pochyla si� do przodu lub do ty�u aby poprawi� realizm celowania
        {
            if (facingRight)
                upperSpineTransform.rotation = new Quaternion(0, 0, startSpineRotation.z + diff / 2, upperSpineTransform.rotation.w);
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z - diff / 2, upperSpineTransform.rotation.w);
        }
        weaponTransform.up = direction; // na koniec ustawiamy bro� w kierunku kursora
    }

    // funkcja do odwracania postaci
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }
}
