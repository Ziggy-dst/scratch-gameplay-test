using System.Collections.Generic;
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

    private void GenerateCover(Transform parent, int row, int column)
    {
        GameObject cover = new GameObject("cover_" + row + "_" + column)
        {
            transform =
            {
                // set position
                parent = parent.transform,
                position = new Vector2(_startPoint.x + column, _startPoint.y - row)
            }
        };
        var spriteRenderer = cover.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("DefaultAssets/Textures/Square");
        spriteRenderer.color = Color.gray;

        spriteRenderer.sortingOrder = 999;
        cover.AddComponent<MergerGridCover>().grid = new Vector2Int(row, column);
    }

    public ItemData FetchGridItem(GridItemType type, int level)
    {
        return _gridItemSo.itemPool[type].itemLevelData[level];
    }

    public void GenerateRandomGrid(Transform parent, int row, int column)
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

        // randomize icon item
        var itemType = Utils.CalculateMultiProbability(_gridItemSo.itemPool);
        int randItemLevelDataIndex = Utils.CalculateMultiProbability(_gridItemSo.itemPool[itemType].itemLevelData);
        var itemData = FetchGridItem(itemType, randItemLevelDataIndex);

        var gridItem = itemObject.AddComponent<GridItem>();
        gridItem.type = itemType;
        gridItem.itemData = itemData;

        _gridData.items[row, column] = gridItem;

        // SetItemData(gridItem, randItemData);

        // set sprite
        SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer>();
        sr.sprite = itemData.image;

        // add cover
        GenerateCover(itemObject.transform, row, column);
    }

    public void GenerateSingleGrid(Transform parent, int row, int column, GridItemType type, int level)
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

        var itemType = type;
        var itemData = FetchGridItem(itemType, level);

        var gridItem = itemObject.AddComponent<GridItem>();
        gridItem.type = itemType;
        gridItem.itemData = itemData;

        _gridData.items[row, column] = gridItem;

        // SetItemData(gridItem, randItemData);

        // set sprite
        SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer>();
        sr.sprite = itemData.image;

        // add cover
        GenerateCover(itemObject.transform, row, column);
    }

    public void GenerateAllGrids()
    {
        _gridData.items = new GridItem[_rows, _columns];
        _gridData.revealedGrids = new List<Vector2Int>();

        GameObject iconParentObject = new GameObject("Icons");

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                GenerateRandomGrid(iconParentObject.transform, i, j);
            }
        }
    }
}
