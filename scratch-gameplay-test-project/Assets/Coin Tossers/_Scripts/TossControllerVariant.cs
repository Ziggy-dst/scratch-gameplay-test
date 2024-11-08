using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TossControllerVariant : MonoBehaviour
{
    
    private bool _isTossing = false;
    private Vector2 _tossOrigin;
    private Vector2 _currentTargetPos;

    [Header("Basic")] 
    public GameObject crosshair;

    [Header("Toss Coin")] 
    public float tossTime;
    public float aimSpeed;
    public List<GameObject> coinPrefabs;

    [Header("Sound FX")] 
    public AudioClip tossSound;
    public AudioClip landSound;
    
    
    void Start()
    {
        _tossOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0));
        transform.position = new Vector3(_tossOrigin.x, _tossOrigin.y, transform.position.z);
    }

    void Update()
    {
        Rotate();
        if(Input.GetMouseButton(0)) Charge();
        if (Input.GetMouseButtonUp(0))
        {
            _currentTargetPos = crosshair.transform.position;
            Toss(_currentTargetPos);
            AudioSource.PlayClipAtPoint(tossSound, Vector2.zero);
            crosshair.transform.localPosition = Vector2.zero;
        }

        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Rotate()
    {
        Vector2 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _mouseWorldPos - (Vector2)transform.position);
    }

    void Charge()
    {
        _currentTargetPos = _tossOrigin;
        crosshair.transform.localPosition += Vector3.up * Time.deltaTime * aimSpeed;
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
