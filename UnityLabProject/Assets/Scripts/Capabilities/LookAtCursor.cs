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
        startSpineRotation = upperSpineTransform.rotation; // pocz¹tkowa rotacja krêgos³upa
        /*startingWeaponRotation = weaponTransform.rotation;*/
        Cursor.SetCursor(cursor, new Vector2(32, 32), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentHealth > 0 && !GameManager.Instance.paused) // celowanie w stronê kursora jest mo¿liwe jeœli postaæ ¿yje i gra nie jest zatrzymana
            FaceMouse();
        else if (!GameManager.Instance.paused)  // jeœli postaæ umiera i gra nie jest zatrzymana ustawiamy rotacjê jako identity aby unikn¹æ nieoczekiwanych ustawieñ podczas animacji
        {
            weaponTransform.rotation = Quaternion.identity;
            upperSpineTransform.rotation = Quaternion.identity;
        }
    }

    private void FaceMouse()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);     // pobieramy pozycjê kursora na ekranie
        // sprawdzamy czy kursor jest odpowiednio daleko od œrodka obrotu broni aby unikn¹æ efektu ci¹g³ego przeskakiwania postaci
        if (Vector2.Distance(mouseWorldPos, weaponTransform.position) < 0.35f || 
            (mouseWorldPos.y < weaponTransform.position.y && Mathf.Abs(weaponTransform.position.x - mouseWorldPos.x) < 0.20f )) return;

        Vector2 direction = new(weaponTransform.position.x - mouseWorldPos.x, weaponTransform.position.y - mouseWorldPos.y); // obliczenie kierunku celowania
        
        // obrócenie postaci w stronê kursora
        if (mouseWorldPos.x <= weaponTransform.position.x && facingRight)
        {
            Flip();
        }
        else if (mouseWorldPos.x >= weaponTransform.position.x && !facingRight)
        {
            Flip();
        }

        // ustawiamy obiekt pomocniczy w stronê kierunku kursora
        targetTransform.rotation = headTransform.rotation;
        targetTransform.up = direction;
        float diff = Mathf.Abs(targetTransform.rotation.z) - 0.7112001f;
        if (diff < 0.13f && diff > -0.13f) // sprawdzamy czy kierunek mieœci siê w granicy w miarê naturalnego u³o¿enia g³owy postaci
        {
            headTransform.up = direction; // obracamy g³owê w kierunku kursora
            // nastêpnie ustawiamy krêgos³up w rotacji pocz¹tkowej
            if (facingRight)
                upperSpineTransform.rotation = startSpineRotation;
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z, startSpineRotation.w);
        }
        else // je¿eli kursor jest za nisko postaæ pochyla siê do przodu lub do ty³u aby poprawiæ realizm celowania
        {
            if (facingRight)
                upperSpineTransform.rotation = new Quaternion(0, 0, startSpineRotation.z + diff / 2, upperSpineTransform.rotation.w);
            else
                upperSpineTransform.rotation = new Quaternion(0, 0, -startSpineRotation.z - diff / 2, upperSpineTransform.rotation.w);
        }
        weaponTransform.up = direction; // na koniec ustawiamy broñ w kierunku kursora
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
