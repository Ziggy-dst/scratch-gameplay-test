using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public GridItem[,] items { get; set; }
    public MergerGridCover[,] covers { get; set; }
    public List<Vector2Int> revealedGrids { get; set; }
}
