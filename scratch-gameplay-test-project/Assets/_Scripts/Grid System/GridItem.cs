using UnityEngine;

public class GridItem : MonoBehaviour
{
    public GridItemType type { get; private set; }
    public ItemData itemData { get; private set; }

    public void Initialize(GridItemType type, ItemData itemData)
    {
        this.type = type;
        this.itemData = itemData;
    }
}
