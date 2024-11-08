using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
public class GridCover : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private bool isRevealed = false;

    public Vector2Int grid;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
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
        _boxCollider2D.enabled = false;
        IconManager.OnCoverRevealed?.Invoke(grid);
        transform.parent.DetachChildren();
    }
}
