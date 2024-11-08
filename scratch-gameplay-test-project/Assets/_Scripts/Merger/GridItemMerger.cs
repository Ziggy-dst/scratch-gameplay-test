using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Merger
{
    public class GridItemMerger
    {
        private GridItem[,] _items;
        private List<GridItem> _cluster;

        public GridItemMerger(GridItem[,] items, List<GridItem> cluster)
        {
            _items = items;
            _cluster = cluster;
        }

        public void Merge()
        {
            //TODO: IconManager.OnMergeStateChanged?.Invoke(true);

            //Get Current Cluster from somewhere

            //switch cluster number
            //case 1 return
            //case 2 merge 1
            //case 3-4 merge 2
            //case 5 merge 3
            //case 6-7 merge 4
            //case 8 merge 5
            //case 9 merge 6

            //Icons in cluster DOMove here
            //OnComplete =>
            //Destroy cluster Icons
            //Instantiate new Icons
            //Randomly DoMove to cluster grids

            //TODO: IconManager.OnMergeStateChanged?.Invoke(false);
            //IconManager Check new empty grids and reset cover
        }
    }
}