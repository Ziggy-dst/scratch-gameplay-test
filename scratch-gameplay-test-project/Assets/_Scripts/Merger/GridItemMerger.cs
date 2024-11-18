using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.GridSystem;
using UnityEngine;

namespace _Scripts.Merger
{
    public struct LeveledUpGridItemSpawnData
    {
        public GridItemType itemType;
        public GridItemData GridItemData;
        public List<Vector2Int> mergedGrids;
    }

    public class GridItemMerger
    {
        private GridItemSO _gridItemSo;
        private GridData _gridData;

        public GridItemMerger(GridItemSO gridItemSo, GridData gridData)
        {
            _gridItemSo = gridItemSo;
            _gridData = gridData;
        }

        public LeveledUpGridItemSpawnData CheckMerge(List<Vector2Int> cluster, Vector2Int mergeOrigin)
        {
            //TODO: IconManager.OnMergeStateChanged?.Invoke(true);
            LeveledUpGridItemSpawnData leveledUpGridItemSpawnData;

            int clusterCount = cluster.Count;

            int countAfterMerge = 0;
            //Get Current Cluster from somewhere
            switch (clusterCount)
            {
                case 2:
                    //get merge result
                    countAfterMerge = 1;
                    //clear all items in cluster
                    //pick random items to become result
                    break;
                case <= 4:
                    countAfterMerge = 2;
                    break;
                case 5:
                    countAfterMerge = 3;
                    break;
                case <= 7:
                    countAfterMerge = 4;
                    break;
                case 8:
                    countAfterMerge = 5;
                    break;
                case 9:
                    countAfterMerge = 6;
                    break;
            }

            GridItem currentGridItem = _gridData.items[cluster[0].x, cluster[0].y];

            int currentLevel = currentGridItem.GridItemData.level;

            GridItemType currentItemType = currentGridItem.type;
            GridItemData leveledUpGridItemData = _gridItemSo.itemPool[currentItemType].itemLevelData[currentLevel+1];
            var mergedGrids = GetMergeGrid(cluster, mergeOrigin, countAfterMerge);

            leveledUpGridItemSpawnData.itemType = currentItemType;
            leveledUpGridItemSpawnData.GridItemData = leveledUpGridItemData;
            leveledUpGridItemSpawnData.mergedGrids = mergedGrids;

            return leveledUpGridItemSpawnData;
            

            //Icons in cluster DOMove here
            //OnComplete =>
            //Destroy cluster Icons
            //Instantiate new Icons
            //Randomly DoMove to cluster grids

            //TODO: IconManager.OnMergeStateChanged?.Invoke(false);
            //IconManager Check new empty grids and reset cover
        }

        private List<Vector2Int> GetMergeGrid(List<Vector2Int> cluster, Vector2Int mergeOrigin, int count)
        {
            // get spawn grid position
            if (count > cluster.Count)
                throw new ArgumentException("Requested count is greater than the list size.");

            List<Vector2Int> mergedGrids = new List<Vector2Int>();

            foreach (var grid in cluster)
            {
                if (mergedGrids.Count == count) break;
                if (Vector2.Distance(grid, mergeOrigin) <= Mathf.Sqrt(2)) mergedGrids.Add(grid);
            }

            return mergedGrids;
            // return cluster.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        }
    }
}