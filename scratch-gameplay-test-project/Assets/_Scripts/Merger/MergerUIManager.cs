using System;
using _Scripts.GridSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MergerUIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    
    private void OnEnable()
    {
        GridManager.onScoreCalculated += UpdateScore;
    }

    private void OnDisable()
    {
        GridManager.onScoreCalculated -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        // scoreText.DOText($"{score}", 0.25f, true, ScrambleMode.Numerals);
        DOVirtual.Int(int.Parse(scoreText.text), score, 0.25f, (x => scoreText.text = $"{x}")).Play();
    }
}