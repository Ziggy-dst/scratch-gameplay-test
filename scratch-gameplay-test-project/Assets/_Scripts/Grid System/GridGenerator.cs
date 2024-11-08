using UnityEngine;

public class GridGenerator
{
    private GridItemSO _gridItemSo;

    private int _rows;
    private int _columns;
    private Vector2 _startPoint;
    private GridData _gridData;

    public GridGenerator(int rows, int columns, Vector2 startPoint, GridItemSO gridItemSo, GridData gridData)
    {
        _rows = rows;
        _columns = columns;
        _startPoint = startPoint;
        _gridItemSo = gridItemSo;
        _gridData = gridData;
    }

    // private void SetItemData(ItemData item, ItemData data)
    // {
    //     item.image = data.image;
    //     item.id = data.id;
    //     item.probability = data.probability;
    //     item.prize = data.prize;
    //     item.type = data.type;
    //     item.level = data.level;
    // }

    private void GenerateCover(Transform parent, int i, int j)
    {
        GameObject cover = new GameObject("cover_" + i + "_" + j)
        {
            transform =
            {
                // set position
                parent = parent.transform,
                position = new Vector2(_startPoint.x + j, _startPoint.y - i)
            }
        };
        cover.AddComponent<SpriteRenderer>().color = Color.gray;
        cover.AddComponent<GridCover>().grid = new Vector2Int(i, j);
    }

    public void GenerateSingleGrid(Transform parent, int row, int column)
    {
        GameObject itemObject = new GameObject("Item" + row + "_" + column)
        {
            transform =
            {
                // set position
                parent = parent.transform,
                position = new Vector2(_startPoint.x + column, _startPoint.y - row)
            }
        };

        _gridData.items[row, column] = itemObject.transform;

        // randomize icon item
        var randItemData = _gridItemSo.itemPool[Utils.CalculateMultiProbability(_gridItemSo.itemPool)];
        // var gridItem = itemObject.AddComponent<GridItem>().itemData;
        itemObject.AddComponent<GridItem>().itemData = randItemData;

        // SetItemData(gridItem, randItemData);

        // set sprite
        SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer>();
        sr.sprite = randItemData.image;

        // add cover
        GenerateCover(itemObject.transform, row, column);
    }

    public void GenerateAllGrids()
    {
        _gridData.items = new Transform[_rows, _columns];
        GameObject iconParentObject = new GameObject("Icons");

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                GenerateSingleGrid(iconParentObject.transform, i, j);
            }
        }
    }
}
