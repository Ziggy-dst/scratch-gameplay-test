using System;
using _Scripts.GridSystem;
using _Scripts.Merger;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MergerUIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject textFeedbackPrefab;
    
    private void OnEnable()
    {
        GridManager.onScoreCalculated += UpdateScore;
        GridItemMerger.onMergeResultGenerated += MergeFeedback;
    }

    private void OnDisable()
    {
        GridManager.onScoreCalculated -= UpdateScore;
        GridItemMerger.onMergeResultGenerated -= MergeFeedback;
    }

    private void UpdateScore(int score)
    {
        DOVirtual.Int(int.Parse(scoreText.text), score, 0.25f, (x => scoreText.text = $"{x}")).Play();
    }

    private void MergeFeedback(int levelGap)
    {
        string feedbackText = "";
        Color feedbackColor = Color.white;
        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        switch (levelGap)
        {
            case 0:
                feedbackText = "Failure!";
                feedbackColor = Color.blue;
                break;
            case 1:
                feedbackText = "Mini Success!";
                feedbackColor = Color.green;
                break;
            case 2:
                feedbackText = "Minor Success!";
                feedbackColor = Color.magenta;
                break;
            case 3:
                feedbackText = "Major Success!";
                feedbackColor = Color.yellow;
                break;
            case >=4:
                feedbackText = "Mega Success!";
                feedbackColor = Color.red;
                break;
        }

        GameObject textFeedback = Instantiate(textFeedbackPrefab, spawnPos, Quaternion.identity);
        TMP_Text tmp = textFeedback.GetComponent<TMP_Text>();
        tmp.color = feedbackColor;
        tmp.text = feedbackText;
    }
}