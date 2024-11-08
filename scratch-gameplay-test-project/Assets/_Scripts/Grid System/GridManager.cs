using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridItemSO gridItemSo;

    public int rows = 5;
    public int columns = 5;
    public Vector2 generateStartPoint = Vector2.zero;

    private GridData _gridData;
    private GridGenerator gridGenerator;
    // private IconMover iconMover;
    // private ClusterDetector clusterDetector;

    void Start()
    {
        _gridData = new GridData();
        gridGenerator = new GridGenerator(rows, columns, generateStartPoint, gridItemSo, _gridData);
        // iconMover = new IconMover(gridData);
        // clusterDetector = new ClusterDetector(gridData);

        // gridGenerator.OnGridGenerated += OnGridGenerated;
        gridGenerator.GenerateAllGrids();

        foreach (var item in _gridData.items)
        {
            print(item.GetComponent<GridItem>().itemData.id);
        }
    }

    private void OnGridGenerated()
    {
        // 网格生成完成后的逻辑
    }

    // private void MoveIcons()
    // {
    //     iconMover.MoveIcons();
    // }

    // private void CheckClusters()
    // {
    //     clusterDetector.CheckClusters(revealedGrids);
    // }
}
