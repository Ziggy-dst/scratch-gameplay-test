using System.Collections.Generic;
using UnityEngine;

public class ClusterDetector
{
    // private GridData _gridData;
    //
    // private int _rows;
    // private int _columns;
    //
    // // grid of revealed icon grids
    // private List<Vector2Int> revealedGrids = new List<Vector2Int>();
    // private List<Vector2Int> movingGrids = new List<Vector2Int>();
    //
    // private bool[,] visitedGrids;
    // private List<Vector2Int> cluster = new List<Vector2Int>();
    //
    // private List<Vector2Int> scoredGrid = new List<Vector2Int>();
    //
    // public ClusterDetector(int rows, int columns, GridData gridData)
    // {
    //
    //     this._gridData = gridData;
    // }
    //
    // public void CheckClusters(List<Vector2Int> revealedGrids)
    // {
    //     // 使用 gridData.IconItems 检查集群
    // }
    //
    // private void CheckClusters()
    // {
    //     visitedGrids = new bool[_rows, _columns];
    //
    //     foreach (var grid in revealedGrids)
    //     {
    //         if (visitedGrids[grid.x, grid.y]) continue;
    //
    //         cluster = new List<Vector2Int>();
    //         FindCluster(grid.x, grid.y, iconItems[grid.x, grid.y].id);
    //
    //         if (cluster.Count >= 3)
    //         {
    //             foreach (var icon in cluster)
    //             {
    //                 if (scoredGrid.Contains(icon)) continue;
    //                 scoredGrid.Add(icon);
    //                 movingGrids.Remove(icon);
    //                 // Instantiate(clusterBGPrefab, iconObjects[icon.x, icon.y].transform);
    //
    //                 // score
    //                 // CalculateScore(iconItems[icon.x, icon.y].prize);
    //             }
    //
    //             // print("> 3 cluster count: " + cluster.Count);
    //         }
    //
    //         // print("cluster count: " + cluster.Count);
    //     }
    // }
    //
    // private void FindCluster(int x, int y, string iconId)
    // {
    //     // 检查是否越界或者已经访问过
    //     if (x < 0 || x >= _rows || y < 0 || y >= _columns || visitedGrids[x, y])
    //         return;
    //
    //     if (!revealedGrids.Contains(new Vector2Int(x, y)))
    //         return;
    //
    //     // 如果当前格子的类型不匹配，直接返回
    //     if (iconItems[x, y].id != iconId)
    //         return;
    //
    //     // 标记为已访问
    //     visitedGrids[x, y] = true;
    //
    //     // 将当前格子加入到聚集中
    //     cluster.Add(new Vector2Int(x, y));
    //
    //     // 递归检查上下左右四个方向
    //     FindCluster(x + 1, y, iconId); // 右
    //     FindCluster(x - 1, y, iconId); // 左
    //     FindCluster(x, y + 1, iconId); // 上
    //     FindCluster(x, y - 1, iconId); // 下
    // }
}
