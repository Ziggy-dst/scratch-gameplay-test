using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.GridSystem;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public static Action<int> onMergeResultGenerated;

        public LeveledUpGridItemSpawnData CheckMerge(List<Vector2Int> cluster, Vector2Int mergeOrigin)
        {
            //TODO: IconManager.OnMergeStateChanged?.Invoke(true);
            LeveledUpGridItemSpawnData leveledUpGridItemSpawnData;

            int clusterCount = cluster.Count;

            int countAfterMerge = 0;

            int levelAfterMerge = 0;

            float randomMergeSeed = Random.Range(0f, 1f);
            
            GridItem currentGridItem = _gridData.items[cluster[0].x, cluster[0].y];

            int currentLevel = currentGridItem.GridItemData.level;

            GridItemType currentItemType = currentGridItem.type;

            // TODO: count after merge bug: 6 =after merge=> 4
            //Get Current Cluster from somewhere
            switch (clusterCount)
            {
                case 2:
                    countAfterMerge = 1;
                    switch (randomMergeSeed)
                    {
                        case <= 0.5f:
                            levelAfterMerge = currentLevel;
                            break;
                        case <= 1:
                            levelAfterMerge = currentLevel + 1;
                            break;
                    }
                    break;
                case 3:
                    switch (randomMergeSeed)
                    {
                        case <= 0.25f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel;
                            break;
                        case <= 0.75f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 1;
                            break;
                        case <= 1f:
                            countAfterMerge = 1;
                            levelAfterMerge = currentLevel + 2;
                            break;
                    }
                    break;
                case 4:
                    switch (randomMergeSeed)
                    {
                        case <= 0.25f:
                            countAfterMerge = 3;
                            levelAfterMerge = currentLevel;
                            break;
                        case <= 0.75f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 1;
                            break;
                        case <= 0.9f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 2;
                            break;
                        case <= 1f:
                            countAfterMerge = 1;
                            levelAfterMerge = currentLevel + 3;
                            break;
                    }
                    break;
                case 5:
                    switch (randomMergeSeed)
                    {
                        case <= 0.2f:
                            countAfterMerge = 4;
                            levelAfterMerge = currentLevel;
                            break;
                        case <= 0.6f:
                            countAfterMerge = 3;
                            levelAfterMerge = currentLevel + 1;
                            break;
                        case <= 0.85f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 2;
                            break;
                        case <= 1f:
                            countAfterMerge = 1;
                            levelAfterMerge = currentLevel + 3;
                            break;
                    }
                    break;
                case 6:
                    switch (randomMergeSeed)
                    {
                        case <= 0.5f:
                            countAfterMerge = 4;
                            levelAfterMerge = currentLevel + 1;
                            break;
                        case <= 0.8f:
                            countAfterMerge = 3;
                            levelAfterMerge = currentLevel + 2;
                            break;
                        case <= 0.95f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 3;
                            break;
                        case <= 1f:
                            countAfterMerge = 1;
                            levelAfterMerge = currentLevel + 4;
                            break;
                    }
                    break;
                case 7:
                    switch (randomMergeSeed)
                    {
                        case <= 0.5f:
                            countAfterMerge = 3;
                            levelAfterMerge = currentLevel + 2;
                            break;
                        case <= 0.8f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 3;
                            break;
                        case <= 1f:
                            countAfterMerge = 1;
                            levelAfterMerge = currentLevel + 4;
                            break;
                    }
                    break;
                case 8:
                    switch (randomMergeSeed)
                    {
                        case <= 0.5f:
                            countAfterMerge = 4;
                            levelAfterMerge = currentLevel + 2;
                            break;
                        case <= 0.8f:
                            countAfterMerge = 3;
                            levelAfterMerge = currentLevel + 3;
                            break;
                        case <= 1f:
                            countAfterMerge = 2;
                            levelAfterMerge = currentLevel + 4;
                            break;
                    }
                    break;
                case 9:
                    countAfterMerge = 1;
                    levelAfterMerge = currentLevel + 10;
                    break;
            }

            levelAfterMerge = Mathf.Min(5, levelAfterMerge);
            int levelGap = levelAfterMerge - currentLevel;
            onMergeResultGenerated?.Invoke(levelGap);
           
            GridItemData leveledUpGridItemData = _gridItemSo.itemPool[currentItemType].itemLevelData[levelAfterMerge];
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