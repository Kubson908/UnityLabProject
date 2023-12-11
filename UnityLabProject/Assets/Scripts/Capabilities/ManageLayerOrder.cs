using UnityEngine;

public class ManageLayerOrder : MonoBehaviour
{
    [SerializeField] private int layerOrder;
    void Start()
    {
        // ustawiamy warstwy przeciwnik�w aby unikn�� ich przenikania si�
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.sortingOrder += layerOrder * 17;
        }
    }
}
