using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextFeedback : MonoBehaviour
{
    public float yDistance;
    public float duration;
    void Start()
    {
        TMP_Text tmp = GetComponent<TMP_Text>();
        
        //fade
        Sequence textFade = DOTween.Sequence();
        textFade
            .Append(tmp.DOFade(0, 0))
            .Append(tmp.DOFade(1, duration / 3))
            .AppendInterval(duration / 3)
            .Append(tmp.DOFade(0, duration / 3))
            .Play();
        
        //zoom
        tmp.transform.localScale = Vector3.zero;
        transform.DOScale(1, duration / 3);
        
        //move
        transform.DOMoveY(transform.position.y + yDistance, duration).OnComplete((() => Destroy(gameObject)));
        
    }
}
