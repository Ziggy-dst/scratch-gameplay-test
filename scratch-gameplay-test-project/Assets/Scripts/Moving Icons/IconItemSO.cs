using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconItemSO", menuName = "Scriptable Objects/IconItemSO")]
public class IconItemSO : ScriptableObject
{
    public List<IconItem> iconPool = new List<IconItem>();

    [Serializable]
    public struct IconItem
    {
        public string id;
        public string name;
        public Sprite image;
        public int prize;
        [Tooltip("probability must be within 0.0-1.0, cumulative probability must be 1")]
        public float probability;
    }
}
