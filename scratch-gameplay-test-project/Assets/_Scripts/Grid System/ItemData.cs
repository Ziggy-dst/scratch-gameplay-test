using UnityEngine;
using System;

[Serializable]
public struct ItemData
{
    public string id;
    public GridItemType type;
    public int level;
    public Sprite image;
    public int prize;
    [Tooltip("probability must be within 0.0-1.0, cumulative probability must be 1")]
    public float probability;
}
