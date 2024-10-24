using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// remove revealed grid after removing icons!!!
/// </summary>
public class IconManager : MonoBehaviour
{
    public static Action<Vector2Int> OnCoverRevealed;
    public static Action<int> OnScoreGained;
    public static bool isIconMoving = false;

    public IconItemSO IconItemSo;

    public int rows = 5;
    public int columns = 5;
    public Vector2 generateStartPoint = Vector2.zero;

    public GameObject coverPrefab;
    public GameObject clusterBGPrefab;

    private Transform[,] iconObjects;
    // index: grid, content: iconItems
    private IconItemSO.IconItem[,] iconItems;
    // grid, position
    private Vector2[,] gridPositions;

    // grid of revealed icon grids
    private List<Vector2Int> revealedGrids = new List<Vector2Int>();
    private List<Vector2Int> movingGrids = new List<Vector2Int>();

    private bool[,] visitedGrids;
    private List<Vector2Int> cluster = new List<Vector2Int>();

    private List<Vector2Int> scoredGrid = new List<Vector2Int>();

    private int revealedCoverCount = 0;

    void Start()
    {
        GenerateGrids();

        InvokeRepeating(nameof(MoveIcons), 0f, 1.5f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) MoveIcons();
        if(Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    private void GenerateGrids()
    {
        iconObjects = new Transform[rows, columns];
        iconItems = new IconItemSO.IconItem[rows, columns];
        gridPositions = new Vector2[rows, columns];

        GameObject iconParentObject = new GameObject("Icons");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject iconObject = new GameObject("Icon_" + i + "_" + j);

                // set position
                iconObject.transform.parent = iconParentObject.transform;
                iconObject.transform.position = new Vector2(generateStartPoint.x + j, generateStartPoint.y - i);
                iconObjects[i, j] = iconObject.transform;
                gridPositions[i, j] = iconObject.transform.position;

                // randomize icon item
                var randIconItem = IconItemSo.iconPool[Random.Range(0, IconItemSo.iconPool.Count)];
                iconItems[i, j] = randIconItem;

                // set sprite
                SpriteRenderer sr = iconObject.AddComponent<SpriteRenderer>();
                sr.sprite = randIconItem.image;

                // add cover
                GameObject cover = Instantiate(coverPrefab, iconObject.transform);
                cover.GetComponent<GridCover>().grid = new Vector2Int(i, j);

                // AddRevealedIconGrid(new Vector2Int(i, j));
            }
        }
    }

    private void MoveIcons()
    {
        int movingGridsCount = movingGrids.Count;

        if (movingGridsCount < 2) return;

        var lastRevealedIconGrid = iconObjects[movingGrids[movingGridsCount-1].x, movingGrids[movingGridsCount-1].y];
        var lastRevealedIconItem = iconItems[movingGrids[movingGridsCount-1].x, movingGrids[movingGridsCount-1].y];

        for (int i = movingGridsCount - 1; i > 0; i--)
        {
            // check the destination
            Vector2 destination = gridPositions[movingGrids[i].x, movingGrids[i].y];

            // move the sprite
            iconObjects[movingGrids[i - 1].x, movingGrids[i - 1].y].DOMove(destination, .5f)
                .SetEase(Ease.Linear)
                .OnStart(() => isIconMoving = true)
                .OnComplete(() => isIconMoving = false);

            // update the sprite renderer grids list
            iconObjects[movingGrids[i].x, movingGrids[i].y] =
                iconObjects[movingGrids[i - 1].x, movingGrids[i - 1].y];

            iconItems[movingGrids[i].x, movingGrids[i].y] =
                iconItems[movingGrids[i - 1].x, movingGrids[i - 1].y];
        }

        lastRevealedIconGrid.DOMove(gridPositions[movingGrids[0].x, movingGrids[0].y], .5f)
            .SetEase(Ease.Linear)
            .OnStart(() => isIconMoving = true)
            .OnComplete(() => isIconMoving = false);;
        iconObjects[movingGrids[0].x, movingGrids[0].y] = lastRevealedIconGrid;
        iconItems[movingGrids[0].x, movingGrids[0].y] = lastRevealedIconItem;

        CheckClusters();
    }

#region Reveal

    private void OnEnable() => OnCoverRevealed += OnCoverReveal;
    private void OnDisable() => OnCoverRevealed -= OnCoverReveal;

    private void OnCoverReveal(Vector2Int newGrid)
    {
        // stop when all covers are revealed
        revealedCoverCount++;
        if (revealedCoverCount == rows * columns) CancelInvoke();

        AddRevealedIconGrid(newGrid);
        CheckClusters();
        // MoveIcons();
    }

    private void AddRevealedIconGrid(Vector2Int newGrid)
    {
        foreach (var grid in movingGrids)
        {
            if (grid.x > newGrid.x)
            {
                int insertIndex = movingGrids.IndexOf(grid);
                movingGrids.Insert(insertIndex, newGrid);
                // print("insert index: " + insertIndex);
                break;
            }

            if (grid.x == newGrid.x)
            {
                if (grid.y > newGrid.y)
                {
                    int insertIndex = movingGrids.IndexOf(grid);
                    movingGrids.Insert(insertIndex, newGrid);
                    // print("insert index: " + insertIndex);
                    break;
                }
            }
        }

        revealedGrids.Add(newGrid);

        if (movingGrids.Contains(newGrid))
        {
            Debug.LogError("repeat moving grid founded");
            return;
        }
        movingGrids.Add(newGrid);
    }

#endregion

#region Result Check

    private void CalculateScore(int iconPrize)
    {
        // TODO: score algorithm
        int score = iconPrize;

        OnScoreGained?.Invoke(score);
    }


    private void CheckClusters()
    {
        visitedGrids = new bool[rows, columns];

        foreach (var grid in revealedGrids)
        {
            if (visitedGrids[grid.x, grid.y]) continue;

            cluster = new List<Vector2Int>();
            FindCluster(grid.x, grid.y, iconItems[grid.x, grid.y].id);

            if (cluster.Count >= 3)
            {
                foreach (var icon in cluster)
                {
                    if (scoredGrid.Contains(icon)) continue;
                    scoredGrid.Add(icon);
                    movingGrids.Remove(icon);
                    Instantiate(clusterBGPrefab, iconObjects[icon.x, icon.y].transform);

                    // score
                    CalculateScore(iconItems[icon.x, icon.y].prize);
                }

                // print("> 3 cluster count: " + cluster.Count);
            }

            // print("cluster count: " + cluster.Count);
        }
    }

    private void FindCluster(int x, int y, string iconId)
    {
        // 检查是否越界或者已经访问过
        if (x < 0 || x >= rows || y < 0 || y >= columns || visitedGrids[x, y])
            return;

        if (!revealedGrids.Contains(new Vector2Int(x, y)))
            return;

        // 如果当前格子的类型不匹配，直接返回
        if (iconItems[x, y].id != iconId)
            return;

        // 标记为已访问
        visitedGrids[x, y] = true;

        // 将当前格子加入到聚集中
        cluster.Add(new Vector2Int(x, y));

        // 递归检查上下左右四个方向
        FindCluster(x + 1, y, iconId); // 右
        FindCluster(x - 1, y, iconId); // 左
        FindCluster(x, y + 1, iconId); // 上
        FindCluster(x, y - 1, iconId); // 下
    }
#endregion
}
