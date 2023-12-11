using UnityEngine;

public class ManageLayerOrder : MonoBehaviour
{
    [SerializeField] private int layerOrder;
    void Start()
    {
        // ustawiamy warstwy przeciwników aby unikn¹æ ich przenikania siê
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.sortingOrder += layerOrder * 17;
        }
    }
}
