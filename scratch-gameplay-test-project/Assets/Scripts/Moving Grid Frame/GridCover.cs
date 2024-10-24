using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
public class GridCover : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool isRevealed = false;

    public Vector2Int grid;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = 100;
    }

    private void OnMouseDown()
    {
        if (IconManager.isIconMoving) return;
        if (isRevealed) return;
        isRevealed = true;
        RevealGrid();
    }

    private void RevealGrid()
    {
        _spriteRenderer.enabled = false;
        IconManager.OnCoverRevealed?.Invoke(grid);
    }
}
