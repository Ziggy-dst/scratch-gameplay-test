using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GridItemSO", menuName = "Scriptable Objects/GridItemSO")]
public class GridItemSO : SerializedScriptableObject
{
    public Dictionary<GridItemType, GridTypeData> itemPool = new Dictionary<GridItemType, GridTypeData>();
}

[Serializable]
public struct GridTypeData
{
    public List<ItemData> itemLevelData;
    public float typeProbability;
}


