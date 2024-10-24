using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TMP")] 
    public TMP_Text score;

    public TMP_Text bet;
    
    private int _totalScore;
    private int _totalBets;
    
    private void OnEnable()
    {
        IconManager.OnScoreGained += UpdateScore;
        Coin.OnBetPlaced += UpdateBets;
    }

    private void OnDisable()
    {
        IconManager.OnScoreGained -= UpdateScore;
        Coin.OnBetPlaced -= UpdateBets;
    }
    
    void Start()
    {
        UpdateScore(0);
        UpdateBets(0);
    }

    void UpdateScore(int scoreGained)
    {
        _totalScore += scoreGained;
        score.DOText($"Score: {_totalScore}", 0.5f, true, ScrambleMode.Numerals);
    }

    void UpdateBets(int betPlaced)
    {
        _totalBets += betPlaced;
        bet.DOText($"Bets Placed: {_totalBets}", 0.5f, true, ScrambleMode.Numerals);
    }
}
