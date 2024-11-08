using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
public class MergerGridCover : GridCover
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private bool isRevealed = false;
    private bool isRevealing = false;
    private bool isMerging = false;

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
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer.sortingOrder = 100;
    }

    private void OnMouseEnter()
    {
        if (isRevealing || isMerging) return;
        
        if (!isRevealed) _spriteRenderer.DOColor(Color.white, 0.1f);
        else
        {
            //Check cluster and display
        }
    }

    private void OnMouseExit()
    {
        if (isRevealing || isMerging) return;
        
        if (!isRevealed) _spriteRenderer.DOColor(Color.gray, 0.1f);
        else
        {
            //Hide cluster
        }
    }

    private void OnMouseDown()
    {
        // if (IconManager.isIconMoving) return;
        if (isRevealing || isMerging) return;
        
        RevealGrid();
        Merge();
    }

    public void RevealGrid()
    {
        if (isRevealed) return;
        isRevealing = true;
        
        //Generate Icon
        
        _spriteRenderer.DOFade(0, 0.1f).OnComplete((() =>
        {
            isRevealed = true;
            isRevealing = false;
            IconManager.OnCoverRevealed?.Invoke(grid);
        }));
    }

    public void Merge()
    {
        if (!isRevealed) return;
        
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
        //Randomly DoMove to grids
        
        //TODO: IconManager.OnMergeStateChanged?.Invoke(false);
        //IconManager Check new empty grids and reset cover
    }
}
