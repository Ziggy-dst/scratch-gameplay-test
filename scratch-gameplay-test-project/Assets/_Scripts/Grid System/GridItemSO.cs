using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridItemSO", menuName = "Scriptable Objects/GridItemSO")]
public class GridItemSO : ScriptableObject
{
    public List<ItemData> itemPool = new List<ItemData>();
}


