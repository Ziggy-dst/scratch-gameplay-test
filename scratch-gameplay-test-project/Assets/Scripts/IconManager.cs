using System;
using System.Collections.Generic;
using System.Linq;
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
    public static bool isIconMoving = false;

    public int rows = 5;
    public int columns = 5;
    public Vector2 generateStartPoint = Vector2.zero;
    public Sprite[] sprites;

    public GameObject coverPrefab;
    public GameObject clusterBGPrefab;

    // index: grid, content: spriteRenderer
    private SpriteRenderer[,] spriteRenderers;
    // grid, position
    private Vector2[,] gridPositions;
    // grid of revealed icon grids
    private List<Vector2Int> revealedGrids = new List<Vector2Int>();

    private List<Vector2Int> movingGrids = new List<Vector2Int>();

    private bool[,] visitedGrids;
    private List<Vector2Int> cluster;

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
        spriteRenderers = new SpriteRenderer[rows, columns];
        gridPositions = new Vector2[rows, columns];

        GameObject spriteParentObject = new GameObject("Icons");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject spriteObject = new GameObject("Icon_" + i + "_" + j);
                spriteObject.transform.parent = spriteParentObject.transform;
                spriteObject.transform.position = new Vector2(generateStartPoint.x + j, generateStartPoint.y - i);
                SpriteRenderer sr = spriteObject.AddComponent<SpriteRenderer>();
                sr.sprite = sprites[Random.Range(0, sprites.Length)];

                // add cover
                GameObject cover = Instantiate(coverPrefab, spriteObject.transform);
                cover.GetComponent<GridCover>().grid = new Vector2Int(i, j);

                gridPositions[i, j] = sr.transform.position;
                spriteRenderers[i, j] = sr;

                // AddRevealedIconGrid(new Vector2Int(i, j));
            }
        }
    }

    private void MoveIcons()
    {
        int movingGridsCount = movingGrids.Count;

        if (movingGridsCount < 2) return;

        SpriteRenderer lastRevealedIconGrid = spriteRenderers[movingGrids[movingGridsCount-1].x, movingGrids[movingGridsCount-1].y];

        for (int i = movingGridsCount - 1; i > 0; i--)
        {
            // check the destination
            Vector2 destination = gridPositions[movingGrids[i].x, movingGrids[i].y];

            // move the sprite
            spriteRenderers[movingGrids[i - 1].x, movingGrids[i - 1].y].transform.DOMove(destination, .5f)
                .SetEase(Ease.Linear)
                .OnStart(() => isIconMoving = true)
                .OnComplete(() => isIconMoving = false);

            // update the sprite renderer grids list
            spriteRenderers[movingGrids[i].x, movingGrids[i].y] =
                spriteRenderers[movingGrids[i - 1].x, movingGrids[i - 1].y];
        }

        lastRevealedIconGrid.transform.DOMove(gridPositions[movingGrids[0].x, movingGrids[0].y], .5f)
            .SetEase(Ease.Linear)
            .OnStart(() => isIconMoving = true)
            .OnComplete(() => isIconMoving = false);;
        spriteRenderers[movingGrids[0].x, movingGrids[0].y] = lastRevealedIconGrid;

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
    private void CheckClusters()
    {
        visitedGrids = new bool[rows, columns];

        foreach (var grid in revealedGrids)
        {
            if (visitedGrids[grid.x, grid.y]) continue;

            cluster = new List<Vector2Int>();
            FindCluster(grid.x, grid.y, spriteRenderers[grid.x, grid.y]);

            if (cluster.Count >= 3)
            {
                foreach (var icon in cluster)
                {
                    movingGrids.Remove(icon);
                    Instantiate(clusterBGPrefab, spriteRenderers[icon.x, icon.y].transform);
                }

                print("> 3 cluster count: " + cluster.Count);
            }

            print("cluster count: " + cluster.Count);
        }
    }

    private void FindCluster(int x, int y, SpriteRenderer icon)
    {
        // 检查是否越界或者已经访问过
        if (x < 0 || x >= rows || y < 0 || y >= columns || visitedGrids[x, y])
            return;

        if (!revealedGrids.Contains(new Vector2Int(x, y)))
            return;

        // 如果当前格子的类型不匹配，直接返回
        if (spriteRenderers[x, y].sprite != icon.sprite)
            return;

        // 标记为已访问
        visitedGrids[x, y] = true;

        // 将当前格子加入到聚集中
        cluster.Add(new Vector2Int(x, y));

        // 递归检查上下左右四个方向
        FindCluster(x + 1, y, icon); // 右
        FindCluster(x - 1, y, icon); // 左
        FindCluster(x, y + 1, icon); // 上
        FindCluster(x, y - 1, icon); // 下
    }
#endregion
}
