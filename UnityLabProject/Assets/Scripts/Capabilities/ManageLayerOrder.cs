using UnityEngine;

public class ManageLayerOrder : MonoBehaviour
{
    [SerializeField] private int layerOrder;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.sortingOrder += layerOrder * 17;
        }
    }
}
