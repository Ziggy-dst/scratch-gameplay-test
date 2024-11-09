using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
public class MergerGridCover : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    // private BoxCollider2D _boxCollider2D;
    private bool isRevealed = false;
    private bool isRevealing = false;
    private bool isMerging = false;

    // private SpriteRenderer _mergerBG;

    public Vector2Int grid;

    private void OnEnable()
    {
        // IconManager.OnMergeStateChanged += => isMerging;
    }

    private void OnDisable()
    {
        // IconManager.OnMergeStateChanged -= => isMerging;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer.sortingOrder = 100;

        // _mergerBG = transform.Find("MergerBG").GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (isRevealing || isMerging) return;
        
        if (!isRevealed) _spriteRenderer.DOColor(Color.white, 0.1f);
        else
        {
            //Check cluster and display
            GridManager.onMouseOverRevealedItem?.Invoke(grid);
        }
    }

    private void OnMouseExit()
    {
        if (isRevealing || isMerging) return;
        
        if (!isRevealed) _spriteRenderer.DOColor(Color.gray, 0.1f);
        else
        {
            //Hide cluster
            GridManager.onMouseExitRevealedItem?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        // if (IconManager.isIconMoving) return;
        if (isRevealing || isMerging) return;

        if (isRevealed) GridManager.onMouseDownRevealedItem?.Invoke(grid);
        else RevealGrid();
    }

    private void RevealGrid()
    {
        isRevealing = true;
        
        //Generate Icon

        _spriteRenderer.DOFade(0, 0.1f).OnComplete((() =>
        {
            isRevealed = true;
            isRevealing = false;
            GridManager.onCoverRevealStateChanged?.Invoke(grid, true);
        }));
    }

    public void Reset()
    {
        isRevealed = false;
        _spriteRenderer.DOFade(1, 0);
        _spriteRenderer.DOColor(Color.gray, 0);
    }
}
