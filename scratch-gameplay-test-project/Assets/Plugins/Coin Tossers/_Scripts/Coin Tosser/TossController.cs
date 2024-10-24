using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TossController : MonoBehaviour
{
    
    private bool _isTossing = false;
    private Vector2 _tossOrigin;
    private GameObject _currentCoin;
    
    [Header("Toss Coin")]
    public float tossSpeed;
    public float tossDistance;
    public List<GameObject> coinPrefabs;

    [Header("Sound FX")] 
    public AudioClip tossSound;
    public AudioClip dunkSound;
    
    
    void Start()
    {
        _tossOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isTossing)
            {
                Dunk();
                AudioSource.PlayClipAtPoint(dunkSound, Vector2.zero);
            }
            else
            {
                Toss();
                AudioSource.PlayClipAtPoint(tossSound, Vector2.zero);
            }
        }

        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Toss()
    {
        _isTossing = true;

        Vector2 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 _tossDir = (_mouseWorldPos - _tossOrigin).normalized;
        Vector2 _tossTarget = _tossOrigin + _tossDir * tossDistance;

        _currentCoin = Instantiate(coinPrefabs?[Random.Range(0, coinPrefabs.Count)], _tossOrigin, Quaternion.identity);
        _currentCoin.transform.DOMove(_tossTarget, tossDistance / tossSpeed).SetEase(Ease.InOutQuad).OnComplete((() =>
        {
            Destroy(_currentCoin.gameObject);
            _currentCoin = null;
            _isTossing = false;
        }));
    }

    void Dunk()
    {
        _currentCoin.transform.DOKill();

        Collider2D[] _hitList = Physics2D.OverlapCircleAll(_currentCoin.transform.position, _currentCoin.transform.localScale.x / 2);

        if (_hitList.Length != 0)
        {
            float _minDistance = 9999;
            GameObject _targetGrid = _hitList[0].gameObject;
            
            foreach (var grid in _hitList)
            {
                float _distance = (grid.transform.position - _currentCoin.transform.position).magnitude;
                if (_distance <= _minDistance) 
                {
                    _targetGrid = grid.gameObject;
                    _minDistance = _distance;
                }
                print($"Grid: {grid.name}\nDistance: {_distance}");
            }

            _targetGrid.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.125f).SetLoops(2, LoopType.Yoyo);
        }
        
        Destroy(_currentCoin.gameObject);
        _currentCoin = null;
        _isTossing = false;
    }
}
