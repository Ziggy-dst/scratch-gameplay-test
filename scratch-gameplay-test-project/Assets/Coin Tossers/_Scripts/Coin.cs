using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin")] 
    public int price = 1;

    public static Action<int> OnBetPlaced;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Dunk()
    {
        transform.DOKill();

        Collider2D[] _hitList = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);

        if (_hitList.Length != 0)
        {
            float _minDistance = 9999;
            GameObject _targetGrid = _hitList[0].gameObject;
            
            foreach (var grid in _hitList)
            {
                float _distance = (grid.transform.position - transform.position).magnitude;
                if (_distance <= _minDistance) 
                {
                    _targetGrid = grid.gameObject;
                    _minDistance = _distance;
                }
            }
            _targetGrid.GetComponent<SpriteRenderer>()?.DOColor(Color.red, 0.25f).SetEase(Ease.Flash, 4, 0);
            TMP_Text gridTMP = _targetGrid.GetComponentInChildren<TMP_Text>();
            gridTMP.text = (Int32.Parse(gridTMP.text) + price).ToString();
            OnBetPlaced?.Invoke(price);
        }

        DOVirtual.DelayedCall(2, (() =>
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
                .OnComplete((() => Destroy(gameObject)));
        })).Play();
    }
}
