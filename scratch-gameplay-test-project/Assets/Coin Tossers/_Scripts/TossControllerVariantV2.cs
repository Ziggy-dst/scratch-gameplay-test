using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TossControllerVariantV2 : MonoBehaviour
{
    
    private bool _isTossing = false;
    private Vector2 _tossOrigin;
    private Vector2 _currentTargetPos;
    private Vector2 _mouseWorldPos;
    private float _currentComboTimer = 0;

    [Header("Basic")] 
    public GameObject crosshair;

    [Header("Toss Coin")] 
    public float tossTime;
    public float aimSpeed;
    public List<GameObject> coinPrefabs;

    [Header("Sound FX")] 
    public AudioClip tossSound;
    public AudioClip landSound;

    [Header("Combo")] 
    public float comboThreshold = 1f;
    public float comboCD = 0.25f;
    
    
    void Start()
    {
        _tossOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0));
        transform.position = new Vector3(_tossOrigin.x, _tossOrigin.y, transform.position.z);
    }

    void Update()
    {
        Rotate();
        
        if (Input.GetMouseButtonDown(1))
        {
            Toss(_mouseWorldPos);
            AudioSource.PlayClipAtPoint(tossSound, Vector2.zero);
        }
        
        if (Input.GetMouseButton(1))
        {
            _currentComboTimer += Time.deltaTime;
            if (_currentComboTimer >= comboThreshold)
            {
                Toss(_mouseWorldPos);
                AudioSource.PlayClipAtPoint(tossSound, Vector2.zero);
                _currentComboTimer -= comboCD;
            }
        }

        if (Input.GetMouseButtonUp(1)) _currentComboTimer = 0;

        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Rotate()
    {
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _mouseWorldPos - (Vector2)transform.position);
    }

    void Toss(Vector2 targetPos)
    {
        _isTossing = true;

        GameObject _currentCoin = Instantiate(coinPrefabs?[Random.Range(0, coinPrefabs.Count)], _tossOrigin, Quaternion.identity);
        _currentCoin.transform.DOMove(targetPos, tossTime).SetEase(Ease.Linear).OnComplete((() =>
        {
            _currentCoin.GetComponent<Coin>().Dunk();
            AudioSource.PlayClipAtPoint(landSound, Vector2.zero);
        }));
    }
}
