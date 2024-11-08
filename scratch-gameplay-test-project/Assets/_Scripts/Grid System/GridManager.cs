using System;
using System.Collections.Generic;
using _Scripts.Merger;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static Action<Vector2Int, bool> onCoverRevealStateChanged;
    public static Action<Vector2Int> onMouseOverRevealedItem;
    public static Action onMouseExitRevealedItem;

    public GridItemSO gridItemSo;
    public GameObject clusterBGPrefab;

    public int rows = 5;
    public int columns = 5;
    public Vector2 generateStartPoint = Vector2.zero;

    private GridData _gridData;
    private GridGenerator gridGenerator;
    private GridItemMerger gridItemMerger;
    // private IconMover iconMover;
    private ClusterDetector clusterDetector;

    private void OnEnable()
    {
        onCoverRevealStateChanged += OnGridRevealStateChanged;
        onMouseOverRevealedItem += OnMouseOverRevealedItem;
        onMouseExitRevealedItem += OnMouseExitRevealedItem;
    }

    private void OnDisable()
    {
        onCoverRevealStateChanged -= OnGridRevealStateChanged;
        onMouseOverRevealedItem -= OnMouseOverRevealedItem;
        onMouseExitRevealedItem -= OnMouseExitRevealedItem;
    }

    void Start()
    {
        _gridData = new GridData();
        gridGenerator = new GridGenerator(rows, columns, generateStartPoint, gridItemSo, _gridData);
        // iconMover = new IconMover(gridData);
        clusterDetector = new ClusterDetector(rows, columns, _gridData);

        // gridGenerator.OnGridGenerated += OnGridGenerated;
        gridGenerator.GenerateAllGrids();

        // gridItemMerger = new GridItemMerger(_gridData.items, )

        // foreach (var item in _gridData.items[Gri])
        // {
        //     print(item.GetComponent<GridItem>().);
        //     print(item.GetComponent<GridItem>().itemData.level);
        // }
    }

    private void OnGridGenerated()
    {
        // 网格生成完成后的逻辑
    }

    // private void MoveIcons()
    // {
    //     iconMover.MoveIcons();
    // }

    private void OnGridRevealStateChanged(Vector2Int revealedGrid, bool isRevealed)
    {
        if (isRevealed) _gridData.revealedGrids.Add(revealedGrid);
        else _gridData.revealedGrids.Remove(revealedGrid);
    }

    private void OnMouseOverRevealedItem(Vector2Int originItemGrid)
    {
        var cluster = clusterDetector.CheckClusters(originItemGrid);
        if (cluster.Count < 2) return;
        // set bg color
        foreach (var i in cluster)
        {
            Instantiate(clusterBGPrefab, i.transform);
        }
    }

    private void OnMouseExitRevealedItem()
    {
        // reset color
        var clusterBGs = GameObject.FindGameObjectsWithTag("ClusterBG");
        foreach (var c in clusterBGs) Destroy(c);
    }
}
