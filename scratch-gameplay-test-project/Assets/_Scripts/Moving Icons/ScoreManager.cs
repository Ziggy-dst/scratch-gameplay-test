using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _totalScore;

    private void OnEnable()
    {
        IconManager.OnScoreGained += GainScore;
    }

    private void OnDisable()
    {
        IconManager.OnScoreGained -= GainScore;
    }

    private void GainScore(int score)
    {
        _totalScore += score;
        print("this time you get: " + score);
        print("total score: " + _totalScore);
    }
}
